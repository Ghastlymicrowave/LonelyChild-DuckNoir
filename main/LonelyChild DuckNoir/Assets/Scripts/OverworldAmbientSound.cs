using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldAmbientSound : SoundHolder
{
    // contains random sounds to be played at random, possibly with varrying pitch
    [SerializeField] float minPitch = .9f;
    [SerializeField] float maxPitch = 1.1f;
    [SerializeField] float minDelay = 5f;
    [SerializeField] float maxDelay = 5f;
    float timer;
    void Start(){
        timer = Random.Range(minDelay,maxDelay);
    }
    void Update(){
        timer -= Time.deltaTime;
        if (timer<=0){
            timer = Random.Range(minDelay,maxDelay);
            PlayRandom();
        }
    }
    public void PlayRandom(){
        audioSource.pitch = Random.Range(minPitch,maxPitch);
        audioSource.PlayOneShot(sounds[Random.Range(0,sounds.Length)]);
    }
}
