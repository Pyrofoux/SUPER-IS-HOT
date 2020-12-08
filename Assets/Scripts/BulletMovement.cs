using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BodyPartScript bp = collision.gameObject.GetComponent<BodyPartScript>();

            //if (!bp.enemy.dead)
                Instantiate(SuperHotScript.instance.hitParticlePrefab, transform.position, transform.rotation);

            bp.HidePartAndReplace();
            bp.enemy.Ragdoll();
        }

        if(gameObject.CompareTag("PlayerBullet"))
        {
          RuleHandler ruleHandler = (RuleHandler)GameObject.FindObjectOfType(typeof(RuleHandler));
          if(ruleHandler.CheckEffect("Shoot is You"))
          {
            //Teleport player where bullet collides
            GameObject player = (GameObject)GameObject.Find("Player");

            Vector3 deltaPosition = gameObject.transform.position - player.transform.position;

            //Only 90% of distance in order not to go though the gound and most walls
            player.transform.position += deltaPosition*0.9f;
          }

        }


        Destroy(gameObject);
    }
}
