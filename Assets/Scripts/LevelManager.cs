using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  public List<string> levelsLayout = new List<string>();
  public int currentLevel = 0;
  public int maxLevels;


  void Start()
      {

          DontDestroyOnLoad(this.gameObject);
          LoadLevels();

          maxLevels = levelsLayout.Count;
          Debug.Log(levelsLayout.Count);
      }

      public bool isTitleScreen()
      {
        return currentLevel == 0;
      }

      public void SwitchLevel(int id)
      {
        if(id >= maxLevels)
        {
          id = 0;
        }

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

levelsLayout.Add(@"
_____________
_____________
_____@___$_=H
__________T=S
Y=M_Y=s______
");

levelsLayout.Add(@"
_____________
_____T=M<Y=M_
_________$_=H
__________T=S
Y=M_Y=s___@__
");
      }


}
