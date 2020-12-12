using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BabaWorld : MonoBehaviour
{

/*

@____________
_____________
_____________
_____________
_____________

#Test conditionals
@____________
____Y=M<T=S__
____Y=S<T=M__
____T=M<s=M__
_______T=S___
_______s=M___

Good solution for $=H<Y=D
@____________
____Y=S<s=M__
____s=M<Y=S__
_____________
Y=M___s=S____


Test: teleport when hit
@____________
____s=Y<s=S___
_____________
_____________
Y=M___s=M____

Test: you are carried by bullets
@____________
____s=Y______
_____________
_____________
Y=M___s=M____

*/

[TextArea(8,12)]
private string layout =@"
@____________
____s=Y<s=S___
_____________
_____________
Y=M___s=M____
";

  public Text textPrefab;


  //Display Settings
  private string babaDisplayString = ":D";
  float startpointX = -500;
  float startpointY = 0;
  float horizontalSpacing = 80;
  float verticalSpacing = 50;

  //Config
  //The interval you want your player to be able to fire input.
  static float inputRate = 0.03f;
  static float inputRateStop = inputRate*0.0001f;
  static string[] wordList = new string[]{"Time","Move","You","Super","Hot","Shoot","Stop","Dead"};

  private int width =0;
  private int height =0;
  private Tile[,] map;
  private Vector2Int baba = new Vector2Int(0,0);
  private int currentUnlockId = 1;

  // HUD
  private Vector3 hudCenter;

  // GameObjects
  private SuperHotScript superhotScript;
  private RuleHandler ruleHandler;
  private GameObject hud;

    // Start is called before the first frame update
    void Start()
    {

      //get canvas HUD
      hud = (GameObject) GameObject.Find("HUD_Baba");
      hudCenter = hud.transform.position;

      //get time handling script -- for game pause
      superhotScript = GetComponent<SuperHotScript>();

      //get ruleHandler -- to modify rules in SuperHot
       ruleHandler = GetComponent<RuleHandler>();

      //Convert ASCII to tiles
      char[] n = {'\n'};
      string[] templines = layout.Split(n); // remove last item here
      string[] lines = new string[templines.Length-2];

       // Don't count first and last line
      Array.Copy(templines, 1, lines, 0, templines.Length-2);

      height = lines.Length;
      width = lines[0].Length;
      map = new Tile[width,height];

      for(int y = 0; y < height; y++)
      {
        string line = lines[y];
        for(int x = 0; x < width; x++)
        {
            char ascii = lines[y][x];
            map[x,y] = Tile.convert(ascii);

            if(map[x,y].spawn)
            {
              baba.x = x;
              baba.y = y;
            }

        }
      }
      ParseRules();
      UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {

      if(superhotScript.babaMode)
      {
        Vector2Int move = checkInput();
        bool needReload = false;

        //Apply move input
        if(move.sqrMagnitude > 0)//Detect move request
        {
          needReload = ApplyMove(move);
        }

        //Update ruls and display accordingly
        if(needReload)
        {
          ParseRules();
          UpdateDisplay();
        }
      }
    }


    private Tile getTile(int x, int y)
    {
      if (x<0 ||  y<0 || x>=width || y >= height)
      {
        return new Wall();
      }

      return map[x,y];

    }

    private Tile getTile(Vector2Int vect)
    {
      return getTile(vect.x, vect.y);
    }

    private void setTile(int x, int y, Tile tile)
    {
      if (x<0 ||  y<0 || x>=width || y >= height)
      {

      }
      else
      {
        map[x,y] = tile;
      }

    }

    private void setTile(Vector2Int vect, Tile tile)
    {
      setTile(vect.x, vect.y, tile);
    }

    private bool ApplyMove(Vector2Int move)
    {

      //Check if baba can move and push tiles
      //Save pushChain of tiles until first empty
      bool validPushChain = true;
      bool staticMet = false;
      Vector2Int current = baba;
      List<Tile> pushChain = new List<Tile>();

      int count = 0;
      while(!staticMet && count < 25)
      {
        count++;
        current += move;
        Tile nextTile = getTile(current);
        pushChain.Add(nextTile);

        staticMet = nextTile.statik;

        //Is it still a pushable chain ?
        if(staticMet == true && nextTile.empty == false)
        {
          validPushChain = false;
        }
      }

      if(validPushChain)
      {
        //Replace each tile with the the tile before it in the chain
        Vector2Int _current = baba+move;
        for(int i = 0; i < pushChain.Count-1; i++)
        {
          _current += move;
          setTile(_current,pushChain[i]);
        }
        //Set first tile in chain to empty
        Vector2Int front = baba+move;
        setTile(front,new Empty());
        baba = front;
        return true;
      }
      else
      {
        return false;
      }

    }

  private void ParseRules()
  { // Parse rules logic from tiles on map

    Dictionary<string,bool> asserts = new Dictionary<string,bool>();
    Dictionary<string,Dictionary<string,bool>> implies = new Dictionary<string,Dictionary<string,bool>>();

    //Read the board, by lines of 7 tiles
    //Horizontal + Vertical


    //Set of "IS" words that are in a valid "WHEN" statement
    //and should not be used to make an assert
    List<Vector2Int> blacklist = new List<Vector2Int>();

    for(int y = 0; y < height; y++)
    {
      for(int x = 0; x < width; x++)
      {

        Vector2Int[] hline = new Vector2Int[7];
        Vector2Int[] vline = new Vector2Int[7];

        Vector2Int[] bottom_hline = new Vector2Int[4];
        Vector2Int[] right_vline = new Vector2Int[4];

        //Extracting horizontal lines and verticalx7
        //o******
        //* *
        //* *
        //***
        //*
        //*
        //*

        for(int i = 0; i < 7; i++)
        {
          hline[i] = new Vector2Int(x+i,y);
          vline[i] = new Vector2Int(x,y+i);
        }

        for(int i = 0; i < 4; i++)
        {
          bottom_hline[i] = new Vector2Int(x+i,y+3);
          right_vline[i] = new Vector2Int(x+3,y+i);
        }

        Vector2Int[] corner_hv = new Vector2Int[]{hline[0], hline[1], hline[2], right_vline[0], right_vline[1], right_vline[2], right_vline[3]};
        Vector2Int[] corner_vh = new Vector2Int[]{vline[0], vline[1], vline[2], vline[3], bottom_hline[1], bottom_hline[2], bottom_hline[3]};


        List<Vector2Int[]> lines = new List<Vector2Int[]>();
        lines.Add(hline);
        lines.Add(vline);
        lines.Add(corner_hv);
        lines.Add(corner_vh);

        for(int l = 0; l < lines.Count; l++)
        {
          ParseLine(lines[l], asserts, implies, blacklist);
        }
      }
    }

    ruleHandler.UpdateRules(asserts, implies);

  }

  private void ParseLine(Vector2Int[] line, Dictionary<string,bool> asserts, Dictionary<string,Dictionary<string,bool>> implies, List<Vector2Int> blacklist)
  {

    // Words on the "line"
    string[] text = new string[7];
    for(int i = 0; i < 7; i++)
    {
      text[i] = getTile(line[i]).txt;
    }


    if(text[1] == "is" && text[3] == "when" && text[5] == "is") //Implication detection
    {
      if(wordList.Contains(text[0]) && wordList.Contains(text[2]) && wordList.Contains(text[4]) && wordList.Contains(text[6]))
      {
        string effect = text[0]+" is "+text[2];
        string trigger = text[4]+" is "+text[6];

        // Add "is" to blacklist
        blacklist.Add(line[1]);
        blacklist.Add(line[5]);

        if(!implies.ContainsKey(trigger))
        {
          implies[trigger] = new Dictionary<string,bool>();
        }
        implies[trigger][effect] = true;
      }
    }

    //If issue in order: check all whens then all is in another loop
    if(blacklist.Contains(line[1]) == false && text[1] == "is") // Assert detection
    {
      if(wordList.Contains(text[0]) && wordList.Contains(text[2]))
      {
        string assert = text[0]+" is "+text[2];
        asserts[assert] = true;
      }
    }

  }

// Interact


  // Unlock variable tiles and replace them by a word
  public void UnlockNext(string unlockedWord)
  {
    bool didReplace = false;
    for(int y = 0; y < height; y++)
    {
      for(int x = 0; x < width; x++)
      {
        Tile tile = getTile(x,y);
        if(tile.lockedId == currentUnlockId)
        {
          Debug.Log("Unlocked lock "+tile.lockedId.ToString()+ " for "+currentUnlockId.ToString());
          Tile replacement = new Word(unlockedWord);
          setTile(x,y, replacement);
          didReplace = true;
        }
      }
    }

    if(didReplace)
    {
      ParseRules();
      //UpdateDisplay();
      currentUnlockId++;
    }
  }


   //The actual time the player will be able to fire input.
    private float nextInput;
    private Vector2Int checkInput()
    {
      //Delay detection
      float horiz = Input.GetAxisRaw("Horizontal");
      float verti = -Input.GetAxisRaw("Vertical");
      bool inputDown = (horiz != 0 || verti != 0);
      if(inputDown && Time.time > nextInput)
      {



        // When Time is slowed, so is the inputRate because of time speed mess
        if(ruleHandler.CheckEffectAndAssert("Time is Move"))
        {
          nextInput = Time.time + inputRate;
        }
        else
        {
          nextInput = Time.time + inputRateStop;
        }

        if(horiz > 0)
        {
          return Vector2Int.right;
        }
        else if(horiz < 0)
        {
          return Vector2Int.left;
        }
        else if(verti > 0)
        {
          return Vector2Int.up;
        }
        else if(verti < 0)
        {
          return Vector2Int.down;
        }

        return Vector2Int.zero;
      }
      //allow to re-move if all is released
      else if(horiz == 0 && verti == 0)
      {
        nextInput = Time.time;
      }


      return Vector2Int.zero;
    }


//UI
    public void UpdateDisplay()
    {
      Clear();

       for(int y = 0; y < height; y++)
       {
         for(int x = 0; x < width; x++)
         {
           Text charUI = (Text)Instantiate(textPrefab, new Vector3(0, 0, 0), Quaternion.identity);

           if(x == baba.x && y == baba.y)
           {
             charUI.text = babaDisplayString;
           }
           else
           {
             charUI.text = getTile(x,y).name;
           }


           charUI.transform.SetParent(hud.transform);
           //Positionning
           charUI.transform.position = hudCenter + new Vector3(startpointX+x*horizontalSpacing,startpointY+y*-verticalSpacing,0);
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

    public bool isBabaMode()
    {
      return superhotScript.babaMode;
    }

}




// Tiles
public class Tile
{

  public string name;
  public bool spawn = false;
  public bool empty;
  public bool statik;
  public string txt = "";
  public int lockedId = -1;

  public Tile(string name, bool empty, bool statik)
  {

    this.name = name;
    this.empty = empty;
    this.statik = statik;

  }


  public static Tile convert(char ascii)
  {
    if(ascii == '#') return new Wall();
    if(ascii == '_') return new Empty();
    if(ascii == 'o') return new Box();
    if(ascii == '@') return new Spawn();
    if(ascii == '=') return new Word("is");
    if(ascii == '<') return new Word("when");
    if(ascii == 'T') return new Word("Time");
    if(ascii == 'M') return new Word("Move");
    if(ascii == 'Y') return new Word("You");
    if(ascii == 'S') return new Word("Stop");
    if(ascii == 's') return new Word("Shoot");
    if(ascii == '$') return new Word("Super");
    if(ascii == 'H') return new Word("Hot");
    if(ascii == 'D') return new Word("Dead");
    if(char.IsDigit(ascii)) return new Locked((int)char.GetNumericValue(ascii));

    // TODO: rechange to "*"
    return new Tile("*",true,true);
  }

}


public class Spawn : Tile
{
  public Spawn():base("_",true,true)
  {
    this.spawn = true;
  }
}

public class Empty : Tile
{
  public Empty():base("_",true,true){}
}

public class Wall : Tile
{
  public Wall():base("#",false,true){}
}

public class Box : Tile
{
  public Box():base("o",false,false){}
}

// Text word
public class Word : Box
{
  public Word(string txt):base()
  {
    // this.name = ascii.ToString();
    this.name = txt.ToUpper();
    this.txt = txt;
  }
}

// Locked variable
public class Locked : Box
{
  public Locked(int id):base()
  {
    // this.name = ascii.ToString();
    this.name = "["+id.ToString()+"]";
    this.txt = "["+id.ToString()+"]";
    this.lockedId = id;
  }
}
