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
        PlayerPrefs.DeleteAll();
        Load();
    }
    public void Load(){
        options = new float[11];
        //TODO: add default values
        options[0] = PlayerPrefs.GetFloat("hsmooth",0.3f);
        options[1] = PlayerPrefs.GetFloat("vsmooth",0.4f);
        options[2] = PlayerPrefs.GetFloat("hsensitivity",0.7f);
        options[3] = PlayerPrefs.GetFloat("vsensitivity",0.7f);
        options[4] = PlayerPrefs.GetFloat("musicVol",0.5f);
        options[5] = PlayerPrefs.GetFloat("sfxVol",0.7f);
        options[6] = PlayerPrefs.GetFloat("camSmooth",0.4f);
        options[7] = PlayerPrefs.GetFloat("OverShoulder",1f);
        options[8] = PlayerPrefs.GetFloat("PosSmooth",.4f);
        options[9] = PlayerPrefs.GetFloat("RotSmooth",.4f);
        options[10] = PlayerPrefs.GetFloat("ambientVol",0.8f);

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

        UpdateSounds();

        PlayerPrefs.SetFloat("hsmooth",options[0]);
        PlayerPrefs.SetFloat("vsmooth",options[1]);
        PlayerPrefs.SetFloat("hsensitivity",options[2]);
        PlayerPrefs.SetFloat("vsensitivity",options[3]);
        PlayerPrefs.SetFloat("musicVol",options[4]);
        PlayerPrefs.SetFloat("sfxVol",options[5]);
        PlayerPrefs.SetFloat("camSmooth",options[6]);
        PlayerPrefs.SetFloat("OverShoulder",options[7]);
        PlayerPrefs.SetFloat("PosSmooth",options[8]);
        PlayerPrefs.SetFloat("RotSmooth",options[9]);
        PlayerPrefs.SetFloat("ambientVol",options[10]);
    }

    public void ChangeOptionSingular(int option, float value){
        if (option == 4 || option == 5){
            options[option] = value;
            UpdateSounds();
        }
    }
    public void UpdateSounds(){
        SoundHolder[] sounds = GameObject.FindObjectsOfType<SoundHolder>();
        for (int i = 0; i < sounds.Length; i++){
            if (sounds[i].IsMusic()){
                sounds[i].SetVol(options[4]);
            }else{
                if (sounds[i].IsAmbient()){
                    sounds[i].SetVol(options[10]);
                }else{
                sounds[i].SetVol(options[5]);
                }
            }
        }
    }

    public float[] GetOptions(){
        if (options==null){
            Load();
        }
        return options;
    }
    
}
