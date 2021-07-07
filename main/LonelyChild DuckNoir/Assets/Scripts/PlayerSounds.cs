using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : SoundHolder
{
    // Start is called before the first frame update
    bool walking;
    [SerializeField] float minPitch = .9f;
    [SerializeField] float maxPitch = 1.1f;
    [SerializeField] float minDelay = 5f;
    [SerializeField] float maxDelay = 5f;
    float timer;
    public void BeginFootseps(){
        if (!walking){
            walking = true;
            timer = Random.Range(minDelay,maxDelay);
        }
    }
    public void EndFootsteps(){
        walking = false;
    }

    void Update(){
        if (walking){
            timer -= Time.deltaTime;
            if (timer<=0){
                timer = Random.Range(minDelay,maxDelay);
                audioSource.pitch = Random.Range(minPitch,maxPitch);
                audioSource.PlayOneShot(sounds[Random.Range(0,sounds.Length)]);
            }
        }
    }
}
