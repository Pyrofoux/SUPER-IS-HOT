using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleHandler : MonoBehaviour
{

    public Dictionary<string,bool> asserts;
    public Dictionary<string,Dictionary<string,bool>> implies;
    public Dictionary<string,bool> triggers;
    public Dictionary<string,bool> effects;
    public Dictionary<string,bool> deadOnce;
    public Dictionary<string,bool> triggerOnce;
    private string[] triggerOnceEvents;
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
       triggers["Shoot is Dead"] = false;
       // triggers["You is Move"] = false;
       // triggers["Super is Hot"] = false;
       // triggers["Time is Move"] = false;


       effects = new Dictionary<string, bool>();
       // effects["Time is Move"] = false;
       // effects["You is Move"] = true;
       // effects["Shoot is You"] = false;

       // Stores things that can be killed only once
       deadOnce = new Dictionary<string, bool>();

       deadOnce["Time"] = false;
       deadOnce["You"] = false;
       //deadOnce["Shoot"] = false;

       triggerOnce = new Dictionary<string, bool>();
       triggerOnceEvents = new string[]{"Shoot is Dead", "You is Shoot"};
       PostCalculation();

    }

    // Update is called once per frame
    void Update()
    {
      //CalculateRules();
    }

    public void CalculateRules()
    {

      //Rule effects <-- deduced from rules in Baba
      //notes: rules order have importance, graph of dependency
      //a rule being conditional or not has an importance
      //ex: player is move VS player is move when time is move
      // ---> difference between assertions (fact) and implications (trigger => effect)



      // Once You or Time is dead, its always dead
      if(!deadOnce["Time"]) deadOnce["Time"] = CheckEffectAndAssert("Time is Dead");
      if(!deadOnce["You"]) deadOnce["You"] = CheckEffectAndAssert("You is Dead");


        //Triggers checks

      //Detect every trigger once and apply it
      for(int i =0; i < triggerOnceEvents.Length; i++)
      {
        triggers[triggerOnceEvents[i]] = triggerOnce[triggerOnceEvents[i]];
      }

      triggers["Time is Dead"] = deadOnce["Time"];
      triggers["You is Dead"] = deadOnce["You"];

        //Time is always flowing, unless it is stopped
        //and it can only be stopped by killing it
        //getting existentialist vibes rn
        triggers["Time is Stop"] = !CanXMove("Time");
        //|| triggers["Time is Dead"];
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
        // || triggers["You is Dead"];

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

        if(playerBullets.Length > 0 && !CanXMove("Shoot"))
        {
          triggers["Shoot is Stop"] = true;
        }
        else
        {
          triggers["Shoot is Stop"] = false;
        }

        //Additional trigger for stop when
        triggers["Shoot is Stop"] = triggers["Shoot is Stop"] || triggers["Shoot is Dead"];

      //Erase all current effects
      effects = new Dictionary<string,bool>();


      // Apply dead effects -- implicit trigger
      if(CheckEvent("Time is Dead"))
      {
        effects["Time is Stop"] = true;
        triggers["Time is Stop"] = true;
      }
      if(CheckEvent("You is Dead"))
      {
        effects["You is Stop"] = true;
        triggers["You is Stop"] = true;
      }
      if(CheckEvent("Shoot is Dead"))
      {
        effects["Shoot is Stop"] = true;
        triggers["Shoot is Stop"] = true;
      }

      // Time always moves, unless it is stopped
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

    public void PostCalculation()
    {
      // Clear TriggerOnce events

      for(int i =0; i < triggerOnceEvents.Length; i++)
      {
        triggerOnce[triggerOnceEvents[i]] = false;
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

      if(deadOnce["Time"]) return false;
      if(CheckEffect("Time is Stop")) return false;
      if(CheckEffect("Time is Move")) return true;
      if(CheckAssert("Time is Stop")) return false;
      if(CheckAssert("Time is Move")) return true;

      Debug.Log("Move check fails");
      return false;

    }



}
