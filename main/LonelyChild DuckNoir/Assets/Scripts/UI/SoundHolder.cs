using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHolder : MonoBehaviour
{
    //this script allows you to assign audioclips to different actions.
    //You can make these sounds play by placing an event trigger on UI buttons and other things.
    [SerializeField] public AudioSource audioSource;
    [SerializeField] protected AudioClip[] sounds;
    [SerializeField] protected bool isMusic = false;
    [SerializeField] protected bool isAmbient = false;//TODO Add some settings for this
    void Start(){
        SettingsManager manager = GameObject.FindObjectOfType<SettingsManager>();
        float volume = 1f;
        if (manager!=null){
            if (isMusic){
                volume = manager.GetOptions()[4];
            }else{
                if(isAmbient){
                    volume = manager.GetOptions()[10];
                }else{
                    volume = manager.GetOptions()[5];
                }
            }
        }
        audioSource.volume = volume;
    }
    public void SetVol(float vol){
        Debug.Log("setting volume:"+vol.ToString());
        audioSource.volume = vol;
    }
    public bool IsMusic(){
        return isMusic;
    }
    public bool IsAmbient(){
        return isAmbient;
    }
    public void Play(){
        audioSource.Play();
    }
}
