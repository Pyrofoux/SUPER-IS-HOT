using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[SelectionBase]
public class EnemyScript : MonoBehaviour
{
    Animator anim;

    public AudioClip EnnemyDeathClip;
    public AudioClip GunShotClip;

    [Header("Prefabs")]
    public Transform weaponHolder;
    public TextMeshPro wordDisplayed;

    [Header("State")]
    public string lockWord = "";
    public float sightDistance = 8f;
    public LayerMask obstacleLayer;
    public bool dead;


    private BabaWorld babaWorld;

    private float stopped_time = 0.01f;
    private float rotSpeed = 0.000002f;
    private float maxRandomDelay = 30f;
    private float randomDelay;

    private AudioSource audioSource;


    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
        anim.enabled = true;

        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weaponHolder.GetComponentInChildren<WeaponScript>().active = false;

        babaWorld = (BabaWorld)GameObject.FindObjectOfType(typeof(BabaWorld));

        if(lockWord != "" && lockWord != "none")
        {
          wordDisplayed.text = "["+lockWord.ToUpper()+"]";
        }
        else
        {
          wordDisplayed.text = "";
        }

        // Might cause some T-posing
        randomDelay = Random.Range(0, maxRandomDelay);
    }

    void Update()
    {

        //randomdelay before shooting
        if(randomDelay > 0)
        {
          anim.enabled = false;
          randomDelay = randomDelay - 1;
          if(randomDelay < 0) anim.enabled = true;
          return ;
        }

        // Follow player
        if(!dead && Time.deltaTime > stopped_time)
        {
          Vector3 destination = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);

          // Smoothly rotate towards the target point.
          transform.LookAt(Vector3.Slerp(destination, transform.position, rotSpeed*Time.deltaTime));
        }




    }

    public void Kill()
    {

         //Fix cause Kill() will be killed multiple times during the whole collision

        //Ragdoll
        anim.enabled = false;
        BodyPartScript[] parts = GetComponentsInChildren<BodyPartScript>();
        foreach (BodyPartScript bp in parts)
        {
            //Each part becomes affected by physics
            bp.rb.isKinematic = false;
            bp.rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        if(dead) return;
        // Throw gun towards player
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
        {
            WeaponScript w = weaponHolder.GetComponentInChildren<WeaponScript>();
            w.Release();
        }

        //Play ennemy death sound
        audioSource.PlayOneShot(EnnemyDeathClip, 0.7f);


        // Hide word
        if(lockWord != "" && lockWord != "none")
        {
          //string titleCaseWord = lockWord[0].ToString().ToUpper()+lockWord.Substring(1).ToLower();
          babaWorld.UnlockNext(lockWord);
          Debug.Log("Unlocked "+lockWord);
        }

        //Unlock word
        Destroy(wordDisplayed);

        dead = true;
    }

    public void Shoot()
    {
        // Check if dead
        if (dead)
            return;

        //Check line of sight
        RaycastHit hit;
        if(Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, sightDistance, obstacleLayer))
        return ;

        // Actually shoot
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weaponHolder.GetComponentInChildren<WeaponScript>().Shoot(GetComponentInChildren<ParticleSystem>().transform.position, transform.rotation, true);

        // Play gunshot sound
        audioSource.PlayOneShot(GunShotClip, 0.5f);
    }
}
