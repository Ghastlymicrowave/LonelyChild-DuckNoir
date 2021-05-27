using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleButton : MonoBehaviour
{
    //Call buttonpress on battlebehavior when clicked.
    public battleBehavior bb;
    public int buttonNum;
    void OnMouseDown()
    {
        bb.ButtonPress(buttonNum);
    }
}
