using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Combat;
using UnityEngine.UI;


public class battleBehavior : MonoBehaviour
{
    TextManager tm;
    public EnemyClass enemy;
    //Our enemy.
    public HeroClass hero;
    public Submenu subMenu;
    //The submenu position.
    public GamePosition gamePosition;
    //Where we are in the turn.
    public GameObject textBox;
    //The textbox at the top of the screen.
    public GameObject MenuPanel;
    //menu with player commands
    public Text fillerText;
    //The text for the text at the top.
    public float typeSpeed = 0.035f;
    //How fast does text scroll?
    bool isTyping;
    bool cancelTyping;
    //Two variables to control text scrolling.
    float timer = 0f;
    //How long into texts scrolling are we?
    public int currentLine;
    //the current line within the text scroll.
    public int endAtLine;
    //when to end the textscroll.
    ButtonEnum? currentAction;
    //what submenu we're currently in
    private IEnumerator theScroll;
    //our scroll
    public GameObject speechTail;
    //VisualAid used for speech
    public AudioSource tick;
    public AudioSource click;
    public AudioSource Damage;
    //audio for textscroller
    public ScannerLogic scannerLogic;
    public GameObject scanner;
    InventoryManager inventoryManager;
    Image healthbarFilled;

    GameSceneManager gameSceneManager;
    enum endCon{
        SENTIMENT,
        DEFEAT,
        RUN,
        CRUCIFIX
    }
    int battleEnded = -1;
    float baseRunChance = 0.2f;
    GameObject minigame;
    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        gameSceneManager = tm.gameObject.GetComponent<GameSceneManager>();
        enemy = EnemyLibrary.GetEnemyFromId(inventoryManager.enemyID);
        StartCoroutine(theScroll = TextScroll(enemy.name + " manifests into view!"));
        MenuPanel = GameObject.Find("MenuPanel");
        subMenu = GameObject.Find("SubmenuPanel").GetComponent<Submenu>();
        subMenu.gameObject.SetActive(false);
        scannerLogic.DecideLights(enemy.hp, enemy.maxHP);
        healthbarFilled = GameObject.Find("HealthbarFilled").GetComponent<Image>();
        hero = new HeroClass();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePosition == GamePosition.EnemyDialogue && timer > typeSpeed)
        {
            //Handling the clicking through of enemy dialogue, and starting of the enemy turn.
            if (Input.GetMouseButtonDown(0))
            {
                if (!isTyping)
                {
                    currentLine += 1;
                     if (currentLine > endAtLine)
                    {
                        if (battleEnded >-1){
                            EndCombat((endCon)battleEnded);
                        }
                        EnemyTurn();
                    }
                    else
                    {
                        StartCoroutine(theScroll = TextScroll(enemy.toScroll[currentLine]));
                        click.Play();
                    }
                }
                else if (isTyping && !cancelTyping)
                {
                    cancelTyping = true;
                }
            }
        }
    }
    public void UpdatePlayerHp(){
        healthbarFilled.fillAmount = (float)hero.hp/(float)hero.maxHP;
    }
    public void ButtonPress(ButtonEnum buttonNum)
    {
        //This gets called from the button scripts when a button gets pressed.
        ButtonPress((int)buttonNum);
    }

    public void ButtonPress(int buttonNum){
        click.Play();
        if (buttonNum == (int)ButtonEnum.Run || buttonNum == (int)ButtonEnum.Crucifix){
            SubButtonPressed((ButtonEnum)buttonNum,0);
        }else{
            subMenu.gameObject.SetActive(true);
            currentAction = (ButtonEnum)buttonNum;
            Debug.Log(subMenu);
            subMenu.OpenMenu((ButtonEnum)buttonNum);
        }
    }

    public void ExitSubmenu(){
        if (currentAction!=null){
            currentAction = null;
        }
        subMenu.gameObject.SetActive(false);
    }


    void beginTurn()
    {
        //The logic for beginning a turn.
        gamePosition = GamePosition.PlayerChoice;
        textBox.SetActive(true);
        MenuPanel.SetActive(true);
        scanner.SetActive(true);
        enemy.toScroll = new string[] {"this enemy has" + enemy.hp+" of " + enemy.maxHP+" hitpoints"};
        endAtLine = enemy.toScroll.Length - 1;
        currentLine = 0;
        StopCoroutine(theScroll);
        StartCoroutine(theScroll = TextScroll(enemy.toScroll[currentLine]));
    }

    public void ExternalSubButtonPressed(int actionID){
        SubButtonPressed(currentAction ?? (ButtonEnum)0, actionID);
    }

    void SubButtonPressed(ButtonEnum actionType, int actionID)
    {
        gamePosition = GamePosition.EnemyDialogue;
        textBox.SetActive(true);
        currentLine = 0;
        int thisEnemyID = enemy.id;
        ExitSubmenu();
        MenuPanel.SetActive(false);
        enemy.toScroll = GetEnemyTextByID(thisEnemyID,actionType,actionID);
        endAtLine = enemy.toScroll.Length - 1;
        
        StopCoroutine(theScroll);
        StartCoroutine(theScroll = TextScroll(enemy.toScroll[currentLine]));
        SentimentalItemUsed(new EnemyActionCase((int)actionType,actionID));
        scannerLogic.DecideLights(enemy.hp, enemy.maxHP);
        ExitSubmenu();
    }
    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        fillerText.text = "";
        isTyping = true;
        cancelTyping = false;
        timer = 0;

        while (isTyping && !cancelTyping && (letter < lineOfText.Length))
        {
            if ((lineOfText[letter] == ' ') || (lineOfText[letter] == '.'))
            { }
            else
            {
                tick.Play();
            }
            fillerText.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
            timer += typeSpeed;
        }

        fillerText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;

    }
    void EnemyTurn()
    {
        scanner.SetActive(false);
        gamePosition = GamePosition.EnemyAttack;
        textBox.SetActive(false);
        EnemyAttackStart();
    }

    public void DamageEnemy(int damage){
        enemy.hp -= damage;
        CheckEnemyAlive();
        Debug.Log(enemy.hp);
    }
    public void DamageEnemyWeak(int damage){
        DamageEnemy(damage*2);
    }
    public void DamageEnemyResist(int damage){
        DamageEnemy(Mathf.FloorToInt(damage/2));
    }

    void CheckEnemyAlive(){
        if (enemy.hp < 0)
        { enemy.hp = 0;/* EnemyDead(); */ }
        else if (enemy.hp > enemy.maxHP)
        { enemy.hp = enemy.maxHP; }
    }

    string[] Sentimental(string[] success, string[] failiure){
        if (enemy.sentiment.Count<=0){
            //do something to end the battle
            battleEnded = (int)endCon.SENTIMENT;
            return success;//combat ended
        }else{
            return failiure;//
        }
    }

    void SentimentalItemUsed(EnemyActionCase action){
        if (enemy.hp==0&&enemy.sentiment.Contains(action)){
            enemy.sentiment.Remove(action);
        }
        //Tick Sentiment meter
    }

    void EndCombat(endCon condition){
        switch(condition){
            case endCon.DEFEAT:
                gameSceneManager.GameOver();
            break;
            case endCon.SENTIMENT:
                gameSceneManager.ExitCombat();
            break;
            case endCon.RUN:
                gameSceneManager.ExitCombat();
            break;
            case endCon.CRUCIFIX:
                gameSceneManager.ExitCombat();
            break;
        }
    }

    public void DamagePlayer(int damage){//can be negative to increase health
        if (hero.hp < 0)
        {hero.hp = 0; /*PlayerLose();*/}
        else if (hero.hp > hero.maxHP)
        { hero.hp = hero.maxHP; }
        UpdatePlayerHp();
    }

    void NotSetUp(){
        Debug.Log("action ID not set up in battleBehavior");
    }

    public void PlayerAttackStart(){//attack id, various parameters
        //create minigame stuff based on params
    }
    public void PlayerAttackEnd(){
        //remove minigame stuff
        Destroy(minigame);
    }
    public void EnemyAttackStart(){//attack id, various parameters
        AttackLogic logic = Instantiate(enemy.GetRandomAttack(),null).GetComponent<AttackLogic>();
        logic.transform.parent = this.transform.parent;
        logic.bb=this;
        //create minigame stuff based on params
    }
    public void EnemyAttackEnd(){
        //remove minigame stuff
        Destroy(minigame);
        beginTurn();
    }
    public string[] GetEnemyTextByID(int enemyID, ButtonEnum actionType, int actionID){//using switch because no loaded memory and fast
        switch(actionType){
            ////////////////////////////////////////////////////    Attack    ////////////
            case ButtonEnum.Attack:
                switch(actionID){
                    case (int)AttackActions.Theremin:
                        switch(enemyID){
                            case (int)Enemies.ghostA:
                                return new string[] {"You attacked with the theramin...",
                                "The ghost... liked it?",
                                "\"That's nice...\"",
                                "\"Not really my genre though.\"",
                                "\"I'm more of a 'Boos' kind of guy.\""};
                                case (int)Enemies.Poor_Dog:
                                DamageEnemyWeak(2);
                                return new string[] {"You attacked with the theremin...",
                                "The ghost recoils at the pitch!",
                                "\"Whine.... Turn it off...\""};
                            default:
                                DamageEnemy(5);
                                return new string[] {"You attacked with the therimin...",
                                "The ghost's form wavers.",
                                "\"Looks like it hurt a little...\""};
                        }

                    case (int)AttackActions.Fire_Poker:
                        switch(enemyID){
                            case (int)Enemies.Poor_Dog:
                                DamageEnemy(2);
                                return new string[] {"You attacked with the FirePoker...",
                                "The ghost isn't loving it... but isn't hating it, either.",
                                "\"Too heavy to be stick...\nTo long to be ball...\"",
                                "\":(\""};
                            default:
                                DamageEnemy(5);
                                return new string[] {"You Attacked with the Fire Poker...",
                                "It worked fine!",
                                "\"Hey, cut that out!\""};
                        }
                    
                    case (int)AttackActions.Flashlight:
                        switch(enemyID){
                            case (int)Enemies.ghostA:
                                DamageEnemyWeak(2);
                                return new string[] {"You attacked with the flashlight...",
                                "It was especially effective!",
                                "\"Ow, who turned on the lights?\""};
                                case (int)Enemies.Poor_Dog:
                                DamageEnemyResist(2);
                                return new string[] {"You attacked with the flashlight...",
                                "The ghost starts darting after the light, thinking it's a ball!",
                                "You go on for a few minutes, making the ghost run in circles.",
                                "This isn't exactly effective...",
                                "\"Where did flat ball go?????\"",
                                "\"Do you have better ball!?!?!?!?!?\""};
                            default:
                                DamageEnemy(5);
                                return new string[] {"You attacked with the flashlight...",
                                "The ghost tries to evade the beam.",
                                "Looks like it hurt a bit..."};
                        }
                    
                    case (int)AttackActions.Garlic://in the case where you want a specific interaction:
                        switch(enemyID){
                            case (int)Enemies.ghostA: //if the target is test enemy 0...
                                //do something specific to this enemy
                                return new string[] {"You Attacked with the Garlic...",
                                "It worked fine!",
                                "\"I'm a ghost, not a vampire...\""};
                                case (int)Enemies.Poor_Dog:
                                DamageEnemyWeak(2);
                                return new string[] {"You attacked with the Garlic...",
                                "The ghost hates it!",
                                "\"Smelly ball bad for me!!! Give better ball >:(\""};

                            default:
                                //execute what this does
                                DamageEnemy(5);
                                return TextManager.stringsToArray("Garlic used");
                        }          
                    default: NotSetUp(); return new string[] {"..."};
                }
            ////////////////////////////////////////////////////    Talk    ////////////
            case ButtonEnum.Talk:
                //talk
                switch(actionID){
                    case (int)TalkEnum.Chat:
                        switch(enemyID){
                            case 0:
                                return new string[] {"You had a chat with the ghost...",
                                "The ghost really enjoyed that.",
                                "You think it might have smiled a little"};
                            case (int)Enemies.Poor_Dog:
                                return new string[] { "You started talking with the ghost...",
                            "You might've briefly mentioned something that sounds vaguely like the word 'ball'.",
                            "\"Ball!?!?!?!?!?\"",
                            "\"Ohh... False Ball-arm...\"" };
                            default:
                                return new string[] {"You had a chat with the ghost...",
                                "The ghost seemed bored..."};  
                        }
                        case (int)TalkEnum.Fake_Throw:
                        switch(enemyID){
                            
                            case (int)Enemies.Poor_Dog:
                                return new string[] { "You made a throwing motion with your arm...",
                            "But there was nothing in your arm?",
                            "\"How could you!!!????\""};
                            default:
                                return new string[] {"You threw something that didn't exist...",
                                "The ghost seemed bored..."};  
                        }
                        case (int)TalkEnum.Pet:
                        switch(enemyID){
                            
                            case (int)Enemies.Poor_Dog:
                                return new string[] { "You tried to pet the ghost...",
                            "But your arm phased right through 'em, so...",
                            "You just kinda made a petting motion with your arm.",
                            "Between you and me, I don't think he knows the difference.",
                            ":)"};
                            default:
                                return new string[] {"You threw something that didn't exist...",
                                "The ghost seemed bored..."};  
                        }
                    default: NotSetUp(); return new string[] {"..."};
                }
            ////////////////////////////////////////////////////    Items    ////////////
            case ButtonEnum.Items:
                //use item
                switch(actionID){
                    case (int)ItemsEnum.Apple:
                        DamagePlayer(-5);
                        return new string[] {"You ate the apple...",
                        "and gained 5 health!",
                        "\"...\""};
                    case (int)ItemsEnum.Ball:
                        switch(enemyID){
                            case (int)Enemies.ghostA:
                                return Sentimental(new string[]{"You showed the ball to the ghost...",
                                "It felt... right.",
                                "\"Thank you...\""},
                                new string[]{"The ghost hesitates and looks at the ball...",
                                "Does this ball mean something to it?",
                                "It snaps out of it's trance, you must have been too soon."});
                                case (int)Enemies.Poor_Dog:
                                return Sentimental(new string[]{"You showed the ball to the ghost...",
                                "It felt... right.",
                                "\"BALL!?!?!???!?\"",
                                "\"!?!?!?!???!??!!?!??!??!?!??!?!?!??!?\"",
                                "\"!?!?!?!???!??!!?!!?!?!?!??!?!?!?!?!????!??!?!??!?!?!??!?\"",
                                "\"!?!?!?!?!?!?!?!????!??!?!??!?!???!??!!?!!?!?!?!??!?!?!?!?!????!??!?!??!?!?!??!?\"",
                                "...",
                                "\"!?\""},
                                new string[]{"The ghost hesitates and looks at the ball...",
                                "Does this ball mean something to it?",
                                "\"I feel like there could be ball...?\"",
                                "\"But... No see ball?????\"",
                                "It snaps out of it's trance, you must have been too soon."});

                        }
                        return new string[] {"You held the ball out to the being...",
                        "But it cannot see it!",
                        "Your machine needs more charge!",
                        "\"...\""};
                    default: NotSetUp(); return new string[] {"..."};
                }
            ////////////////////////////////////////////////////    Crucifix    ////////////
            case ButtonEnum.Crucifix:
                //try to crucifix
                if (enemy.hp ==0){
                    //enemy banished
                    battleEnded = (int)endCon.CRUCIFIX;
                    //add text about crucifix 
                    return new string[]{"the ghost withers away in a a blinding flash"};
                }else{
                    return new string[]{"The ghost was still too powerfull"};
                }
                
            ////////////////////////////////////////////////////    Run    ////////////
            case ButtonEnum.Run:
                //try to run
                if (Random.Range(0f,1f) > baseRunChance + (1-baseRunChance)*(1-(float)enemy.hp/(float)enemy.maxHP)){
                    //add text about escaping 
                    battleEnded = (int)endCon.RUN;
                    return new string[]{"you got away safely"};
                }else{
                    //add text about not being able to run
                    return new string[]{"couldn't get away"};
                }
                
            default: return new string[]{""};//should never be reached
        }
    }
}
