using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSounds : SoundHolder
{
    public void Tick(){
        audioSource.PlayOneShot(sounds[0]);
    }
    public void Click(){
        audioSource.PlayOneShot(sounds[1]);
    }
    public void Damage(){
        audioSource.PlayOneShot(sounds[2]);
    }
}
