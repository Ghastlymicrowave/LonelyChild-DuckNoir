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
            case 4: return new Tutorial(thisBehavior);
            default: return null;
        }
    }
    public static EnemyClass GetRawEnemyFromId(int id)
    {
        Debug.Log("Getting raw enemy data");
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
    battleBehavior thisBehavior;
    public bool canRun = true;
    public int id;
    public string name;
    public string[] toScroll;
    public int hp;
    public int maxHP;
    public string[] attackPrefabNames;
    public List<EnemyActionCase> sentiment;
    public string folderPath = "2D Assets/OverworldGhost/Overworld_Ghost_";
    public string displayPrefabPath = "";
    public int animationFrames = 1;
    public TalkEnum[] talkActions;
    public EnemyResponse[] responses;
    public string[] sentimentalSuccess;
    public string[] sentimentalFaliure;
    public EnemyActionCase sentimentalTrigger;
    public string[] splashTexts = new string[]{"The ghost hovers ominously..."};
    public Sprite[,] GetSprites()
    {
        Debug.Log("Getting sprites");
        Sprite[,] toReturn = new Sprite[3, animationFrames];
        
        for (int i = 1; i < animationFrames+1; i++)
        {
            Debug.Log("current: "+i.ToString()+" out of frames: "+animationFrames.ToString());
            string istring = i.ToString();
            string loadstring = folderPath;
            if (istring.Length<2){istring = "0"+istring;}
            toReturn[0, i-1] = Resources.Load<Sprite>(loadstring + "Front" + istring);
            toReturn[1, i-1] = Resources.Load<Sprite>(loadstring + "Back" + istring);
            toReturn[2, i-1] = Resources.Load<Sprite>(loadstring + "Side" + istring);
            Debug.Log(toReturn[0, i-1].ToString()+toReturn[0, i-1].ToString()+toReturn[0, i-1].ToString());
        }
        return toReturn;
    }
    public EnemyReaction GetReaction(ButtonEnum type, int actionID){//returns reactions array or null
        for (int i = 0; i < responses.Length;i++){
            if (responses[i].trigger.actionType==(int)type && responses[i].trigger.actionID==actionID){
                return responses[i].reactions;
            }
        }
        return null;
    }
    public GameObject GetRandomAttack()
    {
        return Resources.Load(attackPrefabNames[Random.Range(0,attackPrefabNames.Length)], typeof(GameObject)) as GameObject;
    }

    public EnemyClass(battleBehavior battle = null){
        
        if (battle!=null){thisBehavior = battle;}
    }

    protected object[] SingleMethod(params object[] ob){
        return ob;
    }

    protected EnemyResponse GenResponse(ButtonEnum attack, int actionID, string methodName, object[] parameters, string[] text){
        return new EnemyResponse(new EnemyActionCase((int)attack,actionID), 
        new EnemyReaction(
        typeof(battleBehavior).GetMethod(methodName), 
        parameters,
        text,
        thisBehavior));
    }
}

public class EnemyResponse{//a response containing a trigger and a reaction
    public EnemyActionCase trigger;
    public EnemyReaction reactions;//to be invoked
    public EnemyResponse(EnemyActionCase triggerAction, EnemyReaction triggerReaction){
        trigger = triggerAction;
        reactions = triggerReaction;
    }
}

public class EnemyReaction{
    public string[] toDisplay;
    public System.Action React = () => { };
    public EnemyReaction(System.Reflection.MethodInfo methodInfo, object[] methodParameters, string[] displayText, battleBehavior battle = null){
        if( battle==null){return;}
        toDisplay = displayText;  
        React = () => {methodInfo.Invoke(battle, methodParameters);};
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
    public Enemy1(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "Bored Ghost";
        hp = 20;
        maxHP = 20;
        id = 0;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[1] { TalkEnum.Chat };
    }

}
public class PoorDog : EnemyClass
{//example of an actual enemy
    public PoorDog(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Pet)};
        name = "Poor Dog";
        hp = 10;
        maxHP = 10;
        id = 1;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy2",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Tooeasy2",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[3] { TalkEnum.Pet, TalkEnum.Chat, TalkEnum.Fake_Throw };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/PoorDogDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Ball);

        sentimentalSuccess = new string[]{
            "It felt... right.",
            "\"BALL!?!?!???!?\"",
            "\"!?!?!?!???!??!!?!??!??!?!??!?!?!??!?\"",
            "\"!?!?!?!???!??!!?!!?!?!?!??!?!?!?!?!????!??!?!??!?!?!??!?\"",
            "\"!?!?!?!?!?!?!?!????!??!?!??!?!???!??!!?!!?!?!?!??!?!?!?!?!????!??!?!??!?!?!??!?\"",
            "..."
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the ball...",
            "Does this ball mean something to it?",
            "\"I feel like there could be ball...?\"",
            "\"But... No see ball?????\"",
            "It snaps out of it's trance, was there something you needed to do first?"
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,"DamageEnemy",
                SingleMethod((object)4),
                new string[]{
                    "You attacked with the theremin...",
                    "The ghost recoils at the pitch!",
                    "\"Whine.... Turn it off...\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,"DamageEnemy",
                SingleMethod((object)2),
                new string[]{
                    "You attacked with the FirePoker...",
                    "The ghost isn't loving it... but isn't hating it, either.",
                    "\"Too heavy to be stick...\nTo long to be ball...\"",
                    "\":(\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,"DamageEnemy",
                SingleMethod((object)1),
                new string[]{
                    "You attacked with the flashlight...",
                    "The ghost starts darting after the light, thinking it's a ball!",
                    "You go on for a few minutes, making the ghost run in circles.",
                    "This isn't exactly effective...",
                    "\"Where did flat ball go?????\"",
                    "\"Do you have better ball!?!?!?!?!?\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,"DamageEnemy",
                SingleMethod((object)4),
                new string[]{
                    "You attacked with the Garlic...",
                    "The ghost hates it!",
                    "\"Smelly ball bad for me!!! Give better ball >:(\""
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,"DamageEnemy",
                SingleMethod((object)1),
                new string[]{
                    "You started talking with the ghost...",
                    "You might've briefly mentioned something that sounds vaguely like the word 'ball'.",
                    "\"Ball!?!?!?!?!?\"",
                    "\"...\"",
                    "\"Ohh... False Ball-arm...\""
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Fake_Throw,"DamageEnemy",
                SingleMethod((object)2),
                new string[]{
                    "You made a throwing motion with your arm...",
                    "But there was nothing in your hand?",
                    "\"How could you!!!???? >:( >:( >:(\""
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Pet,"DamageEnemy",
                SingleMethod((object)2),
                new string[]{
                    "You tried to pet the ghost...",
                    "But your arm phased right through 'em, so...",
                    "You just kinda made a petting motion with your arm.",
                    "Between you and me, I don't think he knows the difference.",
                    "\"Woof!~ :)\""
            })
        };
    }
}
public class Tutorial : EnemyClass
{//example of an actual enemy
    public Tutorial(battleBehavior battle) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Pet)};
        name = "Ghost Hunting Nerd";
        hp = 6;
        maxHP = 6;
        id = 4;
        canRun = true;
        spritePath = "Prefabs/EnemySpritePrefabs/PoorDogSprite";
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Tooeasy2",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[1] {TalkEnum.Chat};
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/TutorialDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Manual);

        sentimentalSuccess = new string[]{
            "Oh, I see... You already have a manual...",
            "\"I suppose I've taught you everything you need to know about battle sequences...\"",
            "\"I guess I’m just not useful anymore…\"",
            "\"That’s okay with me!  Sayonara!\"",
            
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the manual...",
            "Does this manual mean something to it?",
            "\"Ooh, this is my favorite part! Let me hear you out!\"",
            "\"You're supposed to show me the thing AFTER you beat me up and talk to me!\"",
            "\"It's the cool way to end battles!\"",
            "It snaps out of it's trance, was there something you needed to do first?"
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,"DamageEnemy",
                SingleMethod((object)1),
                new string[]{
                    "You attacked with the theremin...",
                    "The ghost is relatively unfazed!",
                    "\"Ooh, budy, I got a resistance to this attack.\"",
                    "\"You've gotta try the other attack!\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,"DamageEnemy",
                SingleMethod((object)2),
                new string[]{
                    "You attacked with the FirePoker...",
                    "The ghost isn't loving it... but isn't hating it, either.",
                    "\"Too heavy to be stick...\nTo long to be ball...\"",
                    "\":(\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,"DamageEnemy",
                SingleMethod((object)2),
                new string[]{
                    "You attacked with the flashlight...",
                    "\"Oh god! My eyes! It burns!\"",
                    "\"Ha. Just kidding.\" \"I don't have eyes.\"",
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,"DamageEnemy",
                SingleMethod((object)4),
                new string[]{
                    "You attacked with the Garlic...",
                    "The ghost hates it!",
                    "\"How'd you even get that?\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Ruler,"DamageEnemy",
                SingleMethod((object)6),
                new string[]{
                    "You attacked with the Ruler...",
                    "\"...\"",
                    "\"What's wrong with you?'\""
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,"DamageEnemy",
                SingleMethod((object)1),
                new string[]{
                    "You started talking with the ghost...",
                    "\"Hey! You're talking to me!\"",
                    "\"Have you been reading the manual?\"",
                    "\"You can talk to ghosts around here to figure out what their issue is!\"",
                    "\"I don't have any issues, I'm just a tutorial, but some of these guys are absolute class acts...\"",
                    "\"...They won't stop fighting unless you really level with them.\""
            }),
            
        };
    }
}
public class RepressedGhost : EnemyClass
{//example of an actual enemy
    public RepressedGhost(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "The Ghost Of Repressed Emotions";
        hp = 30;
        maxHP = 30;
        id = 2;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder_Reverse",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/Mix_Easy"};
        talkActions = new TalkEnum[2] { TalkEnum.Chat, TalkEnum.ChatTwo };
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/RepressedGhostDisplay";

        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Photo);

        sentimentalSuccess = new string[]{
            "You showed the Photo to the ghost...",
            "It felt... right.",
            "\"Thaaaank yyooouu\""
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the Photo...",
            "That expression on his face looks painful.",
            "\"Oh why, oh why...\""
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,"DamageEnemy",
                SingleMethod((object)2),
                new string[]{
                    "You attacked with the theramin...",
                    "The ghost looks indifferent",
                    "\"Are you picking on me?\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,"DamageEnemy",
                SingleMethod((object)1),
                new string[]{
                    "You attacked with the FirePoker...",
                    "The ghost is unphased",
                    "\"I couldn't feel that... I haven't been able to feel for a while...\"",
                    "\"Ouch that hurts...? is that want you want me to say?\""
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,"DamageEnemy",
                SingleMethod((object)1),
                new string[]{
                    "You started talking with the ghost...",
                    "\"I just wish things had been different, you know?\""
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.ChatTwo,"DamageEnemy",
                SingleMethod((object)1),
                new string[]{
                    "You started talking with the ghost about their life...",
                    "\"Well, it's just...\"",
                    "\"They wanted different things from what I wanted...\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,"DamageEnemy",
                SingleMethod((object)5),
                new string[]{
                    "You attacked with the flashlight...",
                    "The ghost has a painful expression on his face.",
                    "\"Please don't do that...\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,"DamageEnemy",
                SingleMethod((object)4),
                new string[]{
                    "You attacked with the Garlic...",
                    "The ghost didn't like that too much.",
                    "\"Eww...\""
            })
        };
    }

}

public class BoredGhost : EnemyClass{
    public BoredGhost(battleBehavior battle = null) : base(battle){
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "Bored Ghost";
        hp = 20;
        maxHP = 20;
        id = 3;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_Easy 5",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy1",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        //attackPrefabNames
        talkActions = new TalkEnum[1] { TalkEnum.Chat };
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/BoredGhostDisplay";

        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Ball);
        sentimentalSuccess = new string[] {
            "You showed the ball to the ghost...",
            "It felt... right.",
            "\"Thank you...\""
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the ball...",
            "Does this ball mean something to it?",
            "It snaps out of it's trance, you must have been too soon."
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,"DamageEnemy",
                SingleMethod((object)2),
                new string[]{
                    "The ghost... liked it?",
                    "\"That's nice...\"",
                    "\"Not really my genre though.\"",
                    "\"I'm more of a 'Boos' kind of guy.\""
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,"DamageEnemy",
                SingleMethod((object)6),
                new string[]{
                    "You attacked with the flashlight...",
                    "It was especially effective!",
                    "\"Ow, who turned on the lights?\""
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,"DamageEnemy",
                SingleMethod((object)1),
                new string[]{
                    "You started talking with the ghost...",
                    "\"Sigh... Alright...\""
            })
        };
    }
}