using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class BabaRenderer : MonoBehaviour
{

  [Header("Prefabs")]
  public Image tilePrefab;
  public Image ControlsLeft;
  public Image ControlsRight;
  public Image StartKeys;
  public Image StartE;
  public Image Author;
  public Image Title;
  public GameObject hud;

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
  public Sprite CornerSprite;
  public Sprite LineSprite;




  //Private
  private Vector3 hudCenter;
  private BabaWorld babaWorld;

  //Display Settings

  float tileWidth;
  float tileHeight;

  float startpointX;
  float startpointY;
  float horizontalSpacing;
  float verticalSpacing;

  float startpointXTitleScreen;
  float startpointYTitleScreen;

  float maxHTiles = 13;
  float maxVtiles = 9;
  float sidePanelTileSize = 3;

  Image[,] tileSprites;

  private bool updatedResolution = false;
  private bool createdSprites = false;


    // Start is called before the first frame update
    void Start()
    {

      //get canvas HUD
      hudCenter = hud.transform.position;
      babaWorld = GetComponent<BabaWorld>();

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateResolution()
    {
      if(updatedResolution)return ;

      updatedResolution = true;

      //Smallest taken values
      //float neededHorizontalSize = maxHTiles+sidePanelTileSize*2+2+2+1;

      float neededHorizontalSize = babaWorld.width+sidePanelTileSize*2+2+2+1;
      float neededVerticalSize = babaWorld.height+3;

      // TODO: check tiles resize or fixed size ? 10px
      float maxtileWidth = Screen.width/neededHorizontalSize;
      float maxtileHeight = Screen.height/neededVerticalSize;

      // Taking the smallest displayable tile size, fitted to vertical+horizontal space
      tileWidth = tileHeight = Mathf.Min(maxtileWidth, maxtileHeight);

      // Update tile prefab dimensions
      tilePrefab.rectTransform.sizeDelta = new Vector2(tileWidth, tileHeight);

      // Update control panels dimensions, relative to tiles
      ControlsLeft.rectTransform.sizeDelta = new Vector2(tileWidth*sidePanelTileSize, tileHeight*sidePanelTileSize*2);
      ControlsRight.rectTransform.sizeDelta = new Vector2(tileWidth*sidePanelTileSize, tileHeight*sidePanelTileSize*2);


      //Update there because of script order
      startpointX = -(babaWorld.width*tileWidth)/2+tileWidth/2; // horizontal middle
      startpointY = Screen.height/4+tileWidth; // 1/4 of the screen
      horizontalSpacing = tileWidth;
      verticalSpacing = tileHeight;

      startpointXTitleScreen = startpointX;
      startpointYTitleScreen = 0;
    }

  public void UpdateDisplay()
  {


    UpdateResolution();
    CreateSprites();
    //Clear();

     for(int y = 0; y < babaWorld.height ; y++)
     {
       for(int x = 0; x < babaWorld.width ; x++)
       {

         Image tileUI = tileSprites[x,y];
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
       }
     }

  }


  public Vector3 TileToSpace(float x, float y)
  {
    if(babaWorld.isTitleScreen()) // Title screen coordinates are slightly lower than usual
    {
      return hudCenter + new Vector3(startpointXTitleScreen+x*horizontalSpacing,startpointYTitleScreen+y*-verticalSpacing,0);
    }
    else
    {
      return hudCenter + new Vector3(startpointX+x*horizontalSpacing,startpointY+y*-verticalSpacing,0);
    }

  }

  public Vector3 TileToSpace(int x, int y)
  {
    return TileToSpace((float) x, (float) y);
  }


  public void CreateSprites()
  {
    if(createdSprites)return;
    createdSprites = true;

    if(babaWorld.isTitleScreen())
    {

      Title.rectTransform.sizeDelta = new Vector2(tileWidth*8*1.5f, tileHeight*3*1.5f);

      Image titleScreen = (Image)Instantiate(Title, new Vector3(0, 0, 0), Quaternion.identity);
      titleScreen.transform.SetParent(hud.transform);
      titleScreen.transform.position = TileToSpace((float)(babaWorld.width-1)/2,(float)-4);

      // Control panels at startup

      Image panelLeft = (Image)Instantiate(StartKeys, new Vector3(0, 0, 0), Quaternion.identity);
      panelLeft.transform.SetParent(hud.transform);
      //Position is set to number of tiles associated to control panel in UpdateResolution
      panelLeft.transform.position = TileToSpace((float)-sidePanelTileSize-0.5f,(float) babaWorld.height/2);
      panelLeft.GetComponent<BabaTileDoodleFix>().activated = true;

      Image panelRight = (Image)Instantiate(StartE, new Vector3(0, 0, 0), Quaternion.identity);
      panelRight.transform.SetParent(hud.transform);
      //Position is set to number of tiles associated to control panel in UpdateResolution
      panelRight.transform.position = TileToSpace((float)babaWorld.width-0.5f+sidePanelTileSize,(float) babaWorld.height/2);
      panelRight.GetComponent<BabaTileDoodleFix>().activated = true;

      Image credit = (Image)Instantiate(Author, new Vector3(0, 0, 0), Quaternion.identity);
      credit.transform.SetParent(hud.transform);
      credit.transform.position = TileToSpace((float)(babaWorld.width-1)/2,(float)4);
      credit.GetComponent<BabaTileDoodleFix>().activated = true;

    }
    else
    {

      // Load Controls panels
      Image panelLeft = (Image)Instantiate(ControlsLeft, new Vector3(0, 0, 0), Quaternion.identity);
      panelLeft.transform.SetParent(hud.transform);
      //Position is set to number of tiles associated to control panel in UpdateResolution
      panelLeft.transform.position = TileToSpace((float)-sidePanelTileSize-0.5f,(float) babaWorld.height/2);
      panelLeft.GetComponent<BabaTileDoodleFix>().activated = true;



      Image panelRight = (Image)Instantiate(ControlsRight, new Vector3(0, 0, 0), Quaternion.identity);
      panelRight.transform.SetParent(hud.transform);
      //Position is set to number of tiles associated to control panel in UpdateResolution
      panelRight.transform.position = TileToSpace((float)babaWorld.width-0.5f+sidePanelTileSize,(float) babaWorld.height/2);
      panelRight.GetComponent<BabaTileDoodleFix>().activated = true;
    }
    //Load center tiles

    tileSprites = new Image[babaWorld.width,babaWorld.height];

    for(int y = 0; y < babaWorld.height ; y++)
    {
      for(int x = 0; x < babaWorld.width ; x++)
      {
        Image tileUI = (Image)Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        tileUI.transform.SetParent(hud.transform);
        //Positionning
        tileUI.transform.position = TileToSpace(x,y);
        tileSprites[x,y] = tileUI;
        tileUI.sprite = EmptySprite;
      }
    }

    //Outline

    //vertical Lines
    for(int y = 0; y < babaWorld.height ; y++)
    {
      //Line 1
      Image tileUI1 = (Image)Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
      tileUI1.transform.SetParent(hud.transform);
      tileUI1.transform.position = TileToSpace(-1,y);
      tileUI1.sprite = LineSprite;
      tileUI1.GetComponent<BabaTileDoodleFix>().activated = true;

      //Line 2
      Image tileUI2 = (Image)Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
      tileUI2.transform.SetParent(hud.transform);
      tileUI2.transform.position = TileToSpace(babaWorld.width,y);
      tileUI2.sprite = LineSprite;
      tileUI2.GetComponent<BabaTileDoodleFix>().activated = true;
    }

    //Horizontal
    for(int x = 0; x < babaWorld.width ; x++)
    {
      //Line 1
      Image tileUI1 = (Image)Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
      tileUI1.transform.SetParent(hud.transform);
      tileUI1.transform.position = TileToSpace(x,-1);
      tileUI1.sprite = LineSprite;
      tileUI1.GetComponent<BabaTileDoodleFix>().activated = true;
      tileUI1.transform.Rotate(Vector3.forward * -90);

      //Line 2
      Image tileUI2 = (Image)Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
      tileUI2.transform.SetParent(hud.transform);
      tileUI2.transform.position = TileToSpace(x,babaWorld.height);
      tileUI2.sprite = LineSprite;
      tileUI2.GetComponent<BabaTileDoodleFix>().activated = true;
      tileUI2.transform.Rotate(Vector3.forward * -90);
    };

    // Corners
    for(int i=0; i < 4; i++)
    {
      Image tileUI1 = (Image)Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
      tileUI1.transform.SetParent(hud.transform);

      int x; int y;

      if(i%2 == 0)
      {
        x = -1;
      }
      else
      {
        x = babaWorld.width;
      }

      if(i%3 == 0)
      {
        y = -1;
      }
      else
      {
        y = babaWorld.height;
      }

      int[] rotations = new int[]{0,-180,90,-90};

      tileUI1.transform.position = TileToSpace(x, y);
      tileUI1.sprite = CornerSprite;
      tileUI1.GetComponent<BabaTileDoodleFix>().activated = true;
      tileUI1.transform.Rotate(Vector3.forward * rotations[i]);
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

  public void OpenDisplay()
  {
    hud.SetActive(true);
    UpdateDisplay(); // specific order or can't erase them
  }

  public void CloseDisplay()
  {
    hud.SetActive(false);
  }

}
