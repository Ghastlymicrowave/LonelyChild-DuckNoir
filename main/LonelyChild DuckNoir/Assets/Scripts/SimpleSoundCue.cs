using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSoundCue : SoundHolder
{
    // Start is called before the first frame update
    public void Trigger(){
        audioSource.Play();
    }
    public void SetSound(int snd){
        snd = Mathf.Clamp(snd,0,sounds.Length-1);
        audioSource.clip = sounds[snd];
    }
}
