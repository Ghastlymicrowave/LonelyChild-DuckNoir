using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    GameSceneManager sceneManager;
    void Start(){
        sceneManager = GameObject.Find("PersistentManager").GetComponent<GameSceneManager>();
    }
    public void Unpause(){
        sceneManager.Unpause();
    }
    public void Quit(){
        Application.Quit();
    }

    public void Title(){
        sceneManager.Title();
    }
}
