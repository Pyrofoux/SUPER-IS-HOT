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
    public bool fading;

    public bool babaMode = true;
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


    private GameObject hud;
    private RuleHandler ruleHandler;
    private BabaWorld babaWorld;
    private FpsRenderer fpsRenderer;


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
      fpsRenderer = GetComponent<FpsRenderer>();


      bulletList = new List<GameObject>();

      DisplayMenu();

    }

    // Update is called once per frame
    void Update()
    {

      ruleHandler.CalculateRules();
      ApplyEffects();
      //ruleHandler.CalculateRules();
      //ApplyEffects();
        //ApplyEffects();

      fpsRenderer.UpdateCursor();
      CheckWinLoose();

    }

    public void CheckWinLoose()
    {
      if(ruleHandler.CheckEffectAndAssert("You is Dead"))
      {
        fpsRenderer.Die();
      }
    }

    public void DisplayMenu()
    {
      if(babaMode)
      {
        babaWorld.OpenDisplay();
        fpsRenderer.CloseDisplay();
      }
      else
      {
        babaWorld.CloseDisplay();
        fpsRenderer.OpenDisplay();
      }
    }

    public void ApplyEffects()
    {

      if(fading) return;

      //Switch baba mode
      if(Input.GetKeyDown(KeyCode.E))
      {
        babaMode = !babaMode;
        DisplayMenu();
      }


        //Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            //fpsRenderer.Win();
            fpsRenderer.ReloadLevel();
        }

        if(Input.GetKeyUp(KeyCode.U))
        {
          babaWorld.Undo();
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

            ThrowWeapon();
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

        //Let time moving when transitionning
        //if(fading) timeIsMove = true;

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

    public void ThrowWeapon()
    {
      if(weapon != null)
      {
          weapon.Throw();
          weapon = null;
      }
    }

    IEnumerator ShootWaitCoroutine(float time)
    {
        //action = true;
        yield return new WaitForSecondsRealtime(time);
        //action = false;
    }

    //Spawn position for bullets
    Vector3 SpawnPos()
    {
        return Camera.main.transform.position + (Camera.main.transform.forward * .5f) + (Camera.main.transform.up * -.02f);
    }


    public void PlaySound(string soundName)
    {
      fpsRenderer.PlaySound(soundName);
    }

    public bool CheckTimeMoves()
    {
      return ruleHandler.CanXMove("Time");
    }




}
