using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    Rigidbody rb;
    RuleHandler ruleHandler;
    SuperHotScript superhotScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ruleHandler = (RuleHandler)GameObject.FindObjectOfType(typeof(RuleHandler));

        //get time handling script -- for game pause
        superhotScript = (SuperHotScript)GameObject.FindObjectOfType(typeof(SuperHotScript));

    }

    // Update is called once per frame
    void Update()
    {
        if(!gameObject.CompareTag("PlayerBullet")) // Ennemy bullet behave normally
        {
          transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
          if(gameObject.CompareTag("PlayerBullet") && !superhotScript.babaMode) // Your bullets are affected by rule changes
          {
            if(ruleHandler.CanXMove("Shoot"))
            {
              transform.position += transform.forward * speed * Time.fixedDeltaTime;
              CheckCollisionManual();
            }
          }
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
      CollisionEffect(collision.gameObject);
    }

    private void CheckCollisionManual()
    {
      //Use the OverlapBox to detect if there are any other colliders within this box area.
            //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
            Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity);

            //Check when there is a new collider coming into contact with the box
            for(int i=0; i < hitColliders.Length;i++)
            {
                Collider collider = hitColliders[i];
                GameObject target = collider.gameObject;

                CollisionEffect(target);
            }
    }


        private void CollisionEffect(GameObject target)
        {
          // Can kill ennemy if bullets are moving
          if (target.CompareTag("Enemy") && ruleHandler.CanXMove("Shoot"))
          {
            Debug.Log("Collision with enemy !");
              BodyPartScript bp = target.GetComponent<BodyPartScript>();

              //if (!bp.enemy.dead)
                  Instantiate(SuperHotScript.instance.hitParticlePrefab, transform.position, transform.rotation);

              bp.HidePartAndReplace();
              bp.enemy.Kill();
          }

          // if(gameObject.CompareTag("PlayerBullet"))
          // {
          //   if(ruleHandler.CheckEffectAndAssert("Shoot is You"))
          //   {
          //     //Teleport player where bullet collides
          //     GameObject player = (GameObject)GameObject.Find("Player");
          //
          //     Vector3 deltaPosition = gameObject.transform.position - player.transform.position;
          //
          //     //Only 90% of distance in order not to go though the ground and most walls
          //     player.transform.position += deltaPosition*0.9f;
          //   }
          //
          // }

          //bullets don't destroy other bullets
          if(!target.CompareTag("PlayerBullet") && !target.CompareTag("EnnemyBullet"))
          {
            DestroyMyself();
          }



        }

        //TODO: Add event "Shoot is Dead" and find a way to activate it only once
        private void DestroyMyself()
        {
          Destroy(gameObject);
        }
}
