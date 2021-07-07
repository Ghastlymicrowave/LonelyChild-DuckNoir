using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSoundCue : SoundHolder
{
    // Start is called before the first frame update
    public void Trigger(){
        int snd = Random.Range(0,sounds.Length);
        audioSource.clip = sounds[snd];
        audioSource.PlayOneShot(audioSource.clip);
        Debug.Log("playing");
    }
    public void SetSound(int snd){
        snd = Mathf.Clamp(snd,0,sounds.Length-1);
        audioSource.clip = sounds[snd];
    }
}
