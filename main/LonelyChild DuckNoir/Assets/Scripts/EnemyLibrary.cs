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
    public bool playerHurt = false;
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
    public bool triggerIsReaction = false;
    public Sprite[,] GetSprites()
    {
        Sprite[,] toReturn = new Sprite[3, animationFrames];
        
        for (int i = 1; i < animationFrames+1; i++)
        {
            string istring = i.ToString();
            string loadstring = folderPath;
            if (istring.Length<2){istring = "0"+istring;}
            toReturn[0, i-1] = Resources.Load<Sprite>(loadstring + "Front" + istring);
            toReturn[1, i-1] = Resources.Load<Sprite>(loadstring + "Back" + istring);
            toReturn[2, i-1] = Resources.Load<Sprite>(loadstring + "Side" + istring);
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
        splashTexts = new string[]{
            "The Narcissist looks down on you and mutters something about \"wanting to find someone who's just as arrogant as himself.\"",
            "The Narcissist hovers with what looks like chip crumbs in his beard.",
            "The Narcissist mutters something about wasted time.",
            "The Narcissist grumbles about his receding hair.",
            "It smells pretentious in here.",
            "Is that... fog?\nDid he bring a fog machine in here?",
            "The Narcissist sets up a spotlight to light him up from behind...\nHe looks marvelous!"
        };
        name = "The Narcissist";
        hp = 20;
        maxHP = 20;
        id = 5;
        canRun = false;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Spin_Narcissist_1",
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_2",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Narcissist_1",
            "Prefabs/combatEnemyTurn/attacks/Spin_Narcissist_2"};

        //  "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy1",
        //  "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat, TalkEnum.Call_Him_Bald} ,
            new TalkEnum[]{TalkEnum.Gloat, TalkEnum.Compliment, TalkEnum.Insult} ,
            new TalkEnum[]{TalkEnum.Lecture},
            new TalkEnum[]{TalkEnum.ChatTwo} 

        };
        sentiment = new List<string>{"call_him_bald","gloat","lecture"};
        specialVals = new int[]{0,0};
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/NarcissistDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Hourglass);

        sentimentalSuccess = new string[]{
            "You present the hourglass.",
            "\"My goodness, that???s it!.\", \"My lucky hourglass!\"",
            "\"Without it, I felt like the slightest inconvenience took a lifetime.\"\n\"But, now, I feel??? complete?\"",
            "\"Time is all we have, but I???ve been wasting so much...\"\n\"My search for self-centeredness, ironically, was self-centered of me...\"",
            "\"Thank you.\""
        };
        sentimentalFaliure = new string[]{
            "You present the Hourglass.\nThe ghost isn???t quite ready for it yet.",
            "\"I smell a faint pretention in the air...\"\n\"...could it be?\"",
            "\"No??? It couldn???t be.\""
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Theremin!",
                    "\"Hey, you're playing pretty well!\"\n\"This reminds me of the works of Sir Cotus The Shrew!\"",
                    "\"Huh!? You haven't heard of him!?\"",
                    "\"Uh... Well... Of course you wouldn't have!\"\n\"You're not refined enough!\"",
                    "The ghost hides his embarrassment, taking heavy damage in the process."
                },new object[][]{SingleMethod(3)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Theremin!\nThe ghost seems to be in a lot of pain...",
                    "\"Have you even played that thing before!?\"\n\"Turn it off, please!\""
                },new object[][]{SingleMethod(4)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"The fire poker isn't on this floor! I don't have TIME for your CHEATING!\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Flashlight...",
                    "\"Ha, you really thought you did something there.\"\n\"You fool...\"",
                    "\"You're just brightening up all my best features!~\"\n\"I hope you've got something better up your sleeve!\""
                },new object[][]{SingleMethod((object)1)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Flashlight...",
                    "\"Oooh, are you trying to get a good look at my glorious self?\"",
                    "\"Let me strike a pose for you!\"\n\"How's this one?\"\n\"How about this!?\"",
                    "\"Now, stop wasting my TIME.\""
                },new object[][]{SingleMethod((object)2)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Garlic...",
                    "\"The garlic isn't on this floor.\"\n\"I don't have TIME for children who CHEAT!\""
                },new object[][]{SingleMethod((object)2)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.ChatTwo,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "\"Do you have something you wanted to say...?\"",
                    "\"I don't have all day you know, time is running out... or something like that.\""
                },new object[][]{})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You chat up the enemy ghost.\nHe slaps you across the face!",
                    "\"You seriously think I have the TIME for idle chatter?\"",
                    "\"How about I adopt some other orphan boy?\"\n\"Preferably someone I can berate into my OWN image.\""

                },new object[][]{SingleMethod(3)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Call_Him_Bald,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You insult the ghost???s hairless scalp, going so far as to use it as a mirror to admire yourself!.",
                    "\"WHAT???\"\n\"You dare insult the likes of me!?\"",
                    "\"How CRUDE!\"\n \"How PRUDENT!\"\n \"How...\"",
                    "\"...exactly like me?\""

                },new object[][]{SingleMethod(new int[]{1}),SingleMethod(1),SingleMethod("call_him_bald",new string[]{},new string[]{})})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Compliment,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You admire the ghost???s figure...\nHe slaps you across the face!",
                    "\"You think I don???t know how GLORIOUS and UNFLAPPABLE I am!?\"",
                    "\"I???d like to adopt someone who respects my intellect a little bit more.\"\n\"I don???t have TIME to hear what I already know...\""

                },new object[][]{SingleMethod(3)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Gloat,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You gloat, going on about your appearance and your accomplishments.",
                    "If you actually did even one of the things you said you did, you???d surely earn a Nobel prize...",
                    "\"YES! Finally!\" Another piece of the puzzle!\"",
                    "\"I???ve rubbed off on you already!\"\n\"The attitude AND the ego!\"",
                    "\"But??? I think something???s missing yet...\""

                },new object[][]{SingleMethod(new int[]{2}),SingleMethod(2),SingleMethod("gloat",new string[]{},new string[]{})})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Insult,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "You insult the narcissist once more.\nThis time, going after his flabby body.",
                    "\"Hey, I appreciate the moxie, kid, but are insults really the only thing you???ve got?\"",
                    "\"I don???t have TIME for a one-note kid.\"\n\"You???re gonna have to try harder to impress me.\""

                },new object[][]{})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Lecture,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You go on for as long as you can muster, lecturing the ghost on every minute thing you can think of.",
                    "His shirt is SO last century???\nHis shoes? Look! They???re untied!",
                    "Your inflated ego is so massive, it could break the very floor you???re standing on.",
                    "\"How??? perfect!",
                    "\"I???ve rubbed off on you... \"\n\"In only a matter of minutes!\"",
                    "\"I???\"\n\"...I simply MUST adopt you!\"",
                    "\"But??? we can???t go home, not yet!\"\n\"I???m missing something important to me...\"",
                    "\"What was it...\"\n\"Do you happen to have it?\""
                },new object[][]{SingleMethod(new int[]{3}),SingleMethod(3),SingleMethod("lecture",new string[]{},new string[]{})})
            }),
        };
    }
}
public class TroubledChild : EnemyClass
{//example of an actual enemy
    public TroubledChild(battleBehavior battle = null) : base(battle)
    {
        
        name = "A Troubled Child";
        hp = 25;
        maxHP = 25;
        id = 6;
        canRun = false;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Spin_TC_1",
            "Prefabs/combatEnemyTurn/attacks/Sine_TC_1",
            "Prefabs/combatEnemyTurn/attacks/Sine_TC_2",
            "Prefabs/combatEnemyTurn/attacks/TurretTC"
           // "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
           // "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_3",
           // "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy",
            };
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Talk},
            new TalkEnum[]{TalkEnum.Chat,TalkEnum.Console,TalkEnum.Encourage} 
        };
        
        splashTexts = new string[]{
            "The ghost says, \"It's so dark down here, I can't even see who you are...\"",
            "The ghost says, \"Do you have a light, I get scared of the dark...\"",
            "The ghost says, \"Have you seen Roosevelt? I really miss him.\"",
            "The ghost says, \"Have you seen my ted- er- my Roosevelt?\"",
            "The ghost says, \"It's so lonely down here... and dark...\"",
            "The darkness closes in...",
            "The eye on the wall is dilated from the darkness of the room...",
            "He smells like burnt coals.",
            "O feels uncomfortable."
        };

        specialVals = new int[] {0,0};
        triggerIsReaction = true;
        sentiment = new List<string>{"console","chat","light"};
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/TroubledChildDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Teddy_Bear);

        sentimentalSuccess = new string[]{
            "\"Thanks so much for the talk, mister!\"\n\"It was really nice seeing someone around...\"",
            "\"It makes me feel like this place has a bit of hope!\"",
            "\"Like I...\"\n\"...have a bit of hope.\"",
            "\"I guess it's time to move on.\"",
            "\"Bye bye, mister!\""
        };
        sentimentalFaliure = new string[]{          
        };

        responses = new EnemyResponse[]{
             GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Theremin!",
                    "\"What is that sound?\""
                },new object[][]{SingleMethod(1)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Theremin!",
                    "\"Well, it beats what I usually hear...\""
                },new object[][]{SingleMethod(2)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker!",
                    "\"Oh, why!?\"\n\"Are you... him!?\""
                },new object[][]{SingleMethod((object)5)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker!\nThe ghost really hated that...",
                    "\"Get away from me!\""
                },new object[][]{SingleMethod(4)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker!\nThe ghost seems terrified...",
                    "\"AAGH!\"\n\"Not this again!\""
                },new object[][]{SingleMethod(4)})
            }),
            
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Garlic!\nThe ghost didn't care for it...",
                    "\"Ew...\"\n\"...garlic...\""
                },new object[][]{SingleMethod(3)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Garlic!",
                    "\"Where'd you even get that?\""
                },new object[][]{SingleMethod(2)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "\"No way, nuh-uh, not talking to you.\"",
                    "\"Well, I might talk to you if you knew where Roosevelt was.\""
                },new object[][]{})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "\"I???m not saying a word until you let me see my teddy again.\""
                },new object[][]{})
            },new int[]{1}),
            GenResponse(ButtonEnum.Items,(int)ItemsEnum.Teddy_Bear,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks"},
                new string[]{
                    "\"Yay! My teddy! Ok, I???ll talk to you, you must be nicer than those other guys.\"",
                },new object[][]{SingleMethod(new int[]{1}),SingleMethod(1)})
            },new int[]{0}),
            GenResponse(ButtonEnum.Items,(int)ItemsEnum.Teddy_Bear,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks"},
                new string[]{
                    "\"Thank you! I haven???t seen my bear in such a long time. It looks like he got all scratched up.\"",
                },new object[][]{SingleMethod(new int[]{1}),SingleMethod(1)})
            },new int[]{1}),
            //////////////////////////////////////////////////////////////////////////////////
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialRel"},
                new string[]{
                    "You shine the Flashlight at the ghost.",
"\"Hey, that???s actually really nice! Please shine some more light, it???s dark in here!\""
                },new object[][]{SingleMethod(new int[]{-1,1})})
            },new int[]{-1,0}),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem"},
                new string[]{
                    "You shine the Flashlight around the room."
                },new object[][]{SingleMethod(
                    "light",
                    new string[]{"\"Wow, you???re really nice, thanks for lighting up the room!\""},
                    new string[]{"\"Thanks, but I can see well enough now, you don???t need to keep shining that light everywhere.\""}
                )})
            },new int[]{-1,1}),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialRel"},
                new string[]{
                    "You shine the Flashlight around the room.",
"\"Thanks a bunch, i???ve been in the dark for so long- it???s nice to see some light!\""
                },new object[][]{SingleMethod(new int[]{-1,1})}),
                NewReaction(new string[] {"ChangeSpecialRel"},
                new string[]{
                    "You shine the Flashlight around the room.",
"\"Wow, you???re the kindest guy i???ve ever met! I like being able to see things!\""
                },new object[][]{SingleMethod(new int[]{-1,1})})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Console,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks","SentimentalItem"},
                new string[]{
                    "You try to comfort the ghost."
                },new object[][]{SingleMethod(0),SingleMethod(
                    "console",
                    new string[]{"\"That???s really sweet of you, it???s so nice talking to another person again- you???re nothing like the people in coats.\""},
                    new string[]{"Thanks, I feel better now... Can I see my bear again?"}
                )})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks","SentimentalItem"},
                new string[]{
                    "You try to start some small-talk with the ghost."
                },new object[][]{SingleMethod(0),SingleMethod(
                    "chat",
                    new string[]{ "\"It???s dark and quiet down here. You get used to the darkness somewhat, but never the sounds...\"",
                    "\"The walls howl sometimes.\"\n\"It gets really freaky.\"",
                    "\"Is there anything you could do about the dark?\""},
                    new string[]{"\"I know how it sounds, but the people in the basement do terrible things...\"",
                    "\"The results of the things they???ve done are still down there...\"",
                    "\"Before you go, would you let me hold my teddy, pretty please?\"\n\"And is there something you could do about the dark?\""}
                )})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Encourage,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks"},
                new string[]{
                    "You try to raise the ghost???s spirits.",
            "\"What do you mean?\"\n\"I???m not afraid, just kinda lonely???\""
                },new object[][]{SingleMethod(0)}),
                NewReaction(new string[] {"ChangeTalks"},
                new string[]{
                    "You try to raise the ghost???s spirits.",
            "\"Don't look at me like that!\"\n\"I'm lonely, not a wuss!\""
                },new object[][]{SingleMethod(0)})
            }),
        };
    }
}
public class NiceDemonGuy : EnemyClass
{//example of an actual enemy
    public NiceDemonGuy(battleBehavior battle = null) : base(battle)
    {
        splashTexts = new string[]{
            "The demon says, \"Long ago in old country, I played with little toy...\".",
            "The demon says, \"Unlike other demon men, I am approachable, comrade.\"",
            "The demon says, \"Sweet babushka...\".",
            "The thick scent of borscht fills the room."
        };
        sentiment = new List<string>{"talk","eternity","cycles","compliment"};
        name = "Nice Demon Guy";
        hp = 15;
        maxHP = 15;
        id = 7;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_NDG",
            "Prefabs/combatEnemyTurn/attacks/Sine_NDG",
            "Prefabs/combatEnemyTurn/attacks/Sine_NDG_2"
          /*  "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_3",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder"*/};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Talk},
            new TalkEnum[]{TalkEnum.Chat,TalkEnum.Encourage,TalkEnum.Eternity}, 
            new TalkEnum[]{TalkEnum.Hope,TalkEnum.Compliment},
            new TalkEnum[]{TalkEnum.Cycles,TalkEnum.Ask},
            new TalkEnum[]{TalkEnum.Check_in}
        };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/NiceDemonGuyDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Russian_Doll);

        sentimentalSuccess = new string[]{
            "Long time since...playing with little doll",
            "As demon man, It is unbecoming to play with dolls, comrade...",
            "But sweet little babushka doll give demon man much happy time...",
            "Perhaps happiness remains possibility for this demon man...",
            "I see bright light, and ghost of Lenin becons...",
            "Farewell young orphan comrade, sieze the means of adoption!",
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the Russian Doll...",
            "Does this Russian Doll mean something to it?",
            "\"Young comrade child-boy, what was item you held?\""
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Theremin...",
                    "\"Sweet music of old country!",
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Look here silly child, I am demon-man.\"", "\"Fire poker has little effect on me.\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Flashlight...",
                    "\"Why shine bright light in face?\"",
                    "\"I am demon man or something like that.\"\n\"What would flashlight do to stop me?\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Garlic,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Garlic...",
                    "\"I am demon, not vampire...\"\n\"And I am Russian.\"",
                    "\"But, young comrade, I like you.  I take damage for you.\""
                },new object[][]{SingleMethod((object)2)})
            }),
            ////////////////Talk Actions
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem","ChangeTalks"},
                new string[]{
                    "\"Yello young orphan child-boy.  Kind of you to talk to demon-man such as I.",
                "But I am demon-man, and I do demon-man type thing.", 
                    "\"Haunting, posession, totalitarian spying on satellite state.",
                "But I like you, so I tell you what you need know, if you keep me engaged!\""
                },new object[][]{SingleMethod("talk",new string[]{},new string[]{}),SingleMethod(1)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "Cannot complain.",
                    "Being ageless demon-man I feel vigorous!"
                },new object[][]{})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Encourage,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "\"Oh well, I mean, I try my best!  It???s hard to get proper recognition when you???re a demon guy.",
                    "\"Seems like all the people that are prone to compliments are scared of me... and the guys in my corner, well...",
                    "let???s just say they aren???t too keen on sharing their feelings!\""
                },new object[][]{})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Eternity,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem","ChangeTalks"},
                new string[]{
                    "You asked the nice demon guy what eternity feels like." , 
                },new object[][]{SingleMethod("eternity",new string[]{
                    "\"I was not expecting such conversation from small child-boy as yourself.  It feels like mother Russia, Endless!",
                    "As if all things are one.  Am I making of the sense?"
                },new string[]{
                    "\"Were you going somewhere with this? Well get on with it.\""
                }),SingleMethod(2)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Hope,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "\"Hope?  Hope is not something demon-man can even consider...sorry to bring mood down, young orphan...\""
                },new object[][]{SingleMethod(2)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Compliment,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem","ChangeTalks"},
                new string[]{
                    "You compliment the nice demon guy on his manners.", 
                },new object[][]{SingleMethod("compliment",
                new string[]{"\"I appreciate compliment...you are good child-boy...please do not go into basement...\""},
                new string[]{"\"You are flattering me, no?\""}),SingleMethod(3)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Cycles,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem","ChangeTalks"},
                new string[]{
                    "You ask the nice demon about the cyclical nature of existence.  ",
                    "Everything is cycle.",
                    "Friends come and go, just like leaves on tree...",
                    "...",
                    "Now demon-man is sad."
                },new object[][]{SingleMethod("cycles",
                new string[]{},
                new string[]{}),SingleMethod(4)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Ask,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "You ask the nice demon guy if he has been doing okay lately.",
                    "\"I am demon man who is doing bidding of evil, how you think?\""
                },new object[][]{})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Check_in,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "The nice demon guy seems to be lost in thought.",
                    "\"...okay, am I okay? Was I ok? Will I be ok?... circle circle circle ouroboros stacks... where did doll go?\""
                },new object[][]{})
            }),
        };  
    }
}
public class GremlinOfDeceit : EnemyClass
{//example of an actual enemy
    public GremlinOfDeceit(battleBehavior battle = null) : base(battle)
    {
        sentiment = new List<string>{"talk","chat","compliment","ask"};
        name = "The Gremlin Of Deceit";
        hp = 30;
        maxHP = 30;
        id = 9;

        canRun = false;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Spin_GOD",
            "Prefabs/combatEnemyTurn/attacks/Spin_GOD",
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_GOD",
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_GOD",
            "Prefabs/combatEnemyTurn/attacks/GOD WRATH"
          
           
          /*  "Prefabs/combatEnemyTurn/attacks/Sine_Harder_Reverse",
            "Prefabs/combatEnemyTurn/attacks/Sine_Harder",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
            "Prefabs/combatEnemyTurn/attacks/Mix_Easy",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
            
            "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy"*/};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Talk}, 
            new TalkEnum[]{TalkEnum.Chat,TalkEnum.Encourage,TalkEnum.Eternity},
            new TalkEnum[]{TalkEnum.Time,TalkEnum.Compliment},
            new TalkEnum[]{TalkEnum.Cycles,TalkEnum.Ask}
        };
        
        splashTexts= new string[]{
            "\"I've always been here, you can't remove me, can't erase- it behooves me.\"",
            "\"Struggle all you're willing to try, you can't swat this fly!\"",
            "The apparition cackles non-stop.",
            "The apparition babbles something about drawing on and erasing the fabric of the universe.",
            "O feels uncomfortable."
        };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/GremlinOfDeceitDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Eraser);

        sentimentalSuccess = new string[]{
            "\"Oh ye gods!\"\n\"My power is evaporated by one fell blow?\"",
            "\"No!\"\n\"NO!!!\"",
            "...",
            "\"Very well, if you must know???\"",
            "\"There is someone below even me...\"",
            "\"I did not bring these spectres here...\"\n\"I merely... should I say...\"",
            "\"...adopted them?\"",
            "\"You will never truly win.\"",
            "\"You should find a way to escape while you still can.\"",
            "...",
            "But...Your little orphan friends",
            "I shall reunite you with them..."

        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the Eraser...",
            "Does this Eraser mean something to it?",
            "\"You dare!?\"\n\"Haven't you learned not to challenge my authority!\""
            
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Theremin...",
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
                    "\"Oooo, that burns!\"",
                    "(and I liked it)",
                },new object[][]{SingleMethod((object)3)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Flashlight...",
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
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[]{
                NewReaction(new string[]{"ChangeTalks","SentimentalItem"},
                new string[]{
                    "\"Come closer, little one, and I???ll give you a sweet treat!  I promise I am nice!  I won???t even bite!",
                    "I???ve got something over here you might like???\""
                },new object[][]{SingleMethod(1),SingleMethod(
                    "talk",
                    new string[]{},
                    new string[]{}
                )})
                }
            ),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[]{
                NewReaction(new string[]{"ChangeTalks","SentimentalItem"},
                new string[]{
                },new object[][]{SingleMethod(2),SingleMethod(
                    "chat",
                    new string[]{"\"Do you remember me, little O?",
                    "From nightmares long ago? Children draw my form, you know!",
                        "\"Haha, oh rhyming schemes! Am I responsible for reality splitting at the seams?",
                        "Perhaps...\""},
                    new string[]{"\"You won't get anything saying that again, go and find a better friend.\""}
                )})
                }
            ),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Encourage,
            new EnemyReaction[]{
                NewReaction(new string[]{},
                new string[]{
                    "\"What?!\"\n\"What would a child like you want encouraging a deceitful creature like me...?\""
                },new object[][]{})
                }
            ),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Eternity,
            new EnemyReaction[]{
                NewReaction(new string[]{},
                new string[]{
                    "You ask the gremlin about his thoughts on eternity.",
                    "\"Oh, I???m eternity grateful for everything you see.\"\n\"Why not try and compliment me?"
                },new object[][]{})
                }
            ),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Time,
            new EnemyReaction[]{
                NewReaction(new string[]{"ChangeTalks"},
                new string[]{
                    "\"Tick tock goes the ever clicking clock???\"\n\"sometimes forward... ...sometimes not!\""
                },new object[][]{SingleMethod(1)})
                }
            ),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Compliment,
            new EnemyReaction[]{
                NewReaction(new string[]{"ChangeTalks","SentimentalItem"},
                new string[]{
                    "You compliment the Gremlin on his fine smile."
                },new object[][]{SingleMethod(3),SingleMethod(
                    "compliment",
                    new string[]{
                    "\"Oh, compliments are good for me!\" \"I eat them up as you can see.\""},
                    new string[]{"\"Keep the compliments rolling, don't stop! The longer you wait the more you will rot!\""})})
                }
            ),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Cycles,
            new EnemyReaction[]{
                NewReaction(new string[]{"ChangeTalks"},
                new string[]{
                    "You talk to the gremlin about the cyclical nature of playing video games.", 
                    "\"Ah such fun! The thrill of the hunt.\" \"A game is time well spent.\""
                },new object[][]{SingleMethod(1)})
                }
            ),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Ask,
            new EnemyReaction[]{
                NewReaction(new string[]{"ChangeTalks","SentimentalItem"},
                new string[]{
                    "You ask the gremlin about his rhyming.",
                    "\"Why, there???s no rhyming here, dear boy!\"\n\"I only have the world to play with as my little toy.\""
                },new object[][]{SingleMethod(4),SingleMethod(
                    "ask",
                    new string[]{},
                    new string[]{})})
                }
            ),
        };  
    }
}
public class DevilsHands : EnemyClass
{//example of an actual enemy
    public DevilsHands(battleBehavior battle = null) : base(battle)
    {
        splashTexts = new string[]{
            "The ghost twists its hands like it wants to play with something.",
            "The ghost fidgets its hands.",
            "So many hands, so many options!",
            "The ghost twists its hands like it wants to play with something.",
            "The ghost is tapping its many fingers impatiently."
        };
        sentiment = new List<string>{"ball", "chat"};
        name = "Devil's Hands";
        hp = 12;
        maxHP = 12;
        id = 8;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/TurretDevilshands",
            "Prefabs/combatEnemyTurn/attacks/TurretDevilshands2",
            "Prefabs/combatEnemyTurn/attacks/TurretDevilshands3",
            "Prefabs/combatEnemyTurn/attacks/Straight_Devil'sHands"
           // "Prefabs/combatEnemyTurn/attacks/Straight_Wide_Easy_3",
           // "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
           // "Prefabs/combatEnemyTurn/attacks/SineReverse_Easy1",
            //"Prefabs/combatEnemyTurn/attacks/Sine_Harder",
        //"Prefabs/combatEnemyTurn/attacks/SineReverse_Easy2",
          //"Prefabs/combatEnemyTurn/attacks/SineReverse_Easy1",
         //"Prefabs/combatEnemyTurn/attacks/Straight_TooEasy",
         //"Prefabs/combatEnemyTurn/attacks/Sine_TooEasy",
          //"Prefabs/combatEnemyTurn/attacks/Sine_Harder",
           //"Prefabs/combatEnemyTurn/attacks/SineReverse_Tooeasy2"
            };
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Chat,TalkEnum.Ask,TalkEnum.Talk} 
        };
        specialVals = new int[]{0};
        
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
            "For a moment, it's able to stand still!\nIt wasn't for very long, though...",
            "Does this Spinning Toy mean something to it?"
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Theremin...",
                    "\"I've always thought of taking up music.\"",
                    "\"They would never let me.\"\n\"Too much noise\"",
                    "\"The devil plays music.\"\n\"Young ones like me should practice more... wholesome pastimes.\""
                },new object[][]{SingleMethod((object)3)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the FirePoker...",
                    "\"Oh, that old familiar sting!\"\n\"What's wrong with you?\""
                },new object[][]{SingleMethod((object)3)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Flashlight...",
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
            ///////// Talks
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You started talking with the ghost...",
                    "It ignores you and starts swinging its hands wildly!",
                    "\"When I was a young thing, I could never keep my hands still.\"\n\"Always fidgeting... fidget and fidget.\"",
                    "\"They punished me a lot... and now I look like this! Serves me right!\"",
                    "\"No good boy fidgets like that...\"\n\"...no good deeds can come from that...\""
                },new object[][]{SingleMethod((object)2)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer","SentimentalItem"},
                new string[]{
                    "You have trouble trying to talk to the ghost...\nIt seems very distracted.",
                },new object[][]{SingleMethod(1),SingleMethod("chat",
                new string[]{"\"I just need something to do with my hands, you know? I used to have a toy I'd use all the time... back when I had two legs and whatnot- you know.\""},
                new string[]{"\"Hey, you got anything I can hold? anything that snips, bounces, rolls- that kind of thing? I just can't focus...\""})})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Ask,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "You ask the ghost how it became what it is now.",
                    "Well you know, I couldn't control my hands, they just sort of- acted on their own!... Or atleast they do now.",
                    "So the folks that ran the place thought they'd put an end to it- punishment for being a disturbance I assume!",
                    "The process was terrible- zero out of ten, won't go to the basement again."
                },new object[][]{})
            }),
            GenResponse(ButtonEnum.Items,(int)ItemsEnum.Ball,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem"},
                new string[]{
                    "\"Boing Boing Boiong! Fun! How mundane and safe!\""
                },new object[][]{SingleMethod("ball",
                new string[]{"\"I just love this, what a fun toy to bounce around!\""},
                new string[]{"\"Yes this is good, but do you have anything else?\""})})
            }),
            GenResponse(ButtonEnum.Talk,(int)ItemsEnum.Scissors,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You hand the hands a pair of scissors.",
                    "\"Snip Snip Snap! Oops...\""
                },new object[][]{SingleMethod(3)})
            }),
        };  
    }
}
public class PoorDog : EnemyClass
{//example of an actual enemy
    public PoorDog(battleBehavior battle = null) : base(battle)
    {
        name = "Poor Dog";
        hp = 10;
        maxHP = 10;
        id = 1;
        //spritepath
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Turret1",
            "Prefabs/combatEnemyTurn/attacks/BoneEasier2",
            "Prefabs/combatEnemyTurn/attacks/BoneEasier",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Pet,TalkEnum.Chat,TalkEnum.Fake_Throw}, 
            new TalkEnum[]{TalkEnum.ChatTwo} 
        };
        
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/PoorDogDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Ball);

        sentiment = new List<string>{"pet"};
        specialVals = new int[]{0,0};
        sentimentalSuccess = new string[]{
            
            "You held out the ball...\nIt felt... right.",
            "\"BALL!?!?!???!?\"",
            "\"!?!BORK?!?!???!?WOOF?!!?!??!?WOOF?!?!??!?BARK!?!??!?\"",
            "...\"Boof.\""
        };
        sentimentalFaliure = new string[]{
            "The ghost hesitates and looks at the ball...",
            "Does this ball mean something to it?",
            "\"I feel like there could be ball...?\"",
            "\"But... No see ball?????\"",
            "It snaps out of its trance, was there something you needed to do first?"
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.ChatTwo,
            new EnemyReaction[]{
                NewReaction(new string[]{},
                new string[]{
                    "\"Woof Woof!\", the ghost ignores you and jumps around, expecting you to throw something..."
                }, new object[][]{})
            }
            ),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Theremin...\nThe ghost recoils at the pitch!",
                    "\"Whine??? Turn it off!..\""
                },new object[][]{SingleMethod(3)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Theremin!\nThe ghost seems to be in a lot of pain...",
                    "\"BARK BARK BARK BARK BARK! >:(\""
                },new object[][]{SingleMethod(4)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Flashlight...\nThe ghost starts darting after the light, thinking it???s a ball!.",
"You go on for a minute, making the ghost run in circles.\nThis isn???t very effective.",
"\"WHERE DID FLAT BALL GO?????\""

                },new object[][]{SingleMethod(1)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Flashlight..\nThe ghost stares intently at the flashlight???s bulb!",
"\"Bright ball...\"\n\"Give me your wisdom...\"",
"This isn???t very effective."

                },new object[][]{SingleMethod(1)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "You started chatting up the ghost???\nYou may have said something that sounds vaguely like the word ball.",
"\"BALL!??!??!!?!!?\"\n\"...\"",
"\"Oh??? False ball-arm...\""

                },new object[][]{})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Pet,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem","ChangeSpecialAbs","ChangeTalks"},
                new string[]{
                    "You tried to pet the ghost???\nBut your arm just phased right through the dog, so...",
"You just??? kinda??? made a petting motion with your arm.\nBetween you and me, I don???t think he knows the difference.",
"\"Woof!~ :)\""
                },new object[][]{SingleMethod("pet",new string[]{}, new string[]{}),SingleMethod(new int[]{1}),SingleMethod(1)})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Fake_Throw,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You made a throwing motion with your arm???\n...but there was nothing in your hand?",
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
        
        name = "Ghost Hunting Nerd";
        hp = 6;
        maxHP = 6;
        id = 4;
        canRun = false;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy_Tutorial",
            "Prefabs/combatEnemyTurn/attacks/Straight_TooEasy2",
            "Prefabs/combatEnemyTurn/attacks/SineReverse_Tooeasy2_Tutorial",
            "Prefabs/combatEnemyTurn/attacks/Sine_TooEasy_Tutorial"};
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Talk} ,
            new TalkEnum[]{TalkEnum.Chat,TalkEnum.Request_Health,TalkEnum.Request_Proceed} ,
            new TalkEnum[]{} 
        };
        specialVals = new int[]{0,0};
        sentiment = new List<string>{"talk","request"};//define setniment tags
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/TutorialDisplay";
        
        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Manual);

        sentimentalSuccess = new string[]{
            "You hold out the manual.",
            "\"Oh, I see... You have the manual!\"",
            "\"I suppose I've taught you everything you need to know about battle sequences...\"",
            "\"I guess I???m just not useful anymore???\"",
            "\"That???s okay with me! Sayonara!\"",
            
        };
        sentimentalFaliure = new string[]{
            "You present the manual.",
            "\"Ah, that???s my sentimental item...\"\n\"You might want to use that after you???ve lit all your bulbs...\"",
            "\"'Talk' to me for more information.\""
        };

        /*
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
        };*/

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Theremin,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Theremin.\nIt???s not so effective.",
                    "\"You know, I???m resistant to this attack.\"\n\"You might wanna try the other attack.\""
                },new object[][]{SingleMethod(1)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Theremin.\nIt???s not so effective.",
                    "\"I???m resistant to this attack.\"\n\"That means that this attack does less damage to me than your flashlight.\""
                },new object[][]{SingleMethod(1)}),
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Flashlight.\nIt???s pretty effective!",
                    "\"Ouch, I???m weak to this attack!\"\n\"If you keep this up, my HP will be at zero.\"", "\"When my HP falls to zero, you can use the crucifix to end the battle!\"\n\"But there???s another much less painful way too, please talk to me to learn about that. Crucifixion really hurts...\""

                },new object[][]{SingleMethod(2)}),
            },new int[]{-1,0}),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Flashlight.\nIt???s pretty effective!",
                    "\"Attacking an enemy ghost is a good way to win battles!\", \"But there???s another way too...\"", "\"You can use talk to beat ghosts by interacting with them!\"\n\"I personally recommend it!\""
                },new object[][]{SingleMethod(2)}),
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attack with the Flashlight.\nIt???s pretty effective!",
                    "\"Please lighten up! This really hurts...\"\n\"You???re not going to actually crucify me when my HP hits 0, are you????\"",

                },new object[][]{SingleMethod(2)}),
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[]{
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You talk with the enemy ghost.",
                    "\"Oh, hey, you???re talkin??? to me!\"\n\"More of a lover than a fighter, I presume?\"",
                    "\"The talk system works differently than the attack system.\n\"Let me explain it to you.\"\"",
                    "\"Your goal for a talk victory is to get every bulb lit on your scanner.\"\n\"Every unlit bulb represents a talk that you have to perform.\"",
                    "\"But, here???s the catch:\"\n\"not every talk choice makes a bulb light up, and your talk choices change with every talk you get right.\"","\"Observe the way that your talk choices change after this next attack of mine.\""

                }, new object[][]{SingleMethod(new int[]{1}),SingleMethod(1),SingleMethod("talk",new string[]{},new string[]{})})
            },new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Chat,
            new EnemyReaction[] {
                NewReaction(new string[] {},
                new string[]{
                    "You chatted up the enemy ghost.",
                    "\"Nothing much, man how about you?\"",
                    "\"As you can see, this is a classic example of a talk that doesn???t make a bulb light up.\"","\"These don't proceed combat, but they're great opportunity to get to know your enemy, nonetheless.\""
                },new object[][]{})
            },new int[]{1}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Request_Health,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamagePlayer"},
                new string[]{
                    "You requested some health from the enemy ghost.",
                    "\"Oh, here you go.\"\n\"I can spare some health for a newbie.\"",
                    "\"As you can see, some talks can do special things like heal you.\"\n\"Later on, some may damage you, too, so be careful.\""
                },new object[][]{SingleMethod(-4)})
            },new int[]{1}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Request_Proceed,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks","SentimentalItem"},
                new string[]{
                    "You ask the enemy ghost if he can just get on with it already.",
                    "\"Ah, the impatient type.\"\n\"I see how it is.\"",
                    "\"Don???t worry. You???re almost done.\"",
                    "\"You will have the option to end combat when the enemy???s bulbs are all lit up, or when their health is at zero.\"\n\"You can end combat in a few different ways.\"",
                    "\"You can use crucifix to straight-up destroy my soul, but you???ve talked sweet with me all this time, so I don???t think that???s what you???re going for.\"",
                    "\"The other way is to use a sentimental item from your inventory.\",\"This is an item that is special to the ghost you???re fighting, using it will save their soul.\"",
                    "\"My sentimental item is the MANUAL.\"\n\"It was by the entrance to the library, did you pick it up?\"",
                    "\"To use it, go to items and select the MANUAL.\""

                },new object[][]{SingleMethod(new int[]{2}),SingleMethod(2),SingleMethod("request",new string[]{},new string[]{})})
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
            "\"Thaaaank yyooouu...\""
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
                    "You attacked with the Theremin...",
                    "The ghost looks indifferent.",
                    "\"Are you picking on me?\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Fire_Poker,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Fire Poker...",
                    "The ghost is unphased.",
                    "\"I couldn't feel that... I haven't been able to feel for a while...\"",
                    "\"Ouch, that hurts...?\"\n\"is that want you want me to say?\""
                },new object[][]{SingleMethod((object)1)})
            }),
            GenResponse(ButtonEnum.Attack,(int)AttackActions.Flashlight,
            new EnemyReaction[] {
                NewReaction(new string[] {"DamageEnemy"},
                new string[]{
                    "You attacked with the Flashlight...\nThe ghost has a painful expression on his face.",
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
        sentiment = new List<string>{"time","eternity","future","hope"};
        name = "Bored Ghost";
        hp = 18;
        maxHP = 18;
        id = 3;
        attackPrefabNames = new string[] {
            "Prefabs/combatEnemyTurn/attacks/Sine_BG_1",
            "Prefabs/combatEnemyTurn/attacks/Sine_BG_1",
            "Prefabs/combatEnemyTurn/attacks/None_BG",
            "Prefabs/combatEnemyTurn/attacks/None_BG_2"};
        //attackPrefabNames
        talkActions = new TalkEnum[][]{ 
            new TalkEnum[]{TalkEnum.Talk},
            new TalkEnum[]{TalkEnum.Time,TalkEnum.Clocks,TalkEnum.Cycles},
            new TalkEnum[]{TalkEnum.Present,TalkEnum.Eternity,TalkEnum.Past},
            new TalkEnum[]{TalkEnum.Hope,TalkEnum.Future,TalkEnum.Doom,TalkEnum.Bid_Farewell} 
        };
        displayPrefabPath = "Prefabs/EnemySpritePrefabs/BoredGhostDisplay";

        specialVals = new int[] {0};

        sentimentalTrigger = new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Bid_Farewell);
        sentimentalSuccess = new string[] {
            "\"I think I see it??? There???s a light??? Maybe this is my future?\""
        };
        sentimentalFaliure = new string[]{
            "\"No??? I can???t??? I can???t go anywhere, I don???t want to go anywhere.\"","\"Things will be fine the way they have been. Things don???t need to change...\""
        };

        responses = new EnemyResponse[]{
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks"},
                new string[]{
                    "You try asking the ghost to talk to you.",
"\"Ok, I guess I???ll talk to you???\",\"I haven???t done anything this interesting in years.\""
                },new object[][]{SingleMethod(new int[]{1}),SingleMethod(1)})
            }, new int[]{0}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeSpecialAbs","ChangeTalks"},
                new string[]{
                    "You try asking the ghost to keep talking.",
"\"You???ve got more to say?\"\n\"Try not being so boring this time, I guess.\""
                },new object[][]{SingleMethod(new int[]{2}),SingleMethod(1)})
            }, new int[]{1}),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Talk,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks"},
                new string[]{
                    "You try asking the ghost to keep talking.",
"\"Again? You really are stubborn.\"\n\"Well, I've got all day, whatcha got?\""
                },new object[][]{SingleMethod(1)})
            }, new int[]{2}),
/////////////////////////////////////////////////////////
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Time,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks","SentimentalItem"},
                new string[]{
                    "You try talking to the ghost about time.\"",
"\"Ah time, you can never have enough??? until you do.\""

                },new object[][]{SingleMethod(2),SingleMethod("time",
                new string[]{"\"I???ve been awfully bored, being dead and all.\"",
"\"I???ve seen some pretty grizzly things go down, but I stopped caring years ago.\""},
                new string[]{ "\"Were you going somewhere with this?\""}
                )})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Clocks,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks"},
                new string[]{
                    "You try to discuss clocks with the ghost.",
"\"What???s this about clocks? What, do you like watching the hands tick?\"",
"\"I guess that???s one way to spend a life??? but you???ll be doing plenty of that in the afterlife.\"",
"\"Whatever, I guess???\""
                },new object[][]{SingleMethod(0)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Cycles,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks","DamagePlayer"},
                new string[]{
                    "You try to discuss the meaning of cycles in time.",
"\"Cycles? Really? Well, the world does run on cycles. The earth spins, day changes to night changes to day, kids get taken down to the basement, yadda yadda.\"",
"\"Nothing ever starts and nothing ever ends, it just keeps going. Go with the flow, let it pass you over- you???ll get used to it.\"",
"The ghost???s nihilism hurts you a little..."
                },new object[][]{SingleMethod(0),SingleMethod(2)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Present,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks","DamagePlayer"},
                new string[]{
                    "You tell the ghost to try living in the present.",
"\"LIVE in the present? I???d have to be living first. My time???s long gone. Judging by how weak you look, yours isn???t too long either.\"",
"\"No offense to you, but you???re no match for the other spirits in this place.\"",
"The ghost???s words painfully unsettle you???" 
                },new object[][]{SingleMethod(1),SingleMethod(1)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Past,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks"},
                new string[]{
                    "You tell the ghost that they can try fondly remembering the past.",
"\"Why would I do that? There???s just enough behind me as there is going for me. By that I mean there???s nothing.\""
                },new object[][]{SingleMethod(1)})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Eternity,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks","SentimentalItem"},
                new string[]{
                    "You tell the ghost that all that???s left is eternity and nothingness.",
"\"Wow, someone actually gets it. I thought I was totally alone."
                },new object[][]{SingleMethod(3),SingleMethod(
                    "eternity",
                    new string[]{ "\"I can???t believe a living person of all things could empathize with me... I feel like you really get it.\""},
                    new string[]{"\"You???ve been nice to be, but unless you have something else to say you???d best be on your way.\""}
                )})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Hope,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem"},
                new string[]{
                    "You tell the ghost that everything is going to be ok."
                },new object[][]{SingleMethod(
                    "hope",
                    new string[]{"\"That??? gives me hope. Do you really mean that?\""},
                    new string[]{"\"But where will I go, what will I do? I???m trapped here for eternity...\""}
                )})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Future,
            new EnemyReaction[] {
                NewReaction(new string[] {"SentimentalItem"},
                new string[]{
                    "You tell the ghost that it???s future lies elsewhere, it just needs to go there."
                },new object[][]{SingleMethod(
                    "future",
                    new string[]{"\"You???re really right, there is more out there, I totally didn???t even think about that- I???ve been so caught up doing??? ...nothing, I guess.\""},
                    new string[]{"\"Do you really think i???ll get there?\""}
                )})
            }),
            GenResponse(ButtonEnum.Talk,(int)TalkEnum.Doom,
            new EnemyReaction[] {
                NewReaction(new string[] {"ChangeTalks"},
                new string[]{
                    "You tell the ghost that it???s future lies elsewhere, it just needs to go there."
                },new object[][]{SingleMethod(2)})
            }),

        };  
    }
}