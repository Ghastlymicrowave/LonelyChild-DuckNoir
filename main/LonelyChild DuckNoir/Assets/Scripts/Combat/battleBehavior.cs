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
    GameObject enemyParent;
    Animator healthbarAnim;
    enum endCon{
        SENTIMENT,
        DEFEAT,
        RUN,
        CRUCIFIX
    }
    int battleEnded = -1;
    float baseRunChance = 0.4f;
    GameObject minigame;
    List<string> toScroll;
    DisplayEnemy enemyImage;
    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        gameSceneManager = tm.gameObject.GetComponent<GameSceneManager>();
        enemy = EnemyLibrary.GetEnemyFromId(inventoryManager.enemyID,this);
        StartCoroutine(theScroll = TextScroll(enemy.name + " manifests into view!"));
        MenuPanel = GameObject.Find("MenuPanel");
        subMenu = GameObject.Find("SubmenuPanel").GetComponent<Submenu>();
        subMenu.gameObject.SetActive(false);
        scannerLogic.DecideLights(enemy.hp, enemy.maxHP);
        healthbarFilled = GameObject.Find("HealthbarFilled").GetComponent<Image>();
        hero = new HeroClass();
        enemyParent = GameObject.Find("EnemyParent");
        GameObject toInstantiate = (GameObject)Resources.Load(enemy.displayPrefabPath) as GameObject;
        GameObject enemyImageObj = Instantiate(toInstantiate);
        enemyImageObj.transform.SetParent(enemyParent.transform);
        enemyImageObj.transform.localPosition = Vector3.zero;
        enemyImage = enemyImageObj.GetComponent<DisplayEnemy>();
        healthbarAnim = healthbarFilled.transform.parent.GetComponent<Animator>();
        toScroll = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePosition == GamePosition.EnemyDialogue && timer > typeSpeed)
        {
            //Handling the clicking through of enemy dialogue, and starting of the enemy turn.
            if (Input.GetMouseButtonDown(0)||Input.GetButtonDown("Interact"))
            {
                if (!isTyping)
                {
                    currentLine += 1;
                     if (currentLine > toScroll.Count-1)
                    {
                        if (battleEnded >-1){
                            EndCombat((endCon)battleEnded);
                        }
                        EnemyTurn();
                    }
                    else
                    {
                        StartCoroutine(theScroll = TextScroll(toScroll[currentLine]));
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
        Debug.Log(healthbarFilled.fillAmount.ToString());
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
        healthbarAnim.SetBool("IsMinigame",false);
        scanner.SetActive(true);
        toScroll.Clear();
        toScroll.Add(enemy.splashTexts[Random.Range(0,enemy.splashTexts.Length)]);
        currentLine = 0;
        StopCoroutine(theScroll);
        StartCoroutine(theScroll = TextScroll(toScroll[currentLine]));
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
        healthbarAnim.SetBool("IsMinigame",true);
        toScroll.Clear();
        
        if (enemy.sentimentalTrigger.actionType == (int)actionType && enemy.sentimentalTrigger.actionID == (int)actionID){
            Sentimental();
        }else{
            TriggerEnemyReaction(enemy,actionType,actionID);
        }
        if (toScroll.Count<1){
            toScroll.Add("you shouldn't be seeing this- there's something missing in the code?");
        }
        StopCoroutine(theScroll);
        StartCoroutine(theScroll = TextScroll(toScroll[currentLine]));
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
        Debug.Log("Dealt Damage: "+damage.ToString()+ "current HP: "+enemy.hp.ToString());
    }

    void CheckEnemyAlive(){
        if (enemy.hp < 0)
        { enemy.hp = 0;/* EnemyDead(); */ }
        else if (enemy.hp > enemy.maxHP)
        { enemy.hp = enemy.maxHP; }
    }

    public void Sentimental(){
        Debug.Log("using sentimental");
        if (enemy.sentiment.Count<=0){
            //do something to end the battle
            battleEnded = (int)endCon.SENTIMENT;
            toScroll.AddRange(enemy.sentimentalSuccess);//combat ended
        }else{
            toScroll.AddRange(enemy.sentimentalFaliure);
        }
    }

    void SentimentalItemUsed(EnemyActionCase action){
        Debug.Log("sentimental tried using");
        Debug.Log(enemy.sentiment);
        Debug.Log(action);

        for(int i = enemy.sentiment.Count-1; i > -1; i-=1){
            if (enemy.sentiment[i].actionType==action.actionType && enemy.sentiment[i].actionID==action.actionID){
                Debug.Log("SENTIMENTAL ITEM USED, "+enemy.sentiment.Count.ToString() +" remaining");
                toScroll.Add("...It seems the ghost really liked that...");
                enemy.sentiment.RemoveAt(i);
                if (enemy.sentiment.Count>0){
                    toScroll.Add("You have a feeling it still wants more- but of something else.");
                    break;
                }else{
                    toScroll.Add("It seems to be waiting for something important to happen.");
                    break;
                }
            }
        }
    }

    void EndCombat(endCon condition){
        switch(condition){
            case endCon.DEFEAT:
                gameSceneManager.GameOver();
            break;
            case endCon.SENTIMENT:
            inventoryManager.ghostsRoaming.Remove(enemy.id);
            inventoryManager.ghostsAscended.Add(enemy.id);
                gameSceneManager.ExitCombat();
            break;
            case endCon.RUN:
                gameSceneManager.ExitCombat();
            break;
            case endCon.CRUCIFIX:
            inventoryManager.ghostsRoaming.Remove(enemy.id);
            inventoryManager.ghostsCrucified.Add(enemy.id);
                gameSceneManager.ExitCombat();
            break;
        }
    }

    public void DamagePlayer(int damage){//can be negative to increase health
        hero.hp -= damage;
        if (hero.hp <= 0)
        {hero.hp = 0; EndCombat(endCon.DEFEAT); }
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

    public void TriggerEnemyReaction(EnemyClass enemy, ButtonEnum actionType, int actionID){
        EnemyReaction reactions = enemy.GetReaction(actionType,actionID);
        if (reactions!=null){
            toScroll.AddRange(reactions.toDisplay);
            reactions.React();
        }else{
            ActionDefault(actionType,actionID);
        }
    }

    void Missing(ButtonEnum actionType, int actionID){
        Debug.Log("Missing default response for action "+actionType.ToString()+" : "+actionID.ToString());
    }

    public void ActionDefault(ButtonEnum actionType, int actionID){
        switch(actionType){
            case ButtonEnum.Attack:
                switch(actionID){
                    case (int)AttackActions.Fire_Poker: 
                        DamageEnemy(2);
                        toScroll.AddRange(new string[]{
                            "You Attacked with the Fire Poker...",
                            "It worked fine!",
                            "\"Hey, cut that out!\""
                        });
                    break;
                    case (int)AttackActions.Ruler:
                        DamageEnemy(3);
                        toScroll.AddRange(new string[]{
                            "You attacked with the ruler...",
                            "Ouch, looks like that hurt quite a bit!"
                        });
                        break;
                    case (int)AttackActions.Flashlight:
                        DamageEnemy(1);
                        toScroll.AddRange(new string[]{
                            "You attacked with the flashlight...",
                            "The ghost tries to evade the beam.",
                            "Looks like it hurt a bit..."
                        });
                    break;
                    case (int)AttackActions.Garlic:
                        DamageEnemy(1);
                        toScroll.AddRange(new string[]{
                            "You Attacked with the Garlic...",
                                "It worked fine!",
                                "\"I'm a ghost, not a vampire...\""
                        });
                    break;
                    case (int)AttackActions.Theremin:
                        DamageEnemy(1);
                        toScroll.AddRange(new string[]{
                            "You attacked with the theremin...",
                            "The ghost recoils at the pitch!",
                            "\"Whine.... Turn it off...\""
                        });
                    break;
                    default: Missing(actionType,actionID);break;
                }
            break;
            case ButtonEnum.Talk:
                switch(actionID){
                    case (int)TalkEnum.Chat:
                        DamageEnemy(1);
                        toScroll.AddRange(new string[]{
                            "You had a chat with the ghost...",
                            "The ghost seemed bored..."
                        });
                    break;
                    case (int)TalkEnum.ChatTwo:
                        DamageEnemy(1);
                        toScroll.AddRange(new string[]{
                            "You had a chat with the ghost...",
                            "The ghost seemed bored..."
                        });
                    break;
                    case (int)TalkEnum.Fake_Throw:
                        DamageEnemy(1);
                        toScroll.AddRange(new string[]{
                            "You threw something that didn't exist...",
                            "The ghost seemed bored..."
                        });
                    break;
                    case (int)TalkEnum.Flirt:
                        DamageEnemy(1);
                        toScroll.AddRange(new string[]{
                            "You tried to flirt with the ghost...",
                            "The ghost seems to not care..."
                        });
                    break;
                    case (int)TalkEnum.Pet:
                        DamageEnemy(1);
                        toScroll.AddRange(new string[]{
                            "You tried to pet the ghost...",
                            "The ghost doesn't seem to care..."
                        });
                    break;
                    default: Missing(actionType,actionID);break;
                }
            break;
            case ButtonEnum.Items:
                switch(actionID){
                    case (int)ItemsEnum.Apple:
                        DamagePlayer(-5);
                        inventoryManager.RemoveItem((int)ItemsEnum.Apple);
                        toScroll.AddRange(new string[] {"You ate the apple...",
                        "and gained 5 health!",
                        "\"...\""});
                    break;
                    case (int)ItemsEnum.Ball:
                        toScroll.AddRange(new string[] {
                        "You showed the ball to the ghost...",
                        "It didn't seem to care..."});
                        break;
                    case (int)ItemsEnum.Manual:
                        toScroll.AddRange(new string[] {
                        "You showed the manual to the ghost...",
                        "It didn't seem to care..."});
                        break;
                    case (int)ItemsEnum.StaircaseKey:
                        toScroll.AddRange(new string[] {
                        "You showed the staircase key to the ghost...",
                        "It didn't seem to care..."});
                        break;
                    case (int)ItemsEnum.ShowingRoomKey:
                        toScroll.AddRange(new string[] {
                        "You showed the showing room key to the ghost...",
                        "It didn't seem to care..."});
                        break;
                    case (int)ItemsEnum.MasterBedroomKey:
                        toScroll.AddRange(new string[] {
                        "You showed the master bedroom key to the ghost...",
                        "It didn't seem to care..."});
                        break;
                    case (int)ItemsEnum.Hourglass:
                        toScroll.AddRange(new string[] {
                        "You showed the hourglass to the ghost...",
                        "It didn't seem to care..."});
                    break;
                    case (int)ItemsEnum.Key:
                        toScroll.AddRange(new string[] {
                        "You tried to use the key on the ghost...",
                        "That's a ghost, not a lock!",
                        "What's wrong with you?",
                        "\"...\""});
                    break;
                    case (int)ItemsEnum.Photo:
                        toScroll.AddRange(new string[] {
                        "You held the Photo out to the being...",
                        "But it seems it cannot see it..."});
                    break;
                    default: Missing(actionType,actionID);break;
                }
            break;
            case ButtonEnum.Crucifix:
                //try to crucifix
                if (enemy.hp ==0){
                    //enemy banished
                    battleEnded = (int)endCon.CRUCIFIX;
                    //add text about crucifix 
                    toScroll.AddRange(new string[]{"The ghost withers away in a a blinding flash!"});
                }else{
                    toScroll.AddRange(new string[]{"The ghost was still too powerfull"});
                }
            break;
            case ButtonEnum.Run:
                //try to run
                float chance = Random.Range(0f,1f);
                Debug.Log("chance: "+ (baseRunChance + (1f-baseRunChance)*(1f-(float)enemy.hp/(float)enemy.maxHP)).ToString()+ " got: "+ chance.ToString());
                if (enemy.canRun){
                    if (chance < baseRunChance + (1f-baseRunChance)*(1f-(float)enemy.hp/(float)enemy.maxHP)){
                        //add text about escaping 
                        battleEnded = (int)endCon.RUN;
                        toScroll.AddRange( new string[]{"you got away safely"});
                    }else{
                        //add text about not being able to run
                        toScroll.AddRange( new string[]{"couldn't get away"});
                    }
                }else{
                    toScroll.AddRange( new string[]{"You can't get away from this enemy!","If you want to leave, you'll have to deal with it first!"});
                }
            break;
        }
    }
}
