using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerLogic : MonoBehaviour
{
    //This script sets the lights to a given value, index or health.
    public GameObject[] lights;
    public Animator animator;
    

    public void DecideLights(int health, int maxHealth)
    {

        if (maxHealth == health)
        {
            changeLights(6);

        }
        else if (health == 0)
        {
            animator.Play("ScannerMove");
            changeLights(4);

        }
        else
        {
            animator.Play("ScannerIdle");
            float ratio = (((float)health) / ((float)maxHealth));
            print("retard");
            //Switch doesn't have contitional functionality, so this is the only solution I can think of.
            //Feels bad, man.
            if (ratio > .16 && ratio <= .32)
            {
                changeLights(0);
            }

            else if (ratio > .32 && ratio <= .48)
            {

                changeLights(1);
            }
            else if (ratio > .48 && ratio <= .64)
            {
                changeLights(2);
            }
            else if (ratio > .64 && ratio <= .9999)
            {
                changeLights(3);
            }
        }

    }
    public void changeLights(int index)
    {
        foreach (GameObject lig in lights)
        {
            lig.SetActive(false);
        }
        if (index != 6)
        {

            while (index > -1)
            {
                lights[index].SetActive(true);
                index -= 1;
            }

        }
    }
}
