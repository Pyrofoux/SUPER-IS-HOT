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
    public bool fading = false;
    private bool forceTimeStop = false;

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


    private RuleHandler ruleHandler;
    private BabaWorld babaWorld;
    private FpsRenderer fpsRenderer;
    private LevelManager levelManager;


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

        // Detect starting weapon
        instance = this;
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weapon = weaponHolder.GetComponentInChildren<WeaponScript>();
    }


    void Start()
    {
      //IMPORTANT: set framerate or the speed will be whatever the machine can handle
      Application.targetFrameRate = 60;


      GameObject levelManagerGo = GameObject.FindWithTag("LevelManager");
      if(levelManagerGo != null)
      {
        levelManager = levelManagerGo.GetComponent<LevelManager>();
      }


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

      fpsRenderer.UpdateCursor();
      CheckWinLoose();

    }

    public void CheckWinLoose()
    {
      // No winning or losing while in Baba mode
      if(babaMode) return;

      if(ruleHandler.CheckEffectAndAssert("Super is Hot"))
      {
        fpsRenderer.Win();
      }
      else if(ruleHandler.CheckEffectAndAssert("You is Dead"))
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

      if(fading)
      {
          ChangeTimeSpeed();
          return;
      }


      CheckKeysInput();

      CheckGunActions();

      ChangeTimeSpeed();

      ShootIsYouTrigger();
    }


    public void CheckKeysInput()
    {
      if(fading) return; // No actions during fading

      //Special actions during title screen
      if(isTitleScreen())
      {
        if(Input.GetKeyDown(KeyCode.E))
        {
          int selected = babaWorld.getSelectedLevelId();
          if(selected != -1)
          {
            int current = levelManager.currentLevel;
            levelManager.SwitchLevel(selected);
            fpsRenderer.LoadLevel(current, selected);
          }
        }

        if(Input.GetKeyDown(KeyCode.Escape)) // Quit when pressed Escape on title screen
        {
          Application.Quit();
        }

        return;
      }


      //In-game actions

      if(Input.GetKeyDown(KeyCode.Escape))
      {
        int current = levelManager.currentLevel;
        levelManager.SwitchLevel(0);
        fpsRenderer.LoadLevel(current, 0);
      }

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

      //  Undo
      if(Input.GetKeyUp(KeyCode.U))
      {
        babaWorld.Undo();
      }
    }

    public void CheckGunActions()
    {
      int bulletAmount = -1;
      bool realizedEmpty = true;
      if(weapon != null)bulletAmount = weapon.bulletAmount;
      if(weapon != null)realizedEmpty = weapon.realizedEmpty;
      //Shoot
      bool canShoot = ruleHandler.CheckEffectAndAssert("You is Shoot")  &&  bulletAmount > 0;
      bool canThrow = true;

      bool askShoot = Input.GetMouseButtonDown(0);
      bool askThrow = (Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && bulletAmount <= 0 && realizedEmpty));

      // Shooting
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

      if(!babaMode && askShoot && bulletAmount == 0 && !weapon.realizedEmpty)
      {
        weapon.realizedEmpty = true;
        fpsRenderer.PlaySound("gun empty");
      }

      // Throwing
      if ( !babaMode && askThrow && canThrow)
      {
          /*StopCoroutine(ShootWaitCoroutine(.4f));
          StartCoroutine(ShootWaitCoroutine(.4f));*/

          ThrowWeapon();
      }

      //Picking up
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
    }

    public void ShootIsYouTrigger()
    {
      if(!babaMode && ruleHandler.CheckEffectAndAssert("Shoot is You"))
      {
        if(bulletList.Count > 0)
        {
          // Apply Shoot is You effect
          GameObject lastShotBullet = bulletList[bulletList.Count -1];
          gameObject.transform.position = lastShotBullet.transform.position;

          // Apply Shoot is You trigger
          ruleHandler.triggerFrame["Shoot is You"] = 1;
        }

      }
    }

    public void ChangeTimeSpeed()
    {
      // Time move rules + check game not paused
      bool movingTime = !babaMode && ruleHandler.CanXMove("Time");

      if(forceTimeStop)
      {
        movingTime = false;
      }


      float timeSpeed;
      float lerpTime;
      if(movingTime)
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

    public void ManualTimeStop(bool stop)
    {
      forceTimeStop = stop;
    }

    public bool isTitleScreen()
    {
      return levelManager.isTitleScreen();
    }

    public string getNextLevel()
    {
      levelManager.SwitchNextLevel();
      return "Level-"+levelManager.currentLevel.ToString();
    }


}
