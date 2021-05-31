using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Combat;


public class battleBehavior : MonoBehaviour
{
    TextManager tm;
    public Enemy enemy;
    //Our enemy.
    public Submenu subMenu;
    //The submenu position.
    public GamePosition gamePosition;
    //Where we are in the turn.
    public GameObject textBox;
    //The textbox at the top of the screen.
    public GameObject playerChoiceBox;
    //The textbox at the top of the screen.
    public TextMesh fillerText;
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(theScroll = TextScroll(enemy.name + " manifests into view!"));
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        subMenu = GameObject.Find("SubmenuPanel").GetComponent<Submenu>();
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
                        //click.Play();
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
        //handleSubmenu();
    }
    /*void handleSubmenu()
    {

        switch (submenuPosition)
        {
            case SubmenuPosition.Regular:
                updateButtonVisuals("Attack", "Talk", "Items", "Run", "Crucifix");
                break;
            case SubmenuPosition.Attack:
                updateButtonVisuals(enemy.attackChoices[0], enemy.attackChoices[1], enemy.attackChoices[2], enemy.attackChoices[3], enemy.attackChoices[4]);
                break;
            case SubmenuPosition.Talk:
                updateButtonVisuals(enemy.talkChoices[0], enemy.talkChoices[1], enemy.talkChoices[2], enemy.talkChoices[3], enemy.talkChoices[4]);
                break;
            case SubmenuPosition.Inventory:
                updateButtonVisuals(enemy.itemsChoices[0], enemy.itemsChoices[1], enemy.itemsChoices[2], enemy.itemsChoices[3], enemy.itemsChoices[4]);
                break;
            default:
                print("You put bad data in the handleSubmenu, my guy. Cringe.");
                break;
        }
    }*/

    public void ExternalSubButtonPressed(int actionID){
        SubButtonPressed(currentAction ?? (ButtonEnum)0, actionID);
    }

    void SubButtonPressed(ButtonEnum actionType, int actionID)
    {
        
        gamePosition = GamePosition.EnemyDialogue;
        playerChoiceBox.SetActive(false);
        textBox.SetActive(true);
        currentLine = 0;
        int thisEnemyID = enemy.IDBase;
        ExitSubmenu();
        enemy.toScroll = GetEnemyTextByID(thisEnemyID,actionType,actionID);

        /*switch (submenuPosition)
        {
            case SubmenuPosition.Attack:
                //enemy.toScroll = enemy.attackDialogue[whichButton - 1].text.Split('\n');
                enemy.toScroll = tm.GetEnemyTextByID(thisEnemyID + whichButton - 1);
                endAtLine = enemy.toScroll.Length - 1;
                break;
            case SubmenuPosition.Talk:
                //enemy.toScroll = enemy.talkDialogue[whichButton - 1].text.Split('\n');
                endAtLine = enemy.toScroll.Length - 1;
                break;
            case SubmenuPosition.Inventory:
                //Waiting for inventory system to be made
                break;
            case SubmenuPosition.Regular:
                break;
            default:
                print("you passed bad data into Enemy Dialogue");
                break;
        }*/
        StopCoroutine(theScroll);
        StartCoroutine(theScroll = TextScroll(enemy.toScroll[currentLine]));
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
        gamePosition = GamePosition.EnemyAttack;
        playerChoiceBox.SetActive(false);
        textBox.SetActive(false);
        print("wediditreditt");
    }

    void NotSetUp(){
        Debug.Log("action ID not set up in battleBehavior");
    }

    public string[] GetEnemyTextByID(int enemyID, ButtonEnum actionType, int actionID){//using switch because no loaded memory and fast
        switch(actionType){
            case ButtonEnum.Attack:
                switch(actionID){
                    case (int)AttackActions.Punch:
                        //execute what punch does
                        return TextManager.stringsToArray("punch used");
                    case (int)AttackActions.Shout:
                        //execute what punch does
                        return TextManager.stringsToArray("shout used");
                    case (int)AttackActions.Slap://in the case where you want a specific interaction:
                        switch(enemyID){
                            case 0: //if the target is test enemy 0...
                                //do something specific to this enemy
                                return new string[] {"That did nothing"};
                            default:
                                //execute what slap does
                                return TextManager.stringsToArray("slap used");
                        }
                    default: NotSetUp(); return new string[] {"..."};
                }
            case ButtonEnum.Talk:
                //talk
                return new string[]{"talking"};
            case ButtonEnum.Items:
                //use item
                return new string[]{"using an item"};
            case ButtonEnum.Crucifix:
                //try to run
                return new string[]{"using cruifix"};
            case ButtonEnum.Run:
                //try to run
                return new string[]{"running"};
            default: return new string[]{""};//should never be reached
        }
    }
}
