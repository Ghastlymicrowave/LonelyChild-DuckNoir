using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubmenuSoundStuff : EventTrigger
{
    public GameObject soundHolderGO;
    public SoundHolder soundHolder;
    public EventTrigger trigger;

    void Start()
    {
        if (soundHolderGO == null)
        {
            soundHolderGO = GameObject.Find("SoundHolder");
        }
        soundHolder = soundHolderGO.GetComponent<SoundHolder>();
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
