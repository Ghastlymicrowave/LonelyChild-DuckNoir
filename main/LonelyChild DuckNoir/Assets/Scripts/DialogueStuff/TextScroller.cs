using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroller : MonoBehaviour
{
    //This is the script for overworld dialogue. A string array is given to this script, and it scrolls through each line.
    TextManager tm;
    string[] toScroll;
    player_main pm;

    bool isUpdating = false;
    public Text theText;
    public GameObject theCanvas;
    public Animator anim;
    public float typeSpeed = 0.035f;
    //How fast does text scroll?
    bool isTyping;
    bool cancelTyping;
    //Two variables to control text scrolling.
    public int currentLine;
    //the current line within the text scroll.
    public int endAtLine;
    //when to end the textscroll.
    public AudioSource tickSound;
    public AudioSource clickSound;

    void Start()
    {
        tm = new TextManager();
    }

    public void ScrollText(string[] newString, player_main pmReference)
    {
        print("aw yeah");
        pm = pmReference;
        pm.canMove = false;
        theCanvas.SetActive(true);
        toScroll = newString;
        currentLine = 0;
        endAtLine = toScroll.Length - 1;
        StartCoroutine(TextScroll(toScroll[currentLine]));
        
        isUpdating = true;
    }
    void Update()
    {
        if (isUpdating)
        {
            //Handling the clicking through of enemy dialogue, and starting of the enemy turn.
            if (Input.GetMouseButtonDown(0)||Input.GetButtonDown("Submit"))
            {
                if (!isTyping)
                {
                    currentLine += 1;
                    clickSound.Play();
                    if (currentLine > endAtLine)
                    {
                        CloseDialogue();
                        isUpdating = false;
                    }

                    else
                    {

                        StartCoroutine(TextScroll(toScroll[currentLine]));
                    }

                }
                else if (isTyping && !cancelTyping)
                {
                    cancelTyping = true;
                }
            }
        }
    }
    void CloseDialogue()
    {
        pm.canMove = true;
        isUpdating = false;
        toScroll = null;
        StartCoroutine(CloseIt());
    }
    private IEnumerator CloseIt()
    {
        anim.Play("PopDown");
        yield return new WaitForSeconds(.5f);
        theCanvas.SetActive(false);
    }

    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;

        while (isTyping && !cancelTyping && (letter < lineOfText.Length))
        {
            if ((lineOfText[letter] == ' ') || (lineOfText[letter] == '.'))
            { }
            else
            {
                tickSound.Play();
            }
            theText.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }

        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;

    }
}
