using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    float[] options;
    void Start(){
        options = new float[4];
        //TODO: add default values
        options[0] = 0.3f;
        options[1] = 0.3f;
        options[2] = 0.3f;
        options[3] = 0.3f;
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
            Debug.Log("Player found, updating player options");
            playerObj.GetComponent<ThirdPersonPlayer>().UpdateOptions(options);
        }else{
            Debug.Log("no player found to update options");
        }
        
    }

    public float[] GetOptions(){
        return options;
    }
    
}
