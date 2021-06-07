using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public static class EnemyLibrary
{
    public static EnemyClass GetEnemyFromId(int id)
    {
        switch (id)
        {
            case 0: return new Enemy1();
            case 1: return new PoorDog();
            case 2: return new RepressedGhost();
            case 3: return new BoredGhost();
            default: return null;
        }
    }
}
public abstract class EnemyClass
{
    public int id;
    public string name;
    public string[] toScroll;
    public int hp;
    public int maxHP;
    public string[] attackPrefabNames;
    public List<EnemyActionCase> sentiment;
    public string folderPath = "2D Assets/Programmer Art/GhostSprites";
    public string fileName = "ghost1";
    public string spritePath = "";
    public string displayPrefabPath = "";
    public int animationFrames = 1;
    public TalkEnum[] talkActions;
    public Sprite[,] GetSprites()
    {//TODO: FInISH
        Sprite[,] toReturn = new Sprite[4, animationFrames];
        string loadstring = folderPath + "/" + fileName;
        for (int i = 0; i < animationFrames; i++)
        {
            string istring = i.ToString();
            if (istring.Length<2){istring = "0"+istring;}
            toReturn[0, i] = Resources.Load<Sprite>(loadstring + "Forward" + i.ToString());
            toReturn[1, i] = Resources.Load<Sprite>(loadstring + "Backward" + i.ToString());
            toReturn[2, i] = Resources.Load<Sprite>(loadstring + "Left" + i.ToString());
            toReturn[3, i] = Resources.Load<Sprite>(loadstring + "Right" + i.ToString());
        }
        return toReturn;
    }
    public GameObject GetRandomAttack()
    {
        return Resources.Load(attackPrefabNames[Random.Range(0,attackPrefabNames.Length)], typeof(GameObject)) as GameObject;
    }
}

public class HeroClass
{
    public int hp = 10;
    public int maxHP = 10;
}

public class EnemyActionCase
{
    int actionType;
    int actionID;
    public EnemyActionCase(int type, int action)
    {
        actionType = type;
        actionID = action;
    }
}

public class Enemy1 : EnemyClass
{//example of an actual enemy
    public Enemy1()
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "Bored Ghost";
        hp = 20;
        maxHP = 20;
        id = 0;
        spritePath = "Prefabs/EnemySpritePrefabs/BoredGhostSprite";
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine/Sine_TooEasy"};
        talkActions = new TalkEnum[1] { TalkEnum.Chat };
    }

}
public class PoorDog : EnemyClass
{//example of an actual enemy
    public PoorDog()
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Pet)};
        name = "Poor Dog";
        hp = 10;
        maxHP = 10;
        id = 1;
        spritePath = "Prefabs/EnemySpritePrefabs/PoorDogSprite";
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight/Straight_TooEasy2",
            "Prefabs/combatEnemyTurn/attacks/Straight/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine Reversal/SineReverse_Tooeasy2",
            "Prefabs/combatEnemyTurn/attacks/Sine/Sine_TooEasy"};
        talkActions = new TalkEnum[3] { TalkEnum.Pet, TalkEnum.Chat, TalkEnum.Fake_Throw };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/PoorDogSprite";
    }

}
public class RepressedGhost : EnemyClass
{//example of an actual enemy
    public RepressedGhost()
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "The Ghost Of Repressed Emotions";
        hp = 40;
        maxHP = 40;
        id = 2;
        spritePath = "Prefabs/EnemySpritePrefabs/RepressedGhostSprite";
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Sine/Sine_Harder_Reverse",
            "Prefabs/combatEnemyTurn/attacks/Sine/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/Sine Reversal/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/Mix/Mix_Easy"};
        talkActions = new TalkEnum[2] { TalkEnum.Chat, TalkEnum.ChatTwo };
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/RepressedGhostSprite";
    }

}

public class BoredGhost : EnemyClass{
    public BoredGhost(){
        //sentiment
        name = "Bored Ghost";
        hp = 20;
        maxHP = 20;
        id = 3;
        //spritePath
        //attackPrefabNames
        //talkActions
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/BoredGhostSprite";
    }
}

public class SomeEnemy : EnemyClass
{

}