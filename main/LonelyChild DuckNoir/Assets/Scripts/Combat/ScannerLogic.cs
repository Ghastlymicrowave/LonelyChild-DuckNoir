using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerLogic : MonoBehaviour
{
    //This script sets the lights to a given value, index or health.
    public GameObject[] lights;
    public Animator animator;


    public void DecideLights(int lights)
    {
        //Debug.Log(health.ToString() +"/"+ maxHealth.ToString() + "length"+lights.Length.ToString());

        if (lights==5)
        {
            animator.Play("ScannerMove");
            changeLights(lights-1);

        }
        else
        {
            animator.Play("ScannerChange");
            changeLights(lights-1);
        }
    }
    public void changeLights(int index)
    {
        Debug.Log("change lights, "+index.ToString());
        foreach (GameObject lig in lights)//disables all lights
        {
            lig.SetActive(false);
        }
        if (index >= lights.Length){
            Debug.Log("Changing more lights than exist as game objects");
            index = lights.Length-1;
        }
        while (index > -1)
        {
            lights[index].SetActive(true);
            Debug.Log("changing index "+index.ToString());
            index -= 1;
            
        }
    }
}
