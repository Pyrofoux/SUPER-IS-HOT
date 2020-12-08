using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleHandler : MonoBehaviour
{

    public Dictionary<string,bool> asserts;
    public Dictionary<string,Dictionary<string,bool>> implies;
    public Dictionary<string,bool> triggers;
    public Dictionary<string,bool> effects;
    private BabaWorld babaWorld;

    // Start is called before the first frame update
    void Start()
    {

      babaWorld = GetComponent<BabaWorld>();

       //3 different concepts
       //Rule checks (checks if player is moving)
       //Rule effects (makes player moving)
       //Rule effects are deduced from Rule checks values  + the rules defined in the Baba world
       // checks => effects

       //+ assertions
       //when a statement doesn't have "WHEN" before

       asserts = new Dictionary<string, bool>();
       implies = new Dictionary<string,Dictionary<string,bool>>();

       triggers = new Dictionary<string, bool>();
       // triggers["You is Move"] = false;
       // triggers["Super is Hot"] = false;
       // triggers["Time is Move"] = false;


       effects = new Dictionary<string, bool>();
       // effects["Time is Move"] = false;
       // effects["You is Move"] = true;
       // effects["Shoot is You"] = false;

       //Parse();
    }

    // Update is called once per frame
    void Update()
    {


      //Triggers checks

          //Time is always flowing, unless it is stopped
          //getting existentialist vibes rn
          triggers["Time is Move"] = !CheckEffect("Time is Stop") && !babaWorld.isBabaMode();


          //Player movement
          triggers["You is Move"] = false;
          if(!babaWorld.isBabaMode()) // Check game unpaused
          {
            if(!CheckEffect("You is Stop")) //Check movement is controlled + currently moving
            {
              float x = Input.GetAxisRaw("Horizontal");
              float y = Input.GetAxisRaw("Vertical");
              triggers["You is Move"] = (x != 0 || y != 0);
            }

          }

          //triggers["You is Stop"] = !triggers["You is Move"];

          //Bullet shoot
          GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
          triggers["You is Shoot"] = playerBullets.Length > 0;
          Debug.Log(triggers["You is Shoot"]);

          //Should detect when the teleportion occurs
          //triggers["Shoot is You"];



      //Rule effects <-- deduced from rules in Baba
      //notes: rules order have importance, graph of dependency
      //a rule being conditional or not has an importance
      //ex: player is move VS player is move when time is move
      // ---> difference between asserts and implies

      //effects["Player is Move"] = triggers["You is Shoot"];
      //effects["Time is Move"] = false;
      //effects["You is Shoot"] = true;
      //effects["You is Move"] = triggers["You is Shoot"] ;
      //effects["Shoot is You"] = true;

      //asserts["Time is Move"] = true;

      //Erase all current effects
      effects = new Dictionary<string,bool>();


      // Effect takes value based on trigger
      foreach(KeyValuePair<string, Dictionary<string,bool>> implie in implies)
      {
          string trigger = implie.Key;
          Dictionary<string,bool> effectList = implie.Value;
          foreach(KeyValuePair<string, bool> effect in effectList)
          {

            if(effects.ContainsKey(effect.Key))
            {
              // If effect is already activated by another trigger, don't overwrite it
              effects[effect.Key] = effects[effect.Key] || CheckEvent(trigger);
            }
            else
            {
                effects[effect.Key] = CheckEvent(trigger);
            }
          }
      }

    }

    public void UpdateRules(Dictionary<string,bool> _asserts, Dictionary<string,Dictionary<string,bool>> _implies)
    {
      asserts = _asserts;
      implies = _implies;

      bool logRules = true;


      if(logRules)
      {

        //Asserts
        foreach(KeyValuePair<string, bool> assert in asserts)
        {
          Debug.Log(assert.Key);
        }

        // Implications
        foreach(KeyValuePair<string, Dictionary<string,bool>> implie in implies)
        {
          string trigger = implie.Key;
          Dictionary<string,bool> effects = implie.Value;
          foreach(KeyValuePair<string, bool> effect in effects)
          {
            Debug.Log(trigger+" > "+effect.Key);
          }
        }
      }


    }

    public bool CheckEffect(string rule)
    {
      if(asserts.ContainsKey(rule) && asserts[rule])
      {
        if(asserts[rule]) return true;
      }

      if(effects.ContainsKey(rule) && effects[rule])
      {
        return effects[rule];
      }
      return false;
    }

    public bool CheckEvent(string rule)
    {
      if(asserts.ContainsKey(rule) && asserts[rule])
      {
        if(asserts[rule]) return true;
      }

      if(triggers.ContainsKey(rule) && triggers[rule])
      {
        return triggers[rule];
      }
      return false;
    }

    public bool CheckAssert(string rule)
    {
      if(asserts.ContainsKey(rule) && asserts[rule])
      {
        return asserts[rule];
      }

      return false;
    }

}
