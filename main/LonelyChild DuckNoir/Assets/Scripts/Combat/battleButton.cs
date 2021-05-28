using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class battleButton : MonoBehaviour
{
    //Call buttonpress on battlebehavior when clicked.
    public battleBehavior bb;
    public ButtonEnum buttonNum;
    public SpriteRenderer sr;
    void OnMouseOver()
    {
        sr.color = Color.grey;
    }
    void OnMouseExit()
    {
        sr.color = Color.white;
    }
    void OnMouseDown()
    {
        bb.ButtonPress(buttonNum);
    }
}
