using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SuperHotScript : MonoBehaviour
{

    public static SuperHotScript instance;

    public float pickupDistance = 5;
    public float charge;
    public bool canShoot = true;
    public bool action;
    public bool babaMode = false;
    public GameObject bullet;
    public Transform bulletSpawner;

    [Header("Weapon")]
    public WeaponScript weapon;
    public Transform weaponHolder;
    public LayerMask weaponLayer;


    [Space]
    [Header("UI")]
    public Image indicator;

    [Space]
    [Header("Prefabs")]
    public GameObject hitParticlePrefab;
    public GameObject bulletPrefab;

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


        //Retstart
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        //Shoot
        if (!babaMode && canShoot)
        {
            // Check wants to shoot or is forced too
            if (Input.GetMouseButtonDown(0) || (ruleHandler.CheckEffect("You is Shoot"))) //Forced shooting
            {
                // Might have to rework how time affects this
                StopCoroutine(ActionE(.03f));
                StartCoroutine(ActionE(.03f));
                if (weapon != null)
                {
                    weapon.Shoot(SpawnPos(), Camera.main.transform.rotation, false);
                    GameObject lastBullet = weapon.lastBullet;
                }
            }
        }

        //Throw
        if (Input.GetMouseButtonDown(1))
        {
            StopCoroutine(ActionE(.4f));
            StartCoroutine(ActionE(.4f));

            if(weapon != null)
            {
                weapon.Throw();
                weapon = null;
            }
        }

        //Pick up weapons
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupDistance, weaponLayer))
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

        // Old logic code
        //float timeSpeed = moving ? timeFastConst : timeSlowConst;
        //float lerpTime = moving ? lerpSlow : lerpFast;

        //timeSpeed = action ? timeFastConst : timeSpeed;
        //lerpTime = action ? lerpFast : lerpTime;


        CheckTeleport();


    }

    public void CheckTeleport()
    {
      if(ruleHandler.CheckEffectAndAssert("Shoot is You"))
      {
        if(bulletList.Count > 0)
        {
          GameObject lastShotBullet = bulletList[bulletList.Count -1];
          gameObject.transform.position = lastShotBullet.transform.position;
        }

      }
    }


    IEnumerator ActionE(float time)
    {
        action = true;
        yield return new WaitForSecondsRealtime(time);
        action = false;
    }

    //Reloading icon
    public void ReloadUI(float time)
    {
        indicator.transform.DORotate(new Vector3(0, 0, 90), time, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(() => indicator.transform.DOPunchScale(Vector3.one / 3, .2f, 10, 1).SetUpdate(true));
    }


    //Spawn position for bullets
    Vector3 SpawnPos()
    {
        return Camera.main.transform.position + (Camera.main.transform.forward * .5f) + (Camera.main.transform.up * -.02f);
    }


}
