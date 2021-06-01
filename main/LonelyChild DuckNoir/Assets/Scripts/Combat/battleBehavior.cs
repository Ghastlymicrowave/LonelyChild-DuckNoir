using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Combat;


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
    public GameObject playerChoiceBox;
    //The textbox at the top of the screen.
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
    //audio for textscroller
    public ScannerLogic scannerLogic;
    public GameObject scanner;
    InventoryManager inventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        enemy = EnemyLibrary.GetEnemyFromId(inventoryManager.enemyID);
        StartCoroutine(theScroll = TextScroll(enemy.name + " manifests into view!"));
        
        subMenu = GameObject.Find("SubmenuPanel").GetComponent<Submenu>();
        subMenu.gameObject.SetActive(false);
        
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
        playerChoiceBox.SetActive(true);
        textBox.SetActive(true);
    }

    public void ExternalSubButtonPressed(int actionID){
        SubButtonPressed(currentAction ?? (ButtonEnum)0, actionID);
    }

    void SubButtonPressed(ButtonEnum actionType, int actionID)
    {


        gamePosition = GamePosition.EnemyDialogue;
        playerChoiceBox.SetActive(false);
        textBox.SetActive(true);
        currentLine = 0;
        int thisEnemyID = enemy.id;
        ExitSubmenu();
        enemy.toScroll = GetEnemyTextByID(thisEnemyID,actionType,actionID);
        endAtLine = enemy.toScroll.Length - 1;
        scannerLogic.DecideLights(enemy.hp, enemy.maxHP);
        StopCoroutine(theScroll);
        StartCoroutine(theScroll = TextScroll(enemy.toScroll[currentLine]));
        SentimentalItemUsed(new EnemyActionCase((int)actionType,actionID));
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
        playerChoiceBox.SetActive(false);
        textBox.SetActive(false);
        print("wediditreditt");
    }

    public void DamageEnemy(int damage){
        enemy.hp -= damage;
        CheckEnemyAlive();
    }
    public void DamageEnemyWeak(int damage){
        enemy.hp -= damage*2;
        CheckEnemyAlive();
    }
    public void DamageEnemyResist(int damage){
        enemy.hp -= Mathf.FloorToInt(damage/2);
        CheckEnemyAlive();
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
            SentimentalVictory();
            return success;//combat ended
        }else{
            return failiure;//
        }
    }

    void SentimentalItemUsed(EnemyActionCase action){
        if (enemy.sentiment.Contains(action)){
            enemy.sentiment.Remove(action);
        }
        //Tick Sentiment meter
    }

    void SentimentalVictory(){
        
    }

    public void DamagePlayer(int damage){//can be negative to increase health
        if (hero.hp < 0)
        {hero.hp = 0; /*PlayerLose();*/}
        else if (hero.hp > hero.maxHP)
        { hero.hp = hero.maxHP; }
    }

    void NotSetUp(){
        Debug.Log("action ID not set up in battleBehavior");
    }

    public void PlayerAttackStart(params int[] attackParams){//attack id, various parameters
        //create minigame stuff based on params
    }
    public void PlayerAttackEnd(){
        //remove minigame stuff
    }
    public void EnemyAttackStart(params int[] attackParams){//attack id, various parameters
        //create minigame stuff based on params
    }
    public void EnemyAttackEnd(){
        //remove minigame stuff
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
                            default:
                                DamageEnemy(5);
                                return new string[] {"You attacked with the therimin...",
                                "The ghost's form wavers.",
                                "\"Looks like it hurt a little...\""};
                        }

                    case (int)AttackActions.FirePoker:
                        switch(enemyID){
                            default:
                                DamageEnemy(5);
                                return new string[] {"You Attacked with the Fire Poker...",
                                "It worked fine!",
                                "\"Hey, cut that out!\""};
                        }
                    
                    case (int)AttackActions.Flashlight:
                        switch(enemyID){
                            case (int)Enemies.ghostA:
                                DamageEnemyWeak(5);
                                return new string[] {"You attacked with the flashlight...",
                                "It was especially effective!",
                                "\"Ow, who turned on the lights?\""};
                            default:
                                DamageEnemyWeak(5);
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
                            default:
                                return new string[] {"You had a chat with the ghost...",
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

                        }
                        return new string[] {"You held the ball out to the being...",
                        "But it cannot see it!",
                        "Your machine needs more charge!",
                        "\"...\""};
                    default: NotSetUp(); return new string[] {"..."};
                }
            ////////////////////////////////////////////////////    Crucifix    ////////////
            case ButtonEnum.Crucifix:
                //try to run
                return new string[]{"using cruifix"};
            ////////////////////////////////////////////////////    Run    ////////////
            case ButtonEnum.Run:
                //try to run
                return new string[]{"running"};
            default: return new string[]{""};//should never be reached
        }
    }

    public void EnemyAttack(){
        switch(enemy.id){
            case (int)Enemies.ghostA:
                switch(Random.Range(0,2)){
                    case 0:
                    break;
                    case 1:
                    break;
                }
            break;
            default:
            Debug.Log("enemy id not set up for EnemyAttack");
            break;
        }
    }
}
