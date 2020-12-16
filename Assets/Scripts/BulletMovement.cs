using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    Rigidbody rb;
    RuleHandler ruleHandler;
    EffectsApplicator effectsApplicator;

    private bool destroyingMyself;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //get time handling script -- for game pause
        effectsApplicator = (EffectsApplicator)GameObject.FindObjectOfType(typeof(EffectsApplicator));
        //get ruleHandler -- to modify rules in SuperHot
        ruleHandler = (RuleHandler)GameObject.FindObjectOfType(typeof(RuleHandler));

        destroyingMyself = false;

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
         // Your bullets are affected by rule changes
            if(!effectsApplicator.babaMode && ruleHandler.CanXMove("Shoot"))
            {
              transform.position += transform.forward * speed * Time.fixedDeltaTime;
              CheckCollisionManual();
            }

            //Destroy player bullet when Shoot is Dead
            if(ruleHandler.CheckEffectAndAssert("Shoot is Dead"))
            {
              DestroyMyself();
            }
        }

    }


    // public void SetScripts(RuleHandler ruleHandler, effectsApplicator effectsApplicator)
    // {
    //   this.ruleHandler = ruleHandler;
    //   this.effectsApplicator = effectsApplicator;
    // }

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

              BodyPartScript bp = target.GetComponent<BodyPartScript>();

              //if (!bp.enemy.dead)
                  Instantiate(EffectsApplicator.instance.hitParticlePrefab, transform.position, transform.rotation);

              bp.HidePartAndReplace();
              bp.enemy.Kill();
              DestroyMyself();
          }

          //bullets don't destroy other bullets => might be source of weir trahectories
          if(target.CompareTag("PlayerBullet") &&  gameObject.CompareTag("PlayerBullet"))
          {
            //Don't destroy
            //Debug.Log("Ignored 1");
          }
          // own bullets don't diseappear because of player when Shoot is You
          else if(target.CompareTag("Player") && gameObject.CompareTag("PlayerBullet") && ruleHandler.CheckEffectAndAssert("Shoot is You"))
          {
            //Don't destroy
            //Debug.Log("Ignored 2");
          }
          // Don't make guns destroy bullets -- for your own
          else if(target.CompareTag("Gun") && gameObject.CompareTag("PlayerBullet"))
          {
            //Don't destroy
            //Debug.Log("Ignored 3");
          }
          else
          {
            DestroyMyself();
          }

          // if(gameObject.CompareTag("PlayerBullet"))
          // Debug.Log(target);

        }



        //TODO: Add event "Shoot is Dead" and find a way to activate it only once
        private void DestroyMyself()
        {
          if(gameObject.CompareTag("PlayerBullet"))
          {
            /*ruleHandler.triggers["Shoot is Dead"] = true;
            ruleHandler.CalculateRules();
            effectsApplicator.ApplyEffects();
            ruleHandler.triggers["Shoot is Dead"] = false;
            effectsApplicator.bulletList.Remove(gameObject);
            Destroy(gameObject);
            */
            ruleHandler.triggerFrame["Shoot is Dead"] = 1;
          }
          destroyingMyself = true;


        }

    private void LateUpdate()
    {
      if(destroyingMyself)
      {
        effectsApplicator.bulletList.Remove(gameObject);
        Destroy(gameObject);
      }
    }
}
