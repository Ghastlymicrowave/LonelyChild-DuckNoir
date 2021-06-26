using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubmenuSoundStuff : EventTrigger
{
    public GameObject soundHolderGO;
    public MenuSounds soundHolder;
    //public EventTrigger trigger;
    void Start()
    {
        soundHolder = FindObjectOfType<MenuSounds>();
        if (soundHolder==null){
            Debug.LogWarning("submenu sounds is missing a MenuSounds component in the scene! Prepare for null refs!");
        }
    }
    public override void OnPointerClick(PointerEventData data)
    {
        soundHolder.Click();
    }
    public override void OnPointerEnter(PointerEventData data)
    {
        soundHolder.Hover();
    }


    
}
