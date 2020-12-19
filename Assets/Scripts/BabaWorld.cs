using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

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

To test: Flipper style
@____________
____Y=s<s=Y__
____s=Y<s=D__
_____________
Y=M___s=M_T=M

idea: clusterlevel with lots of text and very few space


mitraillette
@____________
___Y=s<Y=s____
_____________
_____________
Y=MY=ss=M_T=M
";

just everything
_____Y=s_____
T= D__________
_____T=M<Y=M_
_@____$=H__#_
Y=M____S=T__#


*/

[Header("DO NOT TOUCH")]

[TextArea(8,12)]
private string layout =@"
__2__Y=s___1_
T=2D__________
_____T=M<Y=M_
_@____$=H__#_
Y=M____S=T__#
";


  //Config
  //The interval you want your player to be able to fire input.
  static float inputRate = 1f;
  //static float inputRate = 0.03f;
  //static float inputRateStop = inputRate*0.0001f;
  private string[] validWords = new string[]{"Time","Move","You","Super","Hot","Shoot","Stop","Dead","Is","When"};
  private string[] validRules = new string[]
  {
    "You is Move","Time is Move","Shoot is Move","You is Stop","Time is Stop","Shoot is Stop","You is Dead","Time is Dead","Shoot is Dead","Super is Hot","You is Shoot","Shoot is You"
  };

  public int width =0;
  public int height =0;
  private Tile[,] map;
  public Vector2Int baba = new Vector2Int(0,0);
  private int currentUnlockId = 1;
  [System.NonSerialized]
  public Vector2Int lastMove = Vector2Int.left;
  public List<Vector2Int> activatedWords = new List<Vector2Int>();

  private List<Tile[,]> pastMaps = new List<Tile[,]>();
  private List<Vector2Int> pastMoves = new List<Vector2Int>();

  // GameObjects
  private EffectsApplicator effectsApplicator;
  private RuleHandler ruleHandler;
  private BabaRenderer renderer;


    // Start is called before the first frame update
    void Start()
    {

      //get time handling script -- for game pause
      effectsApplicator = GetComponent<EffectsApplicator>();

      //get ruleHandler -- to modify rules in SuperHot
       ruleHandler = GetComponent<RuleHandler>();

       renderer = GetComponent<BabaRenderer>();

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

      pastMaps.Add(Tile.cloneMap(map, width, height));
      pastMoves.Add(lastMove);

      ParseRules();
      renderer.UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {

      if(effectsApplicator.babaMode)
      {
        Vector2Int move = checkInput();
        bool needReload = false;

        //Apply move input
        if(move.sqrMagnitude > 0)//Detect move request
        {
          needReload = ApplyMove(move);
        }

        //Update rules, past maps and display accordingly
        if(needReload)
        {
          pastMaps.Add(Tile.cloneMap(map, width, height));
          pastMoves.Add(lastMove);


          ParseRules();
          renderer.UpdateDisplay();
        }
      }
    }


    public Tile getTile(int x, int y)
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

        lastMove = move;
        return true;
      }
      else
      {
        return false;
      }

    }

  private void ParseRules()
  { // Parse rules logic from tiles on map

    activatedWords = new List<Vector2Int>();

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


     //Trigger + Effect detection
    if(text[1] == "is" && text[3] == "when" && text[5] == "is")
    {
      if(validWords.Contains(text[0]) && validWords.Contains(text[2]) && validWords.Contains(text[4]) && validWords.Contains(text[6]))
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

        //Add to activated words
        if(isValidRule(effect))
        {
          activatedWords.Add(line[0]);
          activatedWords.Add(line[1]);
          activatedWords.Add(line[2]);
        }

        if(isValidRule(trigger))
        {
          activatedWords.Add(line[4]);
          activatedWords.Add(line[5]);
          activatedWords.Add(line[6]);
        }

        if(isValidRule(trigger) && isValidRule(effect))
        activatedWords.Add(line[3]);


      }
    }

     // Assertion detection
    //If issue in order: check all "when" then all "is" in another loop
    if(blacklist.Contains(line[1]) == false && text[1] == "is")
    {
      if(validWords.Contains(text[0]) && validWords.Contains(text[2]))
      {
        string assert = text[0]+" is "+text[2];
        asserts[assert] = true;

        //Add to activated words
        if(isValidRule(assert))
        {
          activatedWords.Add(line[0]);
          activatedWords.Add(line[1]);
          activatedWords.Add(line[2]);
        }

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
      EraseHistory();
      ParseRules();
      currentUnlockId++;
    }
  }

  public void Undo()
  {
    if(pastMaps.Count > 1)
    {
      pastMaps.RemoveAt(pastMaps.Count - 1);
      pastMoves.RemoveAt(pastMoves.Count - 1);

      map = pastMaps[pastMaps.Count-1];
      baba -= lastMove;
      lastMove = pastMoves[pastMoves.Count-1];

      ParseRules();
      UpdateDisplay();
    }
  }

  public void EraseHistory()
  {
    pastMaps = new List<Tile[,]>();
    pastMaps.Add(Tile.cloneMap(map,width,height));
    pastMoves = new List<Vector2Int>();
    pastMoves.Add(lastMove);
  }




   //The actual time the player will be able to fire input.
    private float nextInput;
    private Vector2Int checkInput()
    {
      //Delay detection
      float horiz = Input.GetAxisRaw("Horizontal");
      float verti = -Input.GetAxisRaw("Vertical");
      bool inputDown = (horiz != 0 || verti != 0);

      //Debug.Log(nextInput-Time.fixedTime);
      if(inputDown && nextInput - Time.fixedTime <= 0.01f)
      {



        nextInput = Time.fixedTime + inputRate;

        //Before Fixating the FPS:

        // // When Time is slowed, so is the inputRate because of time speed mess
        // if(ruleHandler.CheckEffectAndAssert("Time is Move"))
        // {
        //   nextInput = Time.fixedTime + inputRate;
        // }
        // else
        // {
        //   nextInput = Time.fixedTime + inputRateStop;
        // }

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
      else
      {
        bool latency = horiz == 0 && verti == 0;
        //latency = true;
        if(latency)
        nextInput = Time.fixedTime;
      }


      return Vector2Int.zero;
    }


    // Various utilitary
    public void UpdateDisplay()
    {
      renderer.UpdateDisplay();
    }

    public void OpenDisplay()
    {
      renderer.OpenDisplay();
    }

    public void CloseDisplay()
    {
      EraseHistory();
      renderer.CloseDisplay();
    }

    public bool isValidRule(string rule)
    {
      return validRules.Contains(rule);
    }

    public bool isValidWord(string word)
    {
      string titleCaseWord = word[0].ToString().ToUpper()+word.Substring(1).ToLower();
      return validWords.Contains(titleCaseWord);
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

  public Tile clone()
  {
    Tile clone = new Tile(name, empty, statik);
    clone.name = name;
    clone.spawn = spawn;
    clone.empty = empty;
    clone.statik = statik;
    clone.txt  = txt;
    clone.lockedId  = lockedId;
    return clone;
  }

  public static Tile[,] cloneMap(Tile[,] originalMap, int width, int height)
  {
    Tile[,] cloneMap = new Tile[width,height];
    for(int y = 0; y < height; y++)
    {
      for(int x = 0; x < width; x++)
      {
        cloneMap[x,y] = originalMap[x,y].clone();
      }
    }
    return cloneMap;
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
