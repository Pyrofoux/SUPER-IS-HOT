using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  public List<string> levelsLayout = new List<string>();
  public int currentLevel = 0;
  public int maxLevels;
  LevelManager instance;


  void Awake()
      {
        //Make sure there's only one by scene
        if(instance == null)
        {
             instance = this;
             DontDestroyOnLoad(gameObject);
        }else if(instance != this)
        {
             Destroy(gameObject);
        }


          DontDestroyOnLoad(this.gameObject);
          LoadLevels();

          maxLevels = levelsLayout.Count;
          //Debug.Log(levelsLayout.Count);
      }

      public bool isTitleScreen()
      {
        return currentLevel == 0;
      }

      public void SwitchLevel(int id)
      {
        Debug.Log("Switching to "+id.ToString());
        if(id >= maxLevels)
        {
          id = 0;
        }
        Debug.Log("Switched to "+id.ToString());

        currentLevel = id;

      }

      public void SwitchNextLevel()
      {
        SwitchLevel(currentLevel+1);
      }


      public void LoadLevels()
      {
levelsLayout.Add(@"
_@___________
#1#2#3#4#5#6#
_____________
");

// First Steps
levelsLayout.Add(@"
__Y=s____#_____
__=______#_1___
__M__T=S____=__
_________#___2_
_____@___#_____
");

//Naked
//Time is Move When You is Move
levelsLayout.Add(@"
@____________
_____________
___T=M<_Y_=M_
___=___=_____
___S___M_____
");


levelsLayout.Add(@"
@____________
____Y=S<s=M__
____s=M<Y=S__
_____________
Y=M___s=S____
");
      }


}
