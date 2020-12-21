using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  public List<string> levelsLayout = new List<string>();
  public int currentLevel = 0;
  public int maxLevels;


  void Awake()
      {

          DontDestroyOnLoad(this.gameObject);
          LoadLevels();

          maxLevels = levelsLayout.Count;
      }

      public bool IsTitleScreen()
      {
        return currentLevel == 0;
      }


      public void LoadLevels()
      {
        levelsLayout[0] =@"
        _____________
        ___T=M<Y=M___
        _____@___$_=H
        __________T=S
        Y=M_Y=s______
        ";

        levelsLayout[1] =@"
        _____________
        _____________
        _____@___$_=H
        __________T=S
        Y=M_Y=s______
        ";

        levelsLayout[2] =@"
        _____________
        _____T=M<Y=M_
        _________$_=H
        __________T=S
        Y=M_Y=s___@__
        ";
      }


}
