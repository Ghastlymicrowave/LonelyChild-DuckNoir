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
            case 5: return new Narcissist(thisBehavior);
            case 6: return new TroubledChild(thisBehavior);
            case 7: return new NiceDemonGuy(thisBehavior);
            case 8: return new DevilsHands(thisBehavior);
            case 9: return new GremlinOfDeceit(thisBehavior);
            default: return null;
        }
    }
    public static EnemyClass GetRawEnemyFromId(int id)
    {
        switch (id)
        {
            case 0: return new Enemy1();
            case 1: return new PoorDog();
            case 2: return new RepressedGhost();
            case 3: return new BoredGhost();
            case 4: return new Tutorial();
            case 5: return new Narcissist();
            case 6: return new TroubledChild();
            case 7: return new NiceDemonGuy();
            case 8: return new DevilsHands();
            case 9: return new GremlinOfDeceit();
            default: return null;
        }
    }
}
public abstract class EnemyClass
{
    public bool playerHurt = true;
    battleBehavior thisBehavior;
    public bool canRun = true;
    public int id;
    public string name;
    public string[] toScroll;
    public int hp;
    public int maxHP;
    public string[] attackPrefabNames;
    public List<string> sentiment;
    public string folderPath = "2D Assets/OverworldGhost/Overworld_Ghost_";
    public string displayPrefabPath = "";
    public int animationFrames = 1;
    public TalkEnum[][] talkActions;
    public EnemyResponse[] responses;
    public string[] sentimentalSuccess;
    public string[] sentimentalFaliure;
    public EnemyActionCase sentimentalTrigger;
    public string[] splashTexts = new string[]{"The ghost hovers ominously..."};
    public SpecialText[] specialTexts;
    public int[] specialVals = {0};
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
    public int[] GetIgnoreVals(){
        int[] toReturn = new int[specialVals.Length];
        for(int i = 0; i < specialVals.Length; i++){
            toReturn[i] = -1;
        }
        return toReturn;
    }
    public EnemyReaction GetReaction(ButtonEnum type, int actionID, params int[] spVals){//returns reactions array or null
        if (spVals == null){
            spVals = new int[] {0};
        }
        for (int i = 0; i < responses.Length;i++){

            if (responses[i].trigger.actionType==(int)ButtonEnum.Any){//if type is any
                bool cont = false;
                    for(int a = 0; a < responses[i].specialVal.Length; a++){//check for vals
                        if (responses[i].specialVal[a] == -1){//ignoring param
                            continue;
                        }
                        if (responses[i].specialVal[a] != spVals[a]){
                            cont = true;
                        }
                    }
                if (cont){continue;}
                return responses[i].RandReact();
            }else if (responses[i].trigger.actionType==(int)type && responses[i].trigger.actionID==actionID){
                bool cont = false;
                    for(int a = 0; a < responses[i].specialVal.Length; a++){//check for vals
                        if (responses[i].specialVal[a] == -1){//ignoring param
                            continue;
                        }
                        if (responses[i].specialVal[a] != spVals[a]){
                            cont = true;
                        }
                    }
                if (cont){continue;}
                return responses[i].RandReact();
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

    protected EnemyResponse GenResponse(ButtonEnum attack, int actionID, EnemyReaction[] reactions, params int[] specialVals){
        if (specialVals== null){
            specialVals = GetIgnoreVals();
        }
        return new EnemyResponse(new EnemyActionCase((int)attack,actionID), reactions, specialVals);
    }

    protected EnemyResponse GenAny(EnemyReaction[] reactions, params int[] specialVals){
        if (specialVals== null){
            specialVals = GetIgnoreVals();
        }
        return new EnemyResponse(new EnemyActionCase((int)ButtonEnum.Any,0),reactions,specialVals);
    }

    protected EnemyReaction NewReaction(string[] methodNames, string[] text, object[][] parameters){
        System.Reflection.MethodInfo[] methods = new System.Reflection.MethodInfo[methodNames.Length];
        for(int i = 0; i < methodNames.Length; i++){
            //methods[i] = typeof(battleBehavior).GetMethod(methodNames[i]);
        }
        
        return new EnemyReaction(
            methodNames, 
            parameters,
            text,
            thisBehavior);
    }
}

public class EnemyResponse{//a response containing a trigger and a reaction
    public EnemyActionCase trigger;
    public EnemyReaction[] reactions;//to be invoked
    public int[] specialVal = {0};
    public EnemyReaction RandReact(){
        return reactions[Random.Range(0,reactions.Length)];
    }
    public EnemyResponse(EnemyActionCase triggerAction, EnemyReaction[] triggerReactions, params int[] vals){
        trigger = triggerAction;
        reactions = triggerReactions;
        if (vals!=null){
            specialVal = vals;
        }
    }
}

public class EnemyReaction{
    public string[] toDisplay;
    public string[] methodNames;
    public object[][] methodParams;
    public EnemyReaction(string[] names, object[][] methodParameters, string[] displayText, battleBehavior battle = null){
        if(battle==null){return;}
        toDisplay = displayText;  
        methodNames = names;
        methodParams = methodParameters;
    }
}

public class HeroClass
{
    public int hp = 20;
    public int maxHP = 20;
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
        sentiment = new List<string>{
            "Chat"};
        name = "Bored Ghost";
        hp = 20;
        maxHP = 20;
        id = 0;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat} 
        };
    }

}
public class Narcissist : EnemyClass

{//example of an actual enemy
    public Narcissist(battleBehavior battle = null) : base(battle)
    {
        name = "The Narcissist";
        hp = 20;
        maxHP = 20;
        id = 5;
        canRun = false;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy1",
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_2",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat, TalkEnum.Call_Him_Bald} ,
            new TalkEnum[]{TalkEnum.Gloat, TalkEnum.Compliment, TalkEnum.Insult} ,
            new TalkEnum[]{TalkEnum.Lecture},
            new TalkEnum[]{} 

        };
        sentiment = new List<string>{"call_him_bald","gloat","lecture"};
        specialVals = new int[]{0,0};
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/NarcissistDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Hourglass);

        sentimentalSuccess = new string[]{
            "You present the hourglass.",
            "\"My goodness, that’s it!.\", \"My lucky hourglass!\"",
            "\"Without it, I felt like the slightest inconvenience took a lifetime.\"\n\"But, now, I feel… complete?\"",
            "\"Time is all we have, but I’ve been wasting so much...\"\n\"My search for self-centeredness, ironically, was self-centered of me...\"",
            "\"Thank you.\""
        };
        sentimentalFaliure = new string[]{
            "You present the Hourglass.\nThe ghost isn’t quite ready for it yet.",
            "\"I smell a faint pretention in the air...\"\n\"...could it be?\"",
            "\"No… It couldn’t be.\""
        };

        responses = new EnemyResponse[]{
            
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You chat up the enemy ghost.\nHe slaps you across the face!",
                    "\"You seriously think I have the TIME for idle chatter?\"",
                    "\"How about I adopt some other orphan boy?\"\n\"Preferably someone I can berate into my OWN image.\""

                },new object[][]{SingleMethod(3)})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Call_Him_Bald,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You insult the ghost’s hairless scalp, going so far as to use it as a mirror for admire your own form.",
                    "\"WHAT???\"\n\"You dare insult the likes of me!?\"",
                    "\"How CRUDE!\"\n \"How PRUDENT!\"\n \"How...\"",
                    "\"...exactly like me?\""

                },new object[][]{SingleMethod(new int[]{1}),SingleMethod(1),SingleMethod("call_him_bald",new string[]{},new string[]{})})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Compliment,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You admire the ghost’s figure.\nHe slaps you across the face!",
                    "\"You think I don’t know how GLORIOUS and UNFLAPPABLE I am!?\"",
                    "\"I’d like to adopt someone who respects my intellect a little bit more.\"\n\"I don’t have TIME to hear what I already know...\""

                },new object[][]{SingleMethod(3)})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Gloat,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You gloat, going on about your appearance and your accomplishments.",
                    "If you actually even one of the things you said you did, you’d surely earn a nobel prize...",
                    "\"YES, Finally!\" Another piece of the puzzle!\"",
                    "\"I’ve rubbed off on you already!\"\n\"The attitude AND the ego!\"",
                    "\"But… I think something’s missing yet...\""

                },new object[][]{SingleMethod(new int[]{2}),SingleMethod(2),SingleMethod("gloat",new string[]{},new string[]{})})
            },new int[]{1}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Insult,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "You insult the narcissist once more.\nThis time, going after his flabby body.",
                    "\"Hey, I appreciate the moxxy, kid, but are insults really the only thing you’ve got?\"",
                    "\"I don’t have TIME for a one-note kid.\"\n\"You’re gonna have to try harder to impress me.\""

                },new object[][]{})
            },new int[]{1}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Gloat,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You go on for as long as you can muster, lecturing the ghost on every minute thing you can think of.",
                    "His shirt is SO last century…\nHis shoes? Look! They’re untied!",
                    "Your inflated ego is so massive, it could break the very floor you’re standing on.",
                    "\"How… perfect!",
                    "\"I’ve rubbed off on you... \"\n\"In only a matter of minutes!\"",
                    "\"I…\"\n\"...I simply MUST adopt you!\"",
                    "\"But… we can’t go home, not yet!\"\n\"I’m missing something important to me...\"",
                    "\"What was it...\"\n\"Do you happen to have it?\""
                },new object[][]{SingleMethod(new int[]{3}),SingleMethod(3),SingleMethod("lecture",new string[]{},new string[]{})})
            },new int[]{2}),
        };
    }
}
public class TroubledChild : EnemyClass
{//example of an actual enemy
    public TroubledChild(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<string>{"chat"};
        name = "A Troubled Child";
        hp = 15;
        maxHP = 15;
        id = 6;
        canRun = false;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_3",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy2",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Harder_Reverse",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat} 
        };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/TroubledChildDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Teddy_Bear);

        sentimentalSuccess = new string[]{
            "You held out the Teddy Bear...\nIt felt... right.",
            "\"Could it be?\"",
            "\"This was the only comfort I had.\"\n\"They took it away, and you found it!.\"",
            "\"...Thank you.\"",
            "\"Look at you dragging out the philosophy!\"\n\"I might have been wrong about you!\"",
            "\"...\"",
            "\"All I think about is myself...\"",
            "\"If you can change...\"\n\"...can the likes of me can change, too?\"",
            "..."
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the Teddy Bear...",
            "For a moment, he looks content.",
            "Does this Teddy Bear mean something to it?"
            
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the theremin...",
                    "\"They used to play music...\"\n\"Down there...\""
                },new object[][]{SingleMethod((object)3)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Please no!\"", "\"Not like...\" \n\"...they did.\""
                },new object[][]{SingleMethod((object)4)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the flashlight...",
                    "\"It burns...\"\n\"It burns...\"",
                    "\"It burns like when they...\"\n\"No!\""
                },new object[][]{SingleMethod((object)4)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Garlic...",
                    "\"Why'd they only feed you?\"\n\"What did I do?\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You started talking with the ghost...",
                    "\"Are you...good?\"\n\"You're not here to hurt me?\"",
                    "\"Do you want to stop them?\"",
                    "\"He stays below... in the basement!\""
                },new object[][]{SingleMethod((object)3)})
            }),
        };
    }
}
public class NiceDemonGuy : EnemyClass
{//example of an actual enemy
    public NiceDemonGuy(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<string>{"chat"};
        name = "NiceDemonGuy";
        hp = 15;
        maxHP = 15;
        id = 7;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_3",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Tooeasy2"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat} 
        };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/NiceDemonGuyDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Russian_Doll);

        sentimentalSuccess = new string[]{
            "You held out the Russian Doll...\nIt felt... right.",
            "\"Woah, hey, where the heck did ya find this?\"",
            "\"This was my favorite toy back when I was just a wee demon-in-training!\"",
            "\"Brings back so many warm memories...\"",
            "\"And I don't exaggerate when I say WARM, heh, because, you know...\"",
            "\"...I'm a demon.\"\n(I told you I was a demon, didn't I?)",
            "\"Oh, I'm rambling again.\"\n\"Dang...\"",
            "\"...Before I go, I need you to know something.\"",
            "\"Steer clear of the hole in the living room.\"",
            "\"You don't want anything to do with what's down here, I'm telling ya.\""
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the Russian Doll...",
            "Does this Russian Doll mean something to it?",
            "\"Hey, little buddy, what was that you had in your hand just then?\""
            
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the theremin...",
                    "\"Say, you're good with that thing!\"\n\"Down there...\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Listen, kid... I'm a demon for cryin' out loud.\"", "\"A fire poker just ain't gonna do the trick, sorry to say.\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the flashlight...",
                    "\"What're you shining that in my face for?\"",
                    "\"I'm like a demon guy or whatever.\"\n\"You couldn't possibly expect me to have a meaningful reaction to a flashlight, could ya?\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Garlic...",
                    "\"I'm a demon, not a vampire...\"\n\"...but I'm flattered.\"",
                    "\"Guess I could take some extra damage for the unintended compliment.\""
                },new object[][]{SingleMethod((object)2)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You started talking with the ghost...",
                    "\"Oh, man, no one's talked to me like that in eons!\"\n(Maybe the whole floating demon head thing puts people off???)",
                    "\"Even other demons stay clear!\"\n\"I don't even know why, because...\"",
                    "\"ah, look, I'm rambling...\""
                },new object[][]{SingleMethod((object)5)})
            }),
        };  
    }
}
public class GremlinOfDeceit : EnemyClass
{//example of an actual enemy
    public GremlinOfDeceit(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<string>{"chat"};
        name = "The Gremlin Of Deceit";
        hp = 15;
        maxHP = 15;
        id = 9;

        canRun = false;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder_Reverse",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/Mix_Easy",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy2",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat} 
        };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/GremlinOfDeceitDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Eraser);

        sentimentalSuccess = new string[]{
            "You held out the Eraser...\nIt felt... right.",
            "\"Oh my gods!\"\n\"My power is eviscerated with one fell blow?\"",
            "\"No!\"\n\"No! No! No!\"",
            "\"...\"",
            "\"I didn't bring these ghouls here, if you must know...\"",
            "\"There really is someone down below.\"\n\"Oh!\"",
            "\"You better find your way out while you still can.\""
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the Eraser...",
            "Does this Eraser mean something to it?",
            "\"You dare!?\"\n\"Haven't you learned not to challenge my authority\""
            
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the theremin...",
                    "\"BAH!\"\n\"The ungainly sounds of that mid century hogwash!\"",
                    "*ahem*\n\"That is, I mean...\"",
                    "\"I think you sound lovely, dear...\"\n\"Come closer, I can teach you a thing or two about music...\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Oooo, that burns so sweet...\"\n\"But I know just the right spots!\""
                },new object[][]{SingleMethod((object)3)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the flashlight...",
                    "\"Ah, you little runt!\"\n\"I OUGHTA-\"",
                    "*ahem*",
                    "\"Why don't ya just give that little flashlight here, my sweet?\"\n\"Gremlin'll show you how to really use it.\""
                },new object[][]{SingleMethod((object)2)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Garlic...",
                    "\"What exactly did you think that would do?\"",
                    "\"Do you really think your superstitious tricks will help you?\"\n\"Look around you. Look where that brought your friends.\"",
                    "\"Besides...\"\n\"You know what happens when you sneak food...\""
                },new object[][]{SingleMethod((object)2)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You started talking with the ghost...",
                    "\"Do you remember me?\"\n\"From nightmares long ago?\"",
                    "\"Children draw from my form, you know!\"\n(Oh, my rhyming schemes!)",
                    "\"Am I responsible for reality splitting at the seams?\"\n\"Perhaps...\""
                },new object[][]{SingleMethod((object)4)})
            }),
        };  
    }
}
public class DevilsHands : EnemyClass
{//example of an actual enemy
    public DevilsHands(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<string>{"chat"};
        name = "Devil's Hands";
        hp = 12;
        maxHP = 12;
        id = 8;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_3",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy1",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy1",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Tooeasy2"
            };
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat} 
        };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/DevilsHandsDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Spinning_Toy);

        sentimentalSuccess = new string[]{
            "You held out the Spinning Toy...\nIt felt... right.",
            "\"Why, Lookie here!\"\n\"I remember this...\"",
            "\"So much free time spent, so many hands occupied...\"\n\"So much time not spent...\"",
            "\"...in the basement.\"",
            "\"Thank you.\""
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the Spinning Toy...",
            "For a he's moment, he's able to stand still.\nIt isn't long, though.",
            "Does this Spinning Toy mean something to it?"
            
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the theremin...",
                    "\"I've always thought of taking up music.\"",
                    "\"They would never let me.\"\n\"Too much noise\"",
                    "\"The devil plays music.\"\n\"Young ones like me should practice more... wholesome passtimes.\""
                },new object[][]{SingleMethod((object)3)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Oh, that familiar sting!\"\n\"What's wrong with you?\""
                },new object[][]{SingleMethod((object)3)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the flashlight...",
                    "\"Are you trying to shine a light on the subject?\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Garlic...",
                    "\"Where'd you get that?\"\n\"Do you have any idea what they'll do if they catch you sneaking food?\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You started talking with the ghost...",
                    "\"When I was a young thing, I could never keep my hands still.\"\n\"Always fidgeting... fidget and fidget.\"",
                    "\"I had to be punished.\"",
                    "\"No good boy fidgets like that...\"\n\"...no good deeds can come from that...\""
                },new object[][]{SingleMethod((object)2)})
            }),
        };  
    }
}
public class PoorDog : EnemyClass
{//example of an actual enemy
    public PoorDog(battleBehavior battle = null) : base(battle)
    {
        name = "Poor Dog";
        hp = 14;
        maxHP = 14;
        id = 1;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Turret1",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Pet,TalkEnum.Chat,TalkEnum.Fake_Throw}, 
            new TalkEnum[]{} 
        };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/PoorDogDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Ball);

        sentiment = new List<string>{"pet"};
        specialVals = new int[]{0,0};
        sentimentalSuccess = new string[]{
            
            "You held out the ball...\nIt felt... right.",
            "\"BALL!?!?!???!?\"",
            "\"!?!BORK?!?!???!?WOOF?!!?!??!?WOOF?!?!??!?BARK!?!??!?\"",
            "... Goodbye!"
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the ball...",
            "Does this ball mean something to it?",
            "\"I feel like there could be ball...?\"",
            "\"But... No see ball?????\"",
            "It snaps out of it's trance, was there something you needed to do first?"
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the theremin\nThe ghost recoils at the pitch!",
                    "\"Whine… Turn it off!..\""
                },new object[][]{SingleMethod(3)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the theremin\nThe ghost seems to be in a lot of pain...",
                    "\"BARK BARK BARK BARK BARK! >:(\""
                },new object[][]{SingleMethod(4)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the flashlight..\nThe ghost starts darting after the light, thinking it’s a ball!.",
"You go on for a minute, making the ghost run in circles.\nThis isn’t very effective.",
"\"WHERE DID FLAT BALL GO?????\""

                },new object[][]{SingleMethod(1)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the flashlight..\nThe ghost stares intently at the flashlight’s bulb!",
"\"Bright ball...\"\n\"Give me your wisdom...\"",
"This isn’t very effective."

                },new object[][]{SingleMethod(1)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "You started chatting up the ghost…\nYou may have said something that sounds vaguely like the word ball.",
"\"BALL!??!??!!?!!?\"\n\"...\"",
"\"Oh… False ball-arm...\""

                },new object[][]{})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Pet,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem","ChangeSpecialAbs","ChangeTalks"},
                new string[]{
                    "You tried to pet the ghost…\nBut your arm just phased right through the dog, so...",
"You just… kinda… made a petting motion with your arm.\nBetween you and me, I don’t think he knows the difference.",
"\"Woof!~ :)\""
                },new object[][]{SingleMethod("pet",new string[]{}, new string[]{}),SingleMethod(new int[]{1}),SingleMethod(1)})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Fake_Throw,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You made a throwing motion with your arm…\n...but there was nothing in your hand?",
"\"You monster!!!\"\n\"How could you!?!!?? >:( >:( >:(\""
                },new object[][]{SingleMethod(1)})
            }),
        };  
    }
}
public class Tutorial : EnemyClass
{//example of an actual enemy
    public Tutorial(battleBehavior battle = null) : base(battle)
    {
        playerHurt = true;
        sentiment = new List<string>{"talk"};//define setniment tags
        name = "Ghost Hunting Nerd";
        hp = 6;
        maxHP = 6;
        id = 4;
        canRun = false;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Tooeasy2",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Talk} ,
            new TalkEnum[]{TalkEnum.Chat,TalkEnum.Requrest_Health,TalkEnum.Requrest_Proceed} ,
            new TalkEnum[]{} 
        };
        specialVals = new int[]{0,0};
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/TutorialDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Manual);

        sentimentalSuccess = new string[]{
            "You hold out the manual.",
            "\"Oh, I see... You have the manual!\"",
            "\"I suppose I've taught you everything you need to know about battle sequences...\"",
            "\"I guess I’m just not useful anymore…\"",
            "\"That’s okay with me! Sayonara!\"",
            
        };
        sentimentalFaliure = new string[]{
            "You present the manual.",
            "\"Ah, that’s my sentimental item...\"\n\"You might want to use that after you’ve lit all your bulbs...\"",
            "\"Talk to me for more information.\""
        };

        specialTexts = new SpecialText[]{
            new SpecialText("PLAYER_DAMAGED",
            new string[]{
                "oops you took damage!"
            },1),
            new SpecialText("ENEMY_DAMAGED",
            new string[]{
                "ouch I took damage!"
            },1),
            new SpecialText("GHOST_HP_0",
            new string[]{
                "oops you took damage!"
            },1)
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Theremin.\nIt’s not so effective",
                    "\"You know, I’m resistant to this attack.\"\n\"You might wanna try the other attack.\""
                },new object[][]{SingleMethod(1)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Theremin.\nIt’s not so effective.",
                    "\"I’m resistant to this attack.\"\n\"That means that this attack does less damage to me than your flashlight.\""
                },new object[][]{SingleMethod(1)}),
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Flashlight.\nIt’s pretty effective!",
                    "\"Ouch, I’m weak to this attack!\"\n\"If you keep this up, my HP will be at zero.\"", "\"When my HP falls to zero, you can use the crucifix to end the battle!\"\n\"But there’s another much less painful way too, please talk to me to learn about that. Crucifixion really hurts...\""

                },new object[][]{SingleMethod(2)}),
            },new int[]{-1,0}),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Flashlight.\nIt’s pretty effective!",
                    "\"Attacking an enemy ghost is a good way to win battles!\", \"But there’s another way too...\"", "\"You can use talk to beat ghosts by interacting with them!\"\n\"I personally recommend it!\""
                },new object[][]{SingleMethod(2)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Flashlight.\nIt’s pretty effective!",
                    "\"Please lighten up! This really hurts...\"\n\"You’re not going to actually crucify me when my HP hits 0, are you…?\"",

                },new object[][]{SingleMethod(2)}),
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[]{
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You talk with the enemy ghost.",
                    "\"Oh, hey, you’re talkin’ to me!\"\n\"More of a lover than a fighter, I presume?\"",
                    "\"The talk system works differently than the attack system.\n\"Let me explain it to you.\"\"",
                    "\"Your goal for a talk victory is to get every bulb lit on your scanner.\"\n\"Every unlit bulb represent a talk that you have to perform.\"",
                    "\"But, here’s the catch:\"\n\"not every talk choice makes a bulb light up, and your talk choices change with every talk you get right.\"","\"Observe the way that your talk choices change after this next attack of mine.\""

                }, new object[][]{SingleMethod(new int[]{1}),SingleMethod(1),SingleMethod("talk",new string[]{},new string[]{})})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "You chatted up the enemy ghost",
                    "\"Nothing much, man how about you?\"",
                    "\"As you can see, this is a classic example of a talk that doesn’t make a bulb light up.\"\n\"Doesn’t proceed combat, but a great opportunity to get to know your enemy nonetheless.\""
                },new object[][]{})
            },new int[]{1}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Requrest_Health,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You requested some health from the enemy ghost.",
                    "\"Oh, here you go.\"\n\"I can spare some health for a newbie.\"",
                    "\"As you can see, some talks can do special things like heal you.\"\n\"Later on, some may damage you, too, so be careful.\""
                },new object[][]{SingleMethod(-4)})
            },new int[]{1}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Requrest_Proceed,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You ask the enemy ghost if he can just get on with it already.",
                    "\"Ah, the impatient type.\"\n\"I see how it is.\"",
                    "\"Don’t worry. You’re almost done.\"",
                    "\"You will have the option to end combat when the enemy’s bulbs are all lit up, or when their health is at zero.\"\n\"You can end combat in a few different ways.\"",
                    "\"You can use crucifix to straight-up destroy my soul, but you’ve talked sweet with me all this time, so I don’t think that’s what you’re going for.\"",
                    "\"The other way is to use a sentimental item from your inventory.\",\"This is an item that is special to the ghost you’re fighting, using it will save their soul.\"",
                    "\"My sentimental item is the MANUAL.\"\n\"It was by the entrance to the library, did you pick it up?\"",
                    "\"To use it, go to items and select the MANUAL.\""

                },new object[][]{SingleMethod(new int[]{2}),SingleMethod(2),SingleMethod(new string[]{},new string[]{})})
            },new int[]{1})
        };
    }
}
public class RepressedGhost : EnemyClass
{//example of an actual enemy

public enum moods{
    happy,
    happyTears,
    sad,
    mad
}
    public RepressedGhost(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<string>{"chat"};
        name = "The Ghost Of Repressed Emotions";
        hp = 15;
        maxHP = 15;
        id = 2;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder_Reverse",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/Mix_Easy"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat, TalkEnum.ChatTwo} 
        };
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
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the theramin...",
                    "The ghost looks indifferent",
                    "\"Are you picking on me?\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker...",
                    "The ghost is unphased",
                    "\"I couldn't feel that... I haven't been able to feel for a while...\"",
                    "\"Ouch that hurts...? is that want you want me to say?\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the flashlight...",
                    "The ghost has a painful expression on his face.",
                    "\"Please don't do that...\""
                },new object[][]{SingleMethod((object)4)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Garlic...",
                    "The ghost didn't like that too much.",
                    "\"Eww...\""
                },new object[][]{SingleMethod((object)4)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You started talking with the ghost...",
                    "\"I just wish things had been different, you know?\""
                },new object[][]{SingleMethod((object)6)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.ChatTwo,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You started talking with the ghost about their life...",
                    "\"Well, it's just...\"",
                    "\"They wanted different things from what I wanted...\""
                },new object[][]{SingleMethod((object)5)})
            }),
        };  
    }

}

public class BoredGhost : EnemyClass{
    public BoredGhost(battleBehavior battle = null) : base(battle){
        sentiment = new List<string>{"chat"};
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
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat} 
        };
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
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "The ghost... liked it?",
                    "\"That's nice...\"",
                    "\"Not really my genre though.\"",
                    "\"I'm more of a 'Boos' kind of guy.\""
                },new object[][]{SingleMethod((object)2)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the flashlight...",
                    "It was especially effective!",
                    "\"Ow, who turned on the lights?\""
                },new object[][]{SingleMethod((object)7)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You started talking with the ghost...",
                    "\"Sigh... Alright...\""
                },new object[][]{SingleMethod((object)4)})
            }),
        };  
    }
}