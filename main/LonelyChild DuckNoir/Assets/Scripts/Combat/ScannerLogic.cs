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
            changeLights(Mathf.FloorToInt(health*lights.Length/maxHealth)*lights.Length);
        }
    }
    public void changeLights(int index)
    {
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
            index -= 1;
        }
    }
}
