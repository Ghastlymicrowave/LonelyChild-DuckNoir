using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class battleBehavior : MonoBehaviour
{
    public Enemy enemy;
    //Our enemy

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonPress(int buttonNum)
    {
        print("BUTTON PRESS " + buttonNum.ToString());
        switch (buttonNum)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                print("You sent bad data to buttonPress, dummy.");
                break;
        }
    }
}
