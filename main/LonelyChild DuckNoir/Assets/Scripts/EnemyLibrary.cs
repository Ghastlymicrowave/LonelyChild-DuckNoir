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
    public SpecialText[] specialTexts;
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

    protected EnemyResponse GenResponse(ButtonEnum attack, int actionID, EnemyReaction[] reactions){
        return new EnemyResponse(new EnemyActionCase((int)attack,actionID), 
        reactions
        );
    }

    protected EnemyReaction NewReaction(string methodName, string[] text, object[] parameters){
        return new EnemyReaction(
            typeof(battleBehavior).GetMethod(methodName), 
            parameters,
            text,
            thisBehavior);
    }
}

public class EnemyResponse{//a response containing a trigger and a reaction
    public EnemyActionCase trigger;
    public EnemyReaction[] reactions;//to be invoked
    public EnemyReaction RandReact(){
        return reactions[Random.Range(0,reactions.Length)];
    }
    public EnemyResponse(EnemyActionCase triggerAction, EnemyReaction[] triggerReactions){
        trigger = triggerAction;
        reactions = triggerReactions;
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
public class Narcissist : EnemyClass

{//example of an actual enemy
    public Narcissist(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Call_Him_Bald)};
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
        talkActions = new TalkEnum[2] { TalkEnum.Chat, TalkEnum.Call_Him_Bald };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/NarcissistDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Hourglass);

        sentimentalSuccess = new string[]{
            "You held out the hourglass...\nIt felt... right.",
            "\"Wait... hmm...\"\n\"...yes.\"",
            "\"Yes, I-I remember now.\"\n\"Time.\"",
            "\"Time is all we have, and here I am wasting mine.\"",
            "\"Look at you dragging out the philosophy!\"\n\"I might have been wrong about you!\"",
            "\"...\"",
            "\"All I think about is myself...\"",
            "\"If you can change...\"\n\"...can the likes of me can change, too?\"",
            "..."
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the hourglass...",
            "Does this hourglass mean something to it?",
            "\"I feel...\"\n\"...A sudden surge of pretention?\"",
            "\"No! Only I can be so full of myself!\"\n\"This crooked-nosed knave couldn't rival my superiority!\"",
            "It snaps out of it's trance, was there something you needed to do first?"
        };

        responses = new EnemyResponse[]{
            
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the theremin...",
                    "\"Ah, yes!\"\n\"This reminds me of the soothing sounds of Lydia Kavina!\"\n\"Of course you don't know who that is.\"",
                    "\"Have I told you I am very knowledgeable on a wide range of topics?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"I am reminded of a quote from the Bard himself...though I won’t waste such an intelligent line on deaf ears.\""
                },SingleMethod((object)2))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the flashlight...",
                    "\"Ah! You fool!\"\n\"This only highlights my strong chin and high cheekbones!\"",
                    "(nevermind the lack of hair on my head.)\n(I swear to god, if you so much as mention it...)",
                    "\"I rather enjoy the spotlight, you know.\""
                },SingleMethod((object)2))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the Garlic...",
                    "The ghost hates it!",
                    "\"Who taught you how to behave?\""
                },SingleMethod((object)4))
            }),
            GenResponse(ButtonEnum.Attack,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                     "You started talking with the ghost...",
                    "\"You there, young boy.\"\n\"Are there any other orphans about?\"",
                    "\"I want someone whom I can mold in my own image, but berate significantly enough that I shall feel confident they will never surpass me.\"",
                    "\"Oh, and not you.\""
                },SingleMethod((object)2))
            }),
            GenResponse(ButtonEnum.Attack,(int)TalkEnum.Call_Him_Bald,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You call the ghost bald, even going so far as to attempt to spit-shine his head.",
                    "\"WHAA???\"\n\"You think you can just walk up and do that to the likes of me, you hedge-borne little man!?\"",
                    "\"How crude!\"\n\"How prudent!\"\n\"How...\"",
                    "\"...familiar?\""
                },SingleMethod((object)4))
            })
        };
    }
}
public class TroubledChild : EnemyClass
{//example of an actual enemy
    public TroubledChild(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "A Troubled Child";
        hp = 30;
        maxHP = 30;
        id = 6;
        canRun = false;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_3",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy2",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Harder_Reverse",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy"};
        talkActions = new TalkEnum[1] { TalkEnum.Chat};
        
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
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the theremin...",
                    "\"They used to play music...\"\n\"Down there...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Please no!\"", "\"Not like...\" \n\"...they did.\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the flashlight...",
                    "\"It burns...\"\n\"It burns...\"",
                    "\"It burns like when they...\"\n\"No!\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the Garlic...",
                    "\"Why'd they only feed you?\"\n\"What did I do?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You started talking with the ghost...",
                    "\"Are you...good?\"\n\"You're not here to hurt me?\"",
                    "\"Do you want to stop them?\"",
                    "\"He stays below... in the basement!\""
                },SingleMethod((object)3))
            }),
        };
    }
}
public class NiceDemonGuy : EnemyClass
{//example of an actual enemy
    public NiceDemonGuy(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
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
        talkActions = new TalkEnum[1] { TalkEnum.Chat};
        
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
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the theremin...",
                    "\"Say, you're good with that thing!\"\n\"Down there...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Listen, kid... I'm a demon for cryin' out loud.\"", "\"A fire poker just ain't gonna do the trick, sorry to say.\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the flashlight...",
                    "\"What're you shining that in my face for?\"",
                    "\"I'm like a demon guy or whatever.\"\n\"You couldn't possibly expect me to have a meaningful reaction to a flashlight, could ya?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the Garlic...",
                    "\"I'm a demon, not a vampire...\"\n\"...but I'm flattered.\"",
                    "\"Guess I could take some extra damage for the unintended compliment.\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You started talking with the ghost...",
                    "\"Oh, man, no one's talked to me like that in eons!\"\n(Maybe the whole floating demon head thing puts people off???)",
                    "\"Even other demons stay clear!\"\n\"I don't even know why, because...\"",
                    "\"ah, look, I'm rambling...\""
                },SingleMethod((object)3))
            }),
        };  
    }
}
public class GremlinOfDeceit : EnemyClass
{//example of an actual enemy
    public GremlinOfDeceit(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "The Gremlin Of Deceit";
        hp = 42;
        maxHP = 42;
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
        talkActions = new TalkEnum[1] { TalkEnum.Chat};
        
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
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the theremin...",
                    "\"BAH!\"\n\"The ungainly sounds of that mid century hogwash!\"",
                    "*ahem*\n\"That is, I mean...\"",
                    "\"I think you sound lovely, dear...\"\n\"Come closer, I can teach you a thing or two about music...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Oooo, that burns so sweet...\"\n\"But I know just the right spots!\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the flashlight...",
                    "\"Ah, you little runt!\"\n\"I OUGHTA-\"",
                    "*ahem*",
                    "\"Why don't ya just give that little flashlight here, my sweet?\"\n\"Gremlin'll show you how to really use it.\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the Garlic...",
                    "\"What exactly did you think that would do?\"",
                    "\"Do you really think your superstitious tricks will help you?\"\n\"Look around you. Look where that brought your friends.\"",
                    "\"Besides...\"\n\"You know what happens when you sneak food...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You started talking with the ghost...",
                    "\"Do you remember me?\"\n\"From nightmares long ago?\"",
                    "\"Children draw from my form, you know!\"\n(Oh, my rhyming schemes!)",
                    "\"Am I responsible for reality splitting at the seams?\"\n\"Perhaps...\""
                },SingleMethod((object)3))
            }),
        };  
    }
}
public class DevilsHands : EnemyClass
{//example of an actual enemy
    public DevilsHands(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
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
        talkActions = new TalkEnum[1] { TalkEnum.Chat};
        
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
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the theremin...",
                    "\"I've always thought of taking up music.\"",
                    "\"They would never let me.\"\n\"Too much noise\"",
                    "\"The devil plays music.\"\n\"Young ones like me should practice more... wholesome passtimes.\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Oh, that familiar sting!\"\n\"What's wrong with you?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the flashlight...",
                    "\"Are you trying to shine a light on the subject?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the Garlic...",
                    "\"Where'd you get that?\"\n\"Do you have any idea what they'll do if they catch you sneaking food?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You started talking with the ghost...",
                    "\"When I was a young thing, I could never keep my hands still.\"\n\"Always fidgeting... fidget and fidget.\"",
                    "\"I had to be punished.\"",
                    "\"No good boy fidgets like that...\"\n\"...no good deeds can come from that...\""
                },SingleMethod((object)3))
            }),
        };  
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
            "Prefabs/combatEnemyTurn/attacks/Turret1"};
        //"Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
        //"Prefabs/combatEnemyTurn/attacks/SineReverse_Tooeasy2",
        // "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[3] { TalkEnum.Pet, TalkEnum.Chat, TalkEnum.Fake_Throw };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/PoorDogDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Ball);

        sentimentalSuccess = new string[]{
            
            "You held out the ball...\nIt felt... right.",
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
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the theremin...",
                    "The ghost recoils at the pitch!",
                    "\"Whine.... Turn it off...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the FirePoker...",
                    "The ghost isn't loving it... but isn't hating it, either.",
                    "\"Too heavy to be stick...\nTo long to be ball...\"",
                    "\":(\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the flashlight...",
                    "The ghost starts darting after the light, thinking it's a ball!",
                    "You go on for a few minutes, making the ghost run in circles.",
                    "This isn't exactly effective...",
                    "\"Where did flat ball go?????\"",
                    "\"Do you have better ball!?!?!?!?!?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the Garlic...",
                    "The ghost hates it!",
                    "\"Smelly ball bad for me!!! Give better ball >:(\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You started talking with the ghost...",
                    "You might've briefly mentioned something that sounds vaguely like the word 'ball'.",
                    "\"Ball!?!?!?!?!?\"",
                    "\"...\"",
                    "\"Ohh... False Ball-arm...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Fake_Throw,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You made a throwing motion with your arm...",
                    "But there was nothing in your hand?",
                    "\"How could you!!!???? >:( >:( >:(\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Pet,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You tried to pet the ghost...",
                    "But your arm phased right through 'em, so...",
                    "You just kinda made a petting motion with your arm.",
                    "Between you and me, I don't think he knows the difference.",
                    "\"Woof!~ :)\""
                },SingleMethod((object)3))
            }),
        };  
    }
}
public class Tutorial : EnemyClass
{//example of an actual enemy
    public Tutorial(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "Ghost Hunting Nerd";
        hp = 6;
        maxHP = 6;
        id = 4;
        canRun = false;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Tooeasy2",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[1] {TalkEnum.Chat};
        
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
            "The ghost hesitates and looks at the manual...",
            "Does this manual mean something to it?",
            "\"Ooh, this is my favorite part! Let me help you out!\"",
            "\"You're supposed to show me the thing AFTER you beat me up and talk to me!\"",
            "\"It's the cool way to end battles!\"",
            "It snaps out of it's trance, was there something you needed to do first?"
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
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the theremin...",
                    "The ghost is relatively unfazed!",
                    "\"Ooh, buddy, I got a resistance to this attack.\"",
                    "\"You've gotta try the other attack!\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the FirePoker...",
                    "The ghost isn't loving it... but isn't hating it, either.",
                    "\"Buddy...\"\n\"You're not supposed to have that yet...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the flashlight...",
                    "\"Oh god! My eyes! It burns!\"",
                    "\"Ha. Just kidding.\" \"I don't have eyes.\"",
                    "\"You're gonna wanna keep the attacks coming until all the bulbs are lit on your scanner!\" \n\"When that happens, talk to me!\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the Garlic...",
                    "The ghost hates it!",
                    "\"How'd you even get that?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Ruler,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the Ruler...",
                    "\"...\"",
                    "\"What's wrong with you?'\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You started talking with the ghost...",
                    "\"Hey! You're talking to me!\"",
                    "\"Have you been reading the manual?\"",
                    "\"You can talk to ghosts around here to figure out what their issue is!\"",
                    "\"I don't have any issues, I'm just a tutorial, but some of these guys are absolute class acts...\"",
                    "\"...They won't stop fighting unless you really level with them.\"",
                    "\"After doing so, you can use a certain item with sentimental value to save them, or a crucifix to destroy them.\"",
                    "\"My item was right in front of me, it was a manual.\" \n\"Go ahead and use it on me!\""
                },SingleMethod((object)3))
            })
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
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the theramin...",
                    "The ghost looks indifferent",
                    "\"Are you picking on me?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the FirePoker...",
                    "The ghost is unphased",
                    "\"I couldn't feel that... I haven't been able to feel for a while...\"",
                    "\"Ouch that hurts...? is that want you want me to say?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the flashlight...",
                    "The ghost has a painful expression on his face.",
                    "\"Please don't do that...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the Garlic...",
                    "The ghost didn't like that too much.",
                    "\"Eww...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You started talking with the ghost...",
                    "\"I just wish things had been different, you know?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.ChatTwo,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You started talking with the ghost about their life...",
                    "\"Well, it's just...\"",
                    "\"They wanted different things from what I wanted...\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Fake_Throw,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You made a throwing motion with your arm...",
                    "But there was nothing in your hand?",
                    "\"How could you!!!???? >:( >:( >:(\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Pet,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You tried to pet the ghost...",
                    "But your arm phased right through 'em, so...",
                    "You just kinda made a petting motion with your arm.",
                    "Between you and me, I don't think he knows the difference.",
                    "\"Woof!~ :)\""
                },SingleMethod((object)3))
            }),
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
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "The ghost... liked it?",
                    "\"That's nice...\"",
                    "\"Not really my genre though.\"",
                    "\"I'm more of a 'Boos' kind of guy.\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You attacked with the flashlight...",
                    "It was especially effective!",
                    "\"Ow, who turned on the lights?\""
                },SingleMethod((object)3))
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction("DamageEnemy",
                new string[]{
                    "You started talking with the ghost...",
                    "\"Sigh... Alright...\""
                },SingleMethod((object)3))
            }),
        };  
    }
}