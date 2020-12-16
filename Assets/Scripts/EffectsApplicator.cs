using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EffectsApplicator : MonoBehaviour
{

    public static EffectsApplicator instance;

    public float pickupDistance = 3;
    public float pickupSphereRadius = 0.5f;
    public float charge;
    //public bool canShoot = true;
    public bool action;
    public bool babaMode = false;
    public GameObject bullet;
    public Transform bulletSpawner;

    [Header("Weapon")]
    public WeaponScript weapon;
    public Transform weaponHolder;
    public LayerMask weaponLayer;


    [Space]
    [Header("Prefabs")]
    public GameObject hitParticlePrefab;
    public GameObject bulletPrefab;
    public Image weaponCursor;

    private GameObject hud;
    private RuleHandler ruleHandler;
    private BabaWorld babaWorld;


    // Time constants
    // Constants
    private float timeFastConst = 1f;
    private float timeSlowConst = 0.00003f; //0.00003f

    private float lerpSlow = .05f;
    private float lerpFastest = .5f;
    private float lerpFast = .1f;

    [Header("DO NOT TOUCH")]
    public List<GameObject> bulletList;


    private void Awake()
    {
        instance = this;
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weapon = weaponHolder.GetComponentInChildren<WeaponScript>();
    }


    void Start()
    {
      //IMPORTANT: set framerate or the speed will be whatever the machine can handle
      Application.targetFrameRate = 60;


      hud = (GameObject) GameObject.Find("HUD_Baba");
      ruleHandler = GetComponent<RuleHandler>();
      babaWorld = GetComponent<BabaWorld>();


      bulletList = new List<GameObject>();

      // Disable Canvas at first
      // if(babaMode)
      // {
      //   hud.SetActive(true);
      // }
      // else
      // {
      //   hud.SetActive(false);
      // }
    }

    // Update is called once per frame
    void Update()
    {

      ruleHandler.CalculateRules();
      ApplyEffects();
      //ruleHandler.CalculateRules();
      //ApplyEffects();
        //ApplyEffects();

      UpdateCursor();

    }

    public void ApplyEffects()
    {

      //Switch baba mode
      if(Input.GetKeyDown(KeyCode.E))
      {
        babaMode = !babaMode;
        if(babaMode)
        {
          hud.SetActive(true);
          babaWorld.UpdateDisplay();
        }
        else
        {
          hud.SetActive(false);
        }
      }


        //Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }


        int bulletAmount = -1;
        if(weapon != null)bulletAmount = weapon.bulletAmount;
        //Shoot
        bool canShoot = ruleHandler.CheckAssert("You is Shoot") &&  bulletAmount > 0;
        bool canThrow = true;

        bool askShoot = Input.GetMouseButtonDown(0);
        bool askThrow = (Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && bulletAmount <= 0));

        if (!babaMode && canShoot)
        {
            // Check wants to shoot or is forced too
            if (askShoot || (ruleHandler.CheckEffect("You is Shoot"))) //Forced shooting
            {
                // Might have to rework how time affects this
                StopCoroutine(ShootWaitCoroutine(.03f));
                StartCoroutine(ShootWaitCoroutine(.03f));
                if (weapon != null)
                {
                    weapon.Shoot(SpawnPos(), Camera.main.transform.rotation, false);
                    GameObject lastBullet = weapon.lastBullet;

                    ruleHandler.triggerFrame["You is Shoot"] = 30;
                }
            }
        }

        //Throw
        //bool canThrow = !action;
        if ( !babaMode && askThrow && canThrow)
        {
            /*StopCoroutine(ShootWaitCoroutine(.4f));
            StartCoroutine(ShootWaitCoroutine(.4f));*/

            if(weapon != null)
            {
                weapon.Throw();
                weapon = null;
            }
        }

        //Pick up weapons
        RaycastHit hit;

        //try with ray first then Sphere
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupDistance, weaponLayer))
        {
            if (Input.GetMouseButtonDown(0) && weapon == null)
            {
                hit.transform.GetComponent<WeaponScript>().Pickup();
            }
        }
        else if(Physics.SphereCast(Camera.main.transform.position, pickupSphereRadius, Camera.main.transform.forward, out hit, pickupDistance, weaponLayer))
        {
            if (Input.GetMouseButtonDown(0) && weapon == null)
            {
                hit.transform.GetComponent<WeaponScript>().Pickup();
            }
        }



        // Time move rules + check game not paused
        bool timeIsMove = !babaMode && ruleHandler.CanXMove("Time");

        float timeSpeed;
        float lerpTime;
        if(timeIsMove)
        {
          timeSpeed = timeFastConst;
          lerpTime = lerpSlow;
        }
        else
        {
          timeSpeed = timeSlowConst;
          lerpTime = lerpFast;
        }


        //Smoothen time acceleration or deceleration
        Time.timeScale = Mathf.Lerp(Time.timeScale, timeSpeed, lerpTime);

      //Shoot is You effect & trigger

      if(!babaMode && ruleHandler.CheckEffectAndAssert("Shoot is You"))
      {
        if(bulletList.Count > 0)
        {
          GameObject lastShotBullet = bulletList[bulletList.Count -1];
          gameObject.transform.position = lastShotBullet.transform.position;

          ruleHandler.triggerFrame["Shoot is You"] = 1;
        }

      }
    }


    IEnumerator ShootWaitCoroutine(float time)
    {
        action = true;
        yield return new WaitForSecondsRealtime(time);
        action = false;
    }

    //Spawn position for bullets
    Vector3 SpawnPos()
    {
        return Camera.main.transform.position + (Camera.main.transform.forward * .5f) + (Camera.main.transform.up * -.02f);
    }


    // Gun UI stuff, could be placed somewhere else ideally
    public void ResetCursor()
    {
      weaponCursor.transform.localEulerAngles = new Vector3(0,0,0);
      weaponCursor.transform.localScale = WeaponScript.baseScale;
    }

    public void UpdateCursor()
    {
      if(weapon == null)
      {
        ResetCursor();
      }
      else
      {
        weaponCursor.transform.localEulerAngles = weapon.cursorTransform.transform.localEulerAngles;
        weaponCursor.transform.localScale = weapon.cursorTransform.transform.localScale;
      }
    }


}
