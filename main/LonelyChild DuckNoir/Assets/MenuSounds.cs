using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : SoundHolder
{   
//0 hover
//1 click
    public void Hover()
    {
        audioSource.PlayOneShot(sounds[0]);
    }
    public void Click()
    {
        audioSource.PlayOneShot(sounds[1]);
    }
}
