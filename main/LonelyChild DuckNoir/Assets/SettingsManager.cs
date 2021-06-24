using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    float[] options;
    void Awake(){
        
    }

    void Start(){
        options = new float[4];
        //TODO: add default values
        options[0] = PlayerPrefs.GetFloat("hsmooth",0.3f);
        options[1] = PlayerPrefs.GetFloat("vsmooth",0.4f);
        options[2] = PlayerPrefs.GetFloat("hsensitivity",0.3f);
        options[3] = PlayerPrefs.GetFloat("vsensitivity",0.3f);
        //0 hsmoothing
        //1 vsmoothing
        //2 hsensitivity
        //3 vsensitivity
    }
    public void ChangeOptions(float[] inputOptions){
        options = inputOptions;
        //Update player 
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null){
            Debug.Log(playerObj.ToString());
            Debug.Log("Player found, updating player options");
            playerObj.GetComponent<ThirdPersonPlayer>().UpdateOptions(options);
        }else{
            Debug.Log("no player found to update options");
        }
        PlayerPrefs.SetFloat("hsmooth",options[0]);
        PlayerPrefs.SetFloat("vsmooth",options[1]);
        PlayerPrefs.SetFloat("hsensitivity",options[2]);
        PlayerPrefs.SetFloat("vsensitivity",options[3]);
    }

    public float[] GetOptions(){
        Debug.Log(options);
        return options;
    }
    
}
