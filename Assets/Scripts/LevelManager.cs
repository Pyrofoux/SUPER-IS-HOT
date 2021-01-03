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
__Y=_s___#_____
__=____M_#_1___
__M__T=S____=__
_________#___2_
_____@___#_____
");

//Classic
//Time is Move When You is Move
levelsLayout.Add(@"
@___________
__________M_
__T=M<Y=S___
____________
_#####T=M##_
____________
1_____Y=s___
2_____=_____
3_____S_____
");


// Reversal
levelsLayout.Add(@"
___#__##__#___T__Y
@__#T=M<Y=M___=__=
___#Y###T##___S__S
__________________
____2_=_1_________
______________Y=#s
");

// Self-termination
levelsLayout.Add(@"
@___$=H<Y=D__
_____________
_____________
______##_____
______Y=M#___
_#Y=s<Y=S#___
__####T=M#___
________S#___
_________#___
_____________
");

levelsLayout.Add(@"
_@___________
#1#2#3#4#5#6#
_____________
");

levelsLayout.Add(@"
T_Y=D<s=S__s
=_T=M<s=M__=
S__________M
############
_____@______
__1__2__3___
____________
Y=s___=_____
____________
____________
");


}
}
