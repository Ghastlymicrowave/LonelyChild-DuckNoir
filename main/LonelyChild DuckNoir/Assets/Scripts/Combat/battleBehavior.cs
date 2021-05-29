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
    public SubmenuPosition submenuPosition;
    //The submenu position.
    public GamePosition gamePosition;
    //Where we are in the turn.
    public GameObject textBox;
    //The textbox at the top of the screen.
    public GameObject playerChoiceBox;
    //The textbox at the top of the screen.
    public TextMesh fillerText;
    //The text for the text at the top.
    public TextMesh[] buttonText;
    //The text assets for the buttons.
    public string selectedMove;
    //the name of what the player selected.
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
    public int whichButton;
    //which of the five buttons did we press, as an enum.
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
        click.Play();
        switch (buttonNum)
        {
            case ButtonEnum.One:
                button1();
                whichButton = 1;
                break;
            case ButtonEnum.Two:
                button2();
                whichButton = 2;
                break;
            case ButtonEnum.Three:
                button3();
                whichButton = 3;
                break;
            case ButtonEnum.Four:
                button4();
                whichButton = 4;
                break;
            case ButtonEnum.Five:
                button5();
                whichButton = 5;
                break;
            default:
                print("You sent bad data to buttonPress, dummy.");
                break;
        }
    }

    void button1()
    {
        //This, and the next four functions, are called from buttonpress when a button is pressed.
        //They represent the buttons of player choices from left to right.
        handleSubmenu();
        switch (submenuPosition)
        {
            case SubmenuPosition.Regular:
                submenuPosition = SubmenuPosition.Attack;
                break;
            case SubmenuPosition.Attack:
                selectedMove = buttonText[0].text;
                EnemyDialogue();
                break;
            case SubmenuPosition.Talk:
                selectedMove = buttonText[0].text;
                EnemyDialogue();
                break;
            case SubmenuPosition.Inventory:
                selectedMove = buttonText[0].text;
                EnemyDialogue();
                break;
        }
        handleSubmenu();
    }
    void button2()
    {
        handleSubmenu();
        switch (submenuPosition)
        {
            case SubmenuPosition.Regular:
                submenuPosition = SubmenuPosition.Talk;
                break;
            case SubmenuPosition.Attack:
                selectedMove = buttonText[1].text;
                EnemyDialogue();
                break;
            case SubmenuPosition.Talk:
                selectedMove = buttonText[1].text;
                EnemyDialogue();
                break;
            case SubmenuPosition.Inventory:
                selectedMove = buttonText[1].text;
                EnemyDialogue();
                break;
        }
        handleSubmenu();
    }
    void button3()
    {
        handleSubmenu();
        switch (submenuPosition)
        {
            case SubmenuPosition.Regular:
                submenuPosition = SubmenuPosition.Inventory;
                break;
            case SubmenuPosition.Attack:
                selectedMove = buttonText[2].text;
                EnemyDialogue();
                break;
            case SubmenuPosition.Talk:
                selectedMove = buttonText[2].text;
                EnemyDialogue();
                break;
            case SubmenuPosition.Inventory:
                selectedMove = buttonText[2].text;
                EnemyDialogue();
                break;
        }
        handleSubmenu();
    }
    void button4()
    {
        handleSubmenu();
        switch (submenuPosition)
        {
            case SubmenuPosition.Regular:
                selectedMove = "Run";
                EnemyDialogue();
                break;
            case SubmenuPosition.Attack:
                selectedMove = buttonText[3].text;
                EnemyDialogue();
                break;
            case SubmenuPosition.Talk:
                selectedMove = buttonText[3].text;
                EnemyDialogue();
                break;
            case SubmenuPosition.Inventory:
                selectedMove = buttonText[3].text;
                EnemyDialogue();
                break;
        }
        handleSubmenu();
    }
    void button5()
    {
        handleSubmenu();
        switch (submenuPosition)
        {
            case SubmenuPosition.Regular:
                selectedMove = "Crucifix";
                EnemyDialogue();
                break;
            case SubmenuPosition.Attack:
                submenuPosition = SubmenuPosition.Regular;
                break;
            case SubmenuPosition.Talk:
                submenuPosition = SubmenuPosition.Regular;
                break;
            case SubmenuPosition.Inventory:
                submenuPosition = SubmenuPosition.Regular;
                break;
        }
        handleSubmenu();
    }

    void beginTurn()
    {
        //The logic for beginning a turn.
        gamePosition = GamePosition.PlayerChoice;
        submenuPosition = SubmenuPosition.Regular;
        playerChoiceBox.SetActive(true);
        textBox.SetActive(true);
        handleSubmenu();
    }
    void handleSubmenu()
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
    }
    void updateButtonVisuals(string a, string b, string c, string d, string e)
    {
        buttonText[0].text = a;
        buttonText[1].text = b;
        buttonText[2].text = c;
        buttonText[3].text = d;
        buttonText[4].text = e;
    }

    void EnemyDialogue()
    {
        
        gamePosition = GamePosition.EnemyDialogue;
        playerChoiceBox.SetActive(false);
        textBox.SetActive(true);
        currentLine = 0;
        int TheID = enemy.IDBase;

        switch (submenuPosition)
        {
            case SubmenuPosition.Attack:
                //enemy.toScroll = enemy.attackDialogue[whichButton - 1].text.Split('\n');
                enemy.toScroll = tm.GetEnemyTextByID(TheID + whichButton - 1);
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
        }
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


}
