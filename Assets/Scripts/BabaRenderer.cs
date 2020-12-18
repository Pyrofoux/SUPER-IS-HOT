using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class BabaRenderer : MonoBehaviour
{

  public Image tilePrefab;

  [Header("Sprites")]
  public Sprite BabaUpSprite;
  public Sprite BabaDownSprite;
  public Sprite BabaLeftSprite;
  public Sprite BabaRightSprite;
  public Sprite IsSprite;
  public Sprite WhenSprite;
  public Sprite TimeSprite;
  public Sprite YouSprite;
  public Sprite ShootSprite;
  public Sprite MoveSprite;
  public Sprite StopSprite;
  public Sprite DeadSprite;
  public Sprite SuperSprite;
  public Sprite HotSprite;
  public Sprite WallSprite;
  public Sprite EmptySprite;
  public Sprite Lock1Sprite;
  public Sprite Lock2Sprite;
  public Sprite Lock3Sprite;
  public Sprite Lock4Sprite;
  public Sprite Lock5Sprite;
  public Sprite Lock6Sprite;





  [Header("DO NOT TOUCH")]


  //Private
  private GameObject hud;
  private Vector3 hudCenter;
  private BabaWorld babaWorld;

  //Display Settings
  float startpointX = 0;
  float startpointY = 0;
  float horizontalSpacing = 80;
  float verticalSpacing = 50;

  float tileWidth = 60;
  float tileHeight = 60;


    // Start is called before the first frame update
    void Start()
    {

      //get canvas HUD
      hud = (GameObject) GameObject.Find("HUD_Baba");
      hudCenter = hud.transform.position;
      babaWorld = GetComponent<BabaWorld>();
    }

    // Update is called once per frame
    void Update()
    {

    }


  public void UpdateDisplay()
  {
    Clear();

    startpointX = -babaWorld.width *tileWidth/2;
    startpointY = Screen.height/4;
    horizontalSpacing = tileWidth;
    verticalSpacing = tileHeight;

     for(int y = 0; y < babaWorld.height ; y++)
     {
       for(int x = 0; x < babaWorld.width ; x++)
       {

         Image tileUI = (Image)Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
         string name = babaWorld.getTile(x,y).name;
         name = name[0].ToString().ToUpper()+name.Substring(1).ToLower();

         if(x == babaWorld.baba.x && babaWorld.baba.y == y)
         {
           if(babaWorld.lastMove == Vector2.down){tileUI.sprite = BabaUpSprite;}else
           if(babaWorld.lastMove == Vector2.up){tileUI.sprite = BabaDownSprite;}else
           if(babaWorld.lastMove == Vector2.left){tileUI.sprite = BabaLeftSprite;}else
           if(babaWorld.lastMove == Vector2.right){tileUI.sprite = BabaRightSprite;}
         }
         else
         {

           if(name == "Time"){tileUI.sprite = TimeSprite;}else
           if(name == "You"){tileUI.sprite = YouSprite;}else
           if(name == "Shoot"){tileUI.sprite = ShootSprite;}else
           if(name == "Move"){tileUI.sprite = MoveSprite;}else
           if(name == "Stop"){tileUI.sprite = StopSprite;}else
           if(name == "Dead"){tileUI.sprite = DeadSprite;}else
           if(name == "Is"){tileUI.sprite = IsSprite;}else
           if(name == "When"){tileUI.sprite = WhenSprite;}else
           if(name == "Super"){tileUI.sprite = SuperSprite;}else
           if(name == "Hot"){tileUI.sprite = HotSprite;}else
           if(name == "#"){tileUI.sprite = WallSprite;}else
           if(name == "[1]"){tileUI.sprite = Lock1Sprite;}else
           if(name == "[2]"){tileUI.sprite = Lock2Sprite;}else
           if(name == "[3]"){tileUI.sprite = Lock3Sprite;}else
           if(name == "[4]"){tileUI.sprite = Lock4Sprite;}else
           if(name == "[5]"){tileUI.sprite = Lock5Sprite;}else
           if(name == "[6]"){tileUI.sprite = Lock6Sprite;}else
           {
             tileUI.sprite = EmptySprite;
           }


         }

         // Set active or disactivated shade
         BabaTileDoodleFix tileScript = tileUI.GetComponent<BabaTileDoodleFix>();

         // Warning: if Baba's corresponding tile happens to be in the list of words
         // it will be considered in activated words check
         if(!babaWorld.isValidWord(name) || babaWorld.activatedWords.Contains(new Vector2Int(x,y)))
         {
           tileScript.activated = true;
         }
         else
         {
           tileScript.activated = false;
         }



         tileUI.transform.SetParent(hud.transform);
         //Positionning
         tileUI.transform.position = hudCenter + new Vector3(startpointX+x*horizontalSpacing,startpointY+y*-verticalSpacing,0);
       }
     }

  }

  public void Clear()
  {
    //Clear all UI elements
     GameObject[] uis = GameObject.FindGameObjectsWithTag("BabaUI");
     foreach(GameObject ui in uis)
     {
       GameObject.Destroy(ui);
     }
  }

}
