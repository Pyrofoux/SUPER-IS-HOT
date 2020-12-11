using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleHandler : MonoBehaviour
{

    public Dictionary<string,bool> asserts;
    public Dictionary<string,Dictionary<string,bool>> implies;
    public Dictionary<string,bool> triggers;
    public Dictionary<string,bool> effects;
    public Dictionary<string,bool> dead;
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

       // Stores things that can be killed only once
       dead = new Dictionary<string, bool>();
       dead["Time"] = false;
       dead["You"] = false;
       //dead["Shoot"] = false;

    }

    // Update is called once per frame
    void Update()
    {


      // Once You or Time is dead, its always dead
      if(!dead["Time"]) dead["Time"] = CheckEffectAndAssert("Time is Dead");
      if(!dead["You"]) dead["You"] = CheckEffectAndAssert("You is Dead");




      //Triggers checks

      triggers["Time is Dead"] = dead["Time"];
      triggers["You is Dead"] = dead["You"];

        //Time is always flowing, unless it is stopped
        //and it can only be stopped by killing it
        //getting existentialist vibes rn
        triggers["Time is Stop"] = !CanXMove("Time");
        triggers["Time is Move"] = CanXMove("Time");
        // && !babaWorld.isBabaMode();

        //Player movement
        triggers["You is Move"] = false;
        if(!babaWorld.isBabaMode()) // Check game unpaused
        {
          if(!CheckEffectAndAssert("You is Stop")) //Check movement is controlled + currently moving
          {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            triggers["You is Move"] = (x != 0 || y != 0);
          }
        }

        triggers["You is Stop"] = !triggers["You is Move"];

        // OLDER Bullet shoot
        // GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        // triggers["You is Shoot"] = playerBullets.Length > 0;
        // //Debug.Log(triggers["You is Shoot"]);

        //Bullet shoot
        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        if(playerBullets.Length > 0 && CanXMove("Shoot"))
        {
          triggers["Shoot is Move"] = true;
        }
        else
        {
          triggers["Shoot is Move"] = false;
        }

        //trigger for Shoot is stop when it is destroyed

        //Should detect when the teleportion occurs
        //effect and assert + there is at least one bullet
        //triggers["Shoot is You"];



      //Rule effects <-- deduced from rules in Baba
      //notes: rules order have importance, graph of dependency
      //a rule being conditional or not has an importance
      //ex: player is move VS player is move when time is move
      // ---> difference between assertions (fact) and implications (trigger => effect)

      //Erase all current effects
      effects = new Dictionary<string,bool>();


      // Apply dead effects -- implicit trigger
      if(CheckEvent("Time is Dead")) effects["Time is Stop"] = true;
      if(CheckEvent("You is Dead")) effects["You is Stop"] = true;
      if(CheckEvent("Shoot is Dead")) effects["Shoot is Stop"] = true;

      if(!CheckEffectAndAssert("Time is Stop")) effects["Time is Move"] = true;


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

    public bool CheckEffectAndAssert(string rule)
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

    public bool CheckEffect(string rule)
    {
      if(effects.ContainsKey(rule) && effects[rule])
      {
        return effects[rule];
      }
      return false;
    }

    public bool CheckEventAndAssert(string rule)
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

    public bool CheckEvent(string rule)
    {

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

    public bool CanXMove(string X)
    {
      // X can move ?
      // Time is Move and not X is stop
      //  or
      // Time is Stop and X is Move

      // Time is stop and X is stop => FALSE
      // Time is move and X is move => true

      //Default behaviour is to follow Time unless its explicitel stated otherwise
      //Plus, effects have priority other asserts

      if(CheckEffect(X+" is Stop")) return false;
      if(CheckEffect(X+" is Move")) return true;
      if(CheckAssert(X+" is Stop")) return false;
      if(CheckAssert(X+" is Move")) return true;

      if(dead["Time"]) return false;
      if(CheckEffect("Time is Stop")) return false;
      if(CheckEffect("Time is Move")) return true;
      if(CheckAssert("Time is Stop")) return false;
      if(CheckAssert("Time is Move")) return true;

      Debug.Log("Move check fails");
      return false;

    }



}
