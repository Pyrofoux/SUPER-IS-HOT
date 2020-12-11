using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[SelectionBase]
public class EnemyScript : MonoBehaviour
{
    Animator anim;

    [Header("Prefabs")]
    public Transform weaponHolder;
    public TextMeshPro wordDisplayed;

    [Header("State")]
    public string lockWord = "";
    public bool dead;
    private BabaWorld babaWorld;

    private float stopped_time = 0.01f;
    private float rotSpeed = 0.0002f;


    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(RandomAnimation());

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

    }

    void Update()
    {
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

        if(dead) return; //Fix cause Kill() will be killed multiple times during the whole collision

        //Ragdoll
        anim.enabled = false;
        BodyPartScript[] parts = GetComponentsInChildren<BodyPartScript>();
        foreach (BodyPartScript bp in parts)
        {
            //Each part becomes affected by physics
            bp.rb.isKinematic = false;
            bp.rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        // Throw gun towards player
        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
        {
            WeaponScript w = weaponHolder.GetComponentInChildren<WeaponScript>();
            w.Release();
        }

        if(lockWord != "" && lockWord != "none")
        {
          string titleCaseWord = lockWord[0].ToString().ToUpper()+lockWord.Substring(1).ToLower();
          babaWorld.UnlockNext(titleCaseWord);
          Debug.Log("Unlocked "+titleCaseWord);
        }

        //Unlock word
        Destroy(wordDisplayed);

        dead = true;
    }

    public void Shoot()
    {
        if (dead)
            return;

        if (weaponHolder.GetComponentInChildren<WeaponScript>() != null)
            weaponHolder.GetComponentInChildren<WeaponScript>().Shoot(GetComponentInChildren<ParticleSystem>().transform.position, transform.rotation, true);
    }

    IEnumerator RandomAnimation()
    {
        anim.enabled = false;
        yield return new WaitForSecondsRealtime(Random.Range(.1f, .5f));
        anim.enabled = true;

    }
}
