using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public static class EnemyLibrary
{
    public static EnemyClass GetEnemyFromId(int id, battleBehavior thisBehavior)
    {
        switch (id)
        {
            case 0: return new Enemy1(thisBehavior);
            case 1: return new PoorDog(thisBehavior);
            case 2: return new RepressedGhost(thisBehavior);
            case 3: return new BoredGhost(thisBehavior);
            default: return null;
        }
    }
}
public abstract class EnemyClass
{
    battleBehavior thisBehavior;
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
    public EnemyResponse[] responses;
    public Sprite[,] GetSprites()
    {
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

    public EnemyClass(battleBehavior battle){
        thisBehavior = battle;
    }

    protected object[][] SingleMethod(object[] ob){
        return new object[][]{
            ob
        };
    }

    protected EnemyResponse GenResponse(ButtonEnum attack, int actionID, string methodName, object[][] parameters, string[] text){
        return new EnemyResponse(new EnemyActionCase((int)attack,actionID), new EnemyReaction[]{
            new EnemyReaction(new System.Reflection.MethodInfo[]{
                typeof(battleBehavior).GetMethod(methodName)//response methods names in battleBehavior
            }, 
            parameters,
            text,
            thisBehavior)
        });
    }
}

public class EnemyResponse{//a response containing a trigger and a reaction
    public EnemyActionCase trigger;
    EnemyReaction[] reactions;//to be invoked
    public EnemyResponse(EnemyActionCase triggerAction, EnemyReaction[] triggerReaction){
        trigger = triggerAction;
        reactions = triggerReaction;
    }
}

public class EnemyReaction{
    public string[] toDisplay;
    public System.Action React = () => { };
    public EnemyReaction(System.Reflection.MethodInfo[] methodInfos, object[][] methodParameters, string[] displayText, battleBehavior battle){
        toDisplay = displayText;  
        for (int i = 0; i < methodInfos.Length;i++){
            React += () => {
                methodInfos[i].Invoke(battle,methodParameters[i]);
            };
        }
    }
}

public class HeroClass
{
    public int hp = 10;
    public int maxHP = 10;
}

public class EnemyActionCase
{
    public int actionType;
    public int actionID;
    public EnemyActionCase(int type, int action)
    {
        actionType = type;
        actionID = action;
    }
}

public class Enemy1 : EnemyClass
{//example of an actual enemy
    public Enemy1(battleBehavior battle) : base(battle)
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
    public PoorDog(battleBehavior battle) : base(battle)
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

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,"DamageEnemyWeak",
                SingleMethod(new object[]{(object)2}),
                new string[]{
                    "You attacked with the theremin...",
                    "The ghost recoils at the pitch!",
                    "\"Whine.... Turn it off...\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,"DamageEnemy",
                SingleMethod(new object[]{(object)2}),
                new string[]{
                    "You attacked with the FirePoker...",
                    "The ghost isn't loving it... but isn't hating it, either.",
                    "\"Too heavy to be stick...\nTo long to be ball...\"",
                    "\":(\""
            })
        };
    }
}
public class RepressedGhost : EnemyClass
{//example of an actual enemy
    public RepressedGhost(battleBehavior battle) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "The Ghost Of Repressed Emotions";
        hp = 30;
        maxHP = 30;
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
    public BoredGhost(battleBehavior battle) : base(battle){
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "Bored Ghost";
        hp = 20;
        maxHP = 20;
        id = 3;
        spritePath = "Prefabs/EnemySpritePrefabs/BoredGhostSprite";
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight/Straight_Easy 5",
            "Prefabs/combatEnemyTurn/attacks/Sine Reversal/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/Sine Reversal/SineReverse_Easy1",
            "Prefabs/combatEnemyTurn/attacks/Sine/Sine_TooEasy"};
        //attackPrefabNames
        talkActions = new TalkEnum[1] { TalkEnum.Chat };
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/BoredGhostSprite";
    }
}