using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    GameSceneManager sceneManager;
    [SerializeField] OptionsMenu options;
    void Start(){
        sceneManager = GameObject.Find("PersistentManager").GetComponent<GameSceneManager>();
    }

    void Update(){
        if (Input.GetButtonDown("Pause")){
            Unpause();
        }
    }
    public void Unpause(){
        CloseIfOpen();
        sceneManager.Unpause();
    }
    public void Quit(){
        Application.Quit();
    }

    public void Title(){
        sceneManager.Title();
    }

    public void Options(){
        options.gameObject.SetActive(true);
        options.InitMenu();
    }

    void CloseIfOpen(){
        if (options != null && options.gameObject.activeSelf)
        {
            options.CloseOptions();
        }
    }
}
