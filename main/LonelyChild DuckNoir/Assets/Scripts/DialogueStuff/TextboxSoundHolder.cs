using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextboxSoundHolder : SoundHolder
{
    //0 tick 1 appear
    public void TextTick(){
        audioSource.PlayOneShot(sounds[0]);
    }
    public void TextApear(){
        audioSource.PlayOneShot(sounds[1]);
    }
}
