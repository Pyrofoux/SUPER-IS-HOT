using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;

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

         if(x == babaWorld.baba.x && babaWorld.baba.y == y)
         {
           if(babaWorld.lastMove == Vector2.down){tileUI.sprite = BabaUpSprite;}else
           if(babaWorld.lastMove == Vector2.up){tileUI.sprite = BabaDownSprite;}else
           if(babaWorld.lastMove == Vector2.left){tileUI.sprite = BabaLeftSprite;}else
           if(babaWorld.lastMove == Vector2.right){tileUI.sprite = BabaRightSprite;}
         }
         else
         {
           string name = babaWorld.getTile(x,y).name.ToUpper();

           if(name == "TIME"){tileUI.sprite = TimeSprite;}else
           if(name == "YOU"){tileUI.sprite = YouSprite;}else
           if(name == "SHOOT"){tileUI.sprite = ShootSprite;}else
           if(name == "MOVE"){tileUI.sprite = MoveSprite;}else
           if(name == "STOP"){tileUI.sprite = StopSprite;}else
           if(name == "DEAD"){tileUI.sprite = DeadSprite;}else
           if(name == "IS"){tileUI.sprite = IsSprite;}else
           if(name == "WHEN"){tileUI.sprite = WhenSprite;}else
           if(name == "SUPER"){tileUI.sprite = SuperSprite;}else
           if(name == "HOT"){tileUI.sprite = HotSprite;}else
           if(name == "#"){tileUI.sprite = WallSprite;}else
           {
             tileUI.sprite = EmptySprite;
           }


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
