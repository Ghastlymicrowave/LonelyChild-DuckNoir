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
    public int animationFrames = 1;
    public TalkEnum[] talkActions;
    public Sprite[,] GetSprites()
    {//TODO: FInISH
        Sprite[,] toReturn = new Sprite[4, animationFrames];
        string loadstring = folderPath + "/" + fileName;
        for (int i = 0; i < animationFrames; i++)
        {
            toReturn[0, i] = Resources.Load<Sprite>(loadstring + "Forward" + i.ToString());
            toReturn[1, i] = Resources.Load<Sprite>(loadstring + "Backward" + i.ToString());
            toReturn[2, i] = Resources.Load<Sprite>(loadstring + "Left" + i.ToString());
            toReturn[3, i] = Resources.Load<Sprite>(loadstring + "Right" + i.ToString());
        }
        return toReturn;
    }
    public GameObject GetRandomAttack()
    {
        return Resources.Load(attackPrefabNames[0], typeof(GameObject)) as GameObject;//attackPrefabNames[Random.Range(0,attackPrefabNames.Length)]
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
        name = "Enemy 1";
        hp = 20;
        maxHP = 20;
        id = 1;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Mix/Mix_Easy",
            "Prefabs/combatEnemyTurn/attacks/Sine/Sine_TooEasy"};
        talkActions = new TalkEnum[1] { TalkEnum.Chat };
    }

}
public class PoorDog : EnemyClass
{//example of an actual enemy
    public PoorDog()
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "Poor Dog";
        hp = 10;
        maxHP = 10;
        id = 2;
        folderPath = "EnemySprites/PoorDogConcept";
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight/Straight_TooEasy2",
            "Prefabs/combatEnemyTurn/attacks/Straight/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine Reversal/SineReverse_Tooeasy2",
            "Prefabs/combatEnemyTurn/attacks/Sine/Sine_TooEasy"};
        talkActions = new TalkEnum[3] { TalkEnum.Pet, TalkEnum.Chat, TalkEnum.FakeThrow };
        

    }

}

public class SomeEnemy : EnemyClass
{

}