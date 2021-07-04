using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Combat;
using UnityEngine.UI;
using TMPro;

public class SpecialText{
    public string trigger;
    public string[] text;
    public int triggerOnce;//1 for only once, 0 for never, -1 for infinite
    public SpecialText(string triggerTag, string[] responseText, int triggerVal){
        trigger = triggerTag;
        text = responseText;
        triggerOnce = triggerVal;
    }
}
public class battleBehavior : MonoBehaviour
{
    public int talkIndex = 0;
    [SerializeField] TextMeshPro hpText;
    [SerializeField] GameObject playerHurt;
    [SerializeField] DamageNum damageNumPrefab;
    [SerializeField] List<GameObject> toDisableOnEnd;
    public Animator cameraShake;
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
    public SoundHolder Music;
    public ScannerLogic scannerLogic;
    public GameObject scanner;
    InventoryManager inventoryManager;
    Image healthbarFilled;
    Vector2 healthbarSizeDelta;
    GameSceneManager gameSceneManager;
    GameObject enemyParent;
    Animator healthbarAnim;
    public enum endCon{
        SENTIMENT,
        DEFEAT,
        RUN,
        CRUCIFIX
    }
    int battleEnded = -1;
    float baseRunChance = 0.4f;
    GameObject minigame;
    List<string> toScroll;
    List<string> toInject;
    DisplayEnemy enemyImage;
    List<SpecialText> specialText;
    public int[] spVals = {0};
    float animSpdScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        toInject = new List<string>();
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        gameSceneManager = tm.gameObject.GetComponent<GameSceneManager>();
        enemy = EnemyLibrary.GetEnemyFromId(inventoryManager.enemyID,this);
        StartCoroutine(theScroll = TextScroll(enemy.name + " manifests into view!"));
        MenuPanel = GameObject.Find("MenuPanel");
        subMenu = GameObject.Find("SubmenuPanel").GetComponent<Submenu>();
        subMenu.gameObject.SetActive(false);
        scannerLogic.DecideLights(5-enemy.sentiment.Count);
        healthbarFilled = GameObject.Find("HealthbarFilled").GetComponent<Image>();
        healthbarSizeDelta = healthbarFilled.rectTransform.sizeDelta;
        hero = new HeroClass();
        enemyParent = GameObject.Find("EnemyParent");
        GameObject toInstantiate = (GameObject)Resources.Load(enemy.displayPrefabPath) as GameObject;
        GameObject enemyImageObj = Instantiate(toInstantiate);
        enemyImageObj.transform.SetParent(enemyParent.transform);
        enemyImageObj.transform.localPosition = Vector3.zero;
        enemyImage = enemyImageObj.GetComponent<DisplayEnemy>();
        healthbarAnim = healthbarFilled.transform.parent.GetComponent<Animator>();
        toScroll = new List<string>();
        Music.audioSource.clip = gameSceneManager.GetCombatAudio();
        Music.Play();
        SetEnemyHpDisplay();
        spVals = enemy.specialVals;
    }

    void SendSignal(string signalName){
        if (enemy.specialTexts != null){
            for (int i = 0; i < enemy.specialTexts.Length;i++){
                if (enemy.specialTexts[i].trigger == signalName && enemy.specialTexts[i].triggerOnce!=0){
                    toInject.InsertRange(0,enemy.specialTexts[i].text);
                    if (enemy.specialTexts[i].triggerOnce==1){
                        enemy.specialTexts[i].triggerOnce = 0;
                    }
                }
            }
        }
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
                            enemyImage.EndAnim((endCon)battleEnded);

                        }else{
                            EnemyTurn();
                        }
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
        healthbarFilled.rectTransform.sizeDelta = new Vector2((float)hero.hp/(float)hero.maxHP * healthbarSizeDelta.x,healthbarSizeDelta.y) ;
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
        toScroll = toInject;
        toInject.Clear();
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
        if (enemy.triggerIsReaction){
            TriggerEnemyReaction(enemy,actionType,actionID);
            if (enemy.sentimentalTrigger.actionType == (int)actionType && enemy.sentimentalTrigger.actionID == (int)actionID){
            Sentimental();
            }
        }else{
            if (enemy.sentimentalTrigger.actionType == (int)actionType && enemy.sentimentalTrigger.actionID == (int)actionID){
            Sentimental();
            }else{
            TriggerEnemyReaction(enemy,actionType,actionID);
            }
        }
        
        if (toScroll.Count<1){
            toScroll.Add("...?");//should not be reached ideally
        }
        StopCoroutine(theScroll);
        StartCoroutine(theScroll = TextScroll(toScroll[currentLine]));
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

    public void SetEnemyHpDisplay(){
        hpText.text = "HP: "+enemy.hp.ToString()+"/"+enemy.maxHP.ToString();
    }

    #region  often used enemy methods
        //DamageEnemy
        //DamagePlayer
        //SentimentalItem
        //ChangeSpecialAbs
        //ChangeSpecialRel
        //ChangeTalks
        public void ChangeTalks(int index){
            talkIndex = index;
        }
        public void DamageEnemy(int damage){
            int mod = 1;
            if (Random.Range(0f,1f)>0.5f){
                mod = -1;
            }
            if (Random.Range(0f,1f)>0.7){
                damage = Mathf.Max(damage+mod,1);
            }
            
            enemy.hp -= damage;
            CheckEnemyAlive();
            Debug.Log("Dealt Damage: "+damage.ToString()+ "current HP: "+enemy.hp.ToString());
            SendSignal("GHOST_HP_"+enemy.hp);
            SendSignal("ENEMY_DAMAGED");
            DamageNum damageNumber = Instantiate(damageNumPrefab,enemyImage.gameObject.transform.position + Vector3.back*2,Quaternion.identity,null);
            damageNumber.Text(damage.ToString());

            float c = (float)enemy.hp;
            float m = (float)enemy.maxHP;
            enemyImage.SetIdleSpd( animSpdScale * (-c+m)/m+1f);
            SetEnemyHpDisplay();
        }
        public void DamagePlayer(int damage){//can be negative to increase health
            hero.hp -= damage;
            if(damage > 0)
            {
                cameraShake.Play("CombatCam_Shake");
                SendSignal("PLAYER_DAMAGED");
                if (enemy.playerHurt){
                    enemy.playerHurt = false;
                    playerHurt.SetActive(true);
                }
            }
            if (hero.hp <= 0)
            {hero.hp = 0; battleEnded = (int)endCon.DEFEAT;EndCombat(); }
            else if (hero.hp > hero.maxHP)
            { hero.hp = hero.maxHP; }
            UpdatePlayerHp();
            
        }
        public void SentimentalItem(string thisTag,string[] ifNew, string[] ifUsed){
            if (enemy.sentiment.Contains(thisTag)){
                enemy.sentiment.Remove(thisTag);
                toScroll.AddRange(ifNew);
                scannerLogic.DecideLights(5-enemy.sentiment.Count);
            }else{
                toScroll.AddRange(ifUsed);
            }
        }
        public void ChangeSpecialAbs(params int[] special){
            for (int i = 0; i < special.Length; i++){
                if (special[i]!=-1){
                    if (i >= spVals.Length){
                        Debug.LogError("ChangeSpecialAbs returned an array longer than the enemy's special value array!");
                        break;
                    }
                    spVals[i]= special[i];
                }
            }
        }
        public void ChangeSpecialRel(params int[] special){
            for (int i = 0; i < special.Length; i++){
                if (special[i]!=-1){
                    if (i >= spVals.Length){
                        Debug.LogError("ChangeSpecialRel returned an array longer than the enemy's special value array!");
                        break;
                    }
                    spVals[i] += special[i];
                }
            }
        }
    #endregion
    void CheckEnemyAlive(){
        if (enemy.hp < 0)
        { enemy.hp = 0;/* EnemyDead(); */ }
        else if (enemy.hp > enemy.maxHP)
        { enemy.hp = enemy.maxHP; }
    }

    public void DisableOnEnd(){
        foreach (GameObject i in toDisableOnEnd){
            i.SetActive(false);
        }
    }
    public void Sentimental(){
        Debug.Log("using sentimental");
        if (enemy.sentiment.Count<=0){
            battleEnded = (int)endCon.SENTIMENT;
            toScroll.AddRange(enemy.sentimentalSuccess);//combat ended
            DisableOnEnd();
        }else{
            toScroll.AddRange(enemy.sentimentalFaliure);
        }
    }

    

    /*void SentimentalItemUsed(EnemyActionCase action){
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
    }*/

    public void EndCombat(){
        switch((endCon)battleEnded){
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
        if (playerHurt.activeSelf){
            playerHurt.SetActive(false);
        }
    }
    public void EnemyAttackEnd(){
        //remove minigame stuff
        Destroy(minigame);
        beginTurn();
        enemyImage.Attack();
    }

    public void TriggerEnemyReaction(EnemyClass enemy, ButtonEnum actionType, int actionID){
        EnemyReaction reactions = enemy.GetReaction(actionType,actionID,spVals);
        if (reactions!=null){
            if (reactions.toDisplay.Length!=0){
                toScroll.AddRange(reactions.toDisplay);
            }
            for(int i = 0; i < reactions.methodNames.Length; i++){
                var thisMethod = this.GetType().GetMethod(reactions.methodNames[i]);
                thisMethod.Invoke(this,reactions.methodParams[i]);
            }
        }else{
            ActionDefault(actionType,actionID);
        }
    }

    void Missing(ButtonEnum actionType, int actionID){
       toScroll.AddRange(new string[]{
                            "That did absolutely nothing."
                        });
    }

    public void ActionDefault(ButtonEnum actionType, int actionID){
        switch(actionType){
            case ButtonEnum.Attack:
                switch(actionID){
                    case (int)AttackActions.Fire_Poker: 
                        DamageEnemy(4);
                        toScroll.AddRange(new string[]{
                            "You Attacked with the Fire Poker...",
                            "It worked fine!",
                            "\"Hey, cut that out!\""
                        });
                    break;
                    case (int)AttackActions.Ruler:
                        DamageEnemy(5);
                        toScroll.AddRange(new string[]{
                            "You attacked with the ruler...",
                            "Ouch, looks like that hurt quite a bit!"
                        });
                        break;
                    case (int)AttackActions.Flashlight:
                        DamageEnemy(2);
                        toScroll.AddRange(new string[]{
                            "You attacked with the flashlight...",
                            "The ghost tries to evade the beam.",
                            "Looks like it hurt a bit..."
                        });
                    break;
                    case (int)AttackActions.Garlic:
                        DamageEnemy(3);
                        toScroll.AddRange(new string[]{
                            "You Attacked with the Garlic...",
                                "It worked fine!",
                                "\"I'm a ghost, not a vampire...\""
                        });
                    break;
                    case (int)AttackActions.Theremin:
                        DamageEnemy(3);
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
                        toScroll.AddRange(new string[]{
                            "You tried to talk to the ghost...",
                            "The ghost wouldn't respond..."
                        });
                    break;
                    case (int)TalkEnum.ChatTwo:
                        toScroll.AddRange(new string[]{
                            "You tried to have a discussion with the ghost...",
                            "The ghost didn't seem to care..."
                        });
                    break;
                    case (int)TalkEnum.Fake_Throw:
                        toScroll.AddRange(new string[]{
                            "You threw something that didn't exist...",
                            "The ghost seemed bored..."
                        });
                    break;
                    case (int)TalkEnum.Flirt:
                        toScroll.AddRange(new string[]{
                            "You tried to flirt with the ghost...",
                            "The ghost doesn't react..."
                        });
                    break;
                    case (int)TalkEnum.Pet:
                        toScroll.AddRange(new string[]{
                            "You tried to pet the ghost...",
                            "The ghost recoils..."
                        });
                    break;
                    case (int)TalkEnum.Call_Him_Bald:
                        toScroll.AddRange(new string[]{
                            "You called the ghost bald...",
                            "The ghost seems confused..."
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
                        "What's wrong with you?"});
                    break;
                    case (int)ItemsEnum.Photo:
                        toScroll.AddRange(new string[] {
                        "You held the Photo out to the being...",
                        "But it seems it cannot see it..."});
                    break;
                    case (int)ItemsEnum.Teddy_Bear:
                        toScroll.AddRange(new string[] {
                        "You held the Teddy Bear out to the being...",
                        "But it ignores it..."});
                    break;
                    case (int)ItemsEnum.Russian_Doll:
                        toScroll.AddRange(new string[] {
                        "You held the Russian Doll out to the being...",
                        "But it doesn't get a reaction..."});
                    break;
                    case (int)ItemsEnum.Eraser:
                        toScroll.AddRange(new string[] {
                        "You held the Eraser out to the being...",
                        "But it seems disinterested..."});
                    break;
                    case (int)ItemsEnum.Spinning_Toy:
                        toScroll.AddRange(new string[] {
                        "You held the Spinning Toy out to the being...",
                        "But it doesn't seem to care..."});
                    break;
                    case (int)ItemsEnum.Scissors:
                        toScroll.AddRange(new string[] {
                        "You held out the Scissors...",
                        "Nothing happens."});
                    break;
                    default: Missing(actionType,actionID);break;
                }
            break;
            case ButtonEnum.Crucifix:
                //try to crucifix
                if (enemy.hp ==0){
                    //enemy banished
                    battleEnded = (int)endCon.CRUCIFIX;
                    DisableOnEnd();
                    //add text about crucifix 
                    switch(Random.Range(0,4)){
                        case 0: toScroll.AddRange(new string[]{"The ghost withers away in a blinding flash!"});
                        break;
                        case 1: toScroll.AddRange(new string[]{"The ghost's face contorts and melts away into nothingness!"});
                        break;
                        case 2: toScroll.AddRange(new string[]{"The ghost wails as it disintegrates!"});
                        break;
                        case 3: toScroll.AddRange(new string[]{"The ghost lets out a scream as its torn apart!"});
                        break;
                    }
                   
                }else{
                    switch(Random.Range(0,3)){
                        case 0: toScroll.AddRange(new string[]{"The ghost seems terrified by your crucifix.","You can't banish the ghost yet, it's still too strong!"});
                        break;
                        case 1: toScroll.AddRange(new string[]{"The ghost cowers at the sight of your crucifix.","You'll need to weaken the ghost more before using it!"});
                        break;
                        case 2: toScroll.AddRange(new string[]{" The ghost's form wavers, but the it's desperately trying to hold on!","You have to weaken it more before using the crucifix!"});
                        break;
                    }
                    
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
                        DisableOnEnd();
                        toScroll.AddRange( new string[]{"You got away safely..."});
                    }else{
                        //add text about not being able to run
                        toScroll.AddRange( new string[]{"You couldn't get away!"});
                    }
                }else{
                    toScroll.AddRange( new string[]{"You can't get away from this enemy!","If you want to leave, you'll have to deal with it first!"});
                }
            break;
        }
    }
}
