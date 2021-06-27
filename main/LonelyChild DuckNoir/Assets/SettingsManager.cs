using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    float[] options;
    void Awake(){
        Load();
    }

    public void Defaults(){
        options = new float[7];
        options[0] = 0.3f;
        options[1] = 0.4f;
        options[2] = 0.3f;
        options[3] = 0.3f;
        options[4] = 0.8f;
        options[5] = 0.8f;
        options[6] = 0.5f;
    }
    public void Load(){
        options = new float[7];
        //TODO: add default values
        options[0] = PlayerPrefs.GetFloat("hsmooth",0.3f);
        options[1] = PlayerPrefs.GetFloat("vsmooth",0.4f);
        options[2] = PlayerPrefs.GetFloat("hsensitivity",0.3f);
        options[3] = PlayerPrefs.GetFloat("vsensitivity",0.3f);
        options[4] = PlayerPrefs.GetFloat("musicVol",0.8f);
        options[5] = PlayerPrefs.GetFloat("sfxVol",0.8f);
        options[6] = PlayerPrefs.GetFloat("camSmooth",0.5f);
    }
    public void UpdatePlayer(){
        ThirdPersonPlayer playerObj = GameObject.FindObjectOfType<ThirdPersonPlayer>();
        if (playerObj != null){
            Debug.Log(playerObj.ToString());
            Debug.Log("Player found, updating player options");
            playerObj.UpdateOptions(options);
        }else{
            Debug.Log("no player found to update options");
        }
    }
    public void ChangeOptions(float[] inputOptions){
        options = inputOptions; 
        UpdatePlayer();

        SoundHolder[] sounds = GameObject.FindObjectsOfType<SoundHolder>();
        for (int i = 0; i < sounds.Length; i++){
            if (sounds[i].IsMusic()){
                sounds[i].SetVol(options[4]);
            }else{
                sounds[i].SetVol(options[5]);
            }
        }

        PlayerPrefs.SetFloat("hsmooth",options[0]);
        PlayerPrefs.SetFloat("vsmooth",options[1]);
        PlayerPrefs.SetFloat("hsensitivity",options[2]);
        PlayerPrefs.SetFloat("vsensitivity",options[3]);
        PlayerPrefs.SetFloat("musicVol",options[4]);
        PlayerPrefs.SetFloat("sfxVol",options[5]);
        PlayerPrefs.SetFloat("camSmooth",options[6]);
    }

    public float[] GetOptions(){
        if (options==null){
            Load();
        }
        return options;
    }
    
}
