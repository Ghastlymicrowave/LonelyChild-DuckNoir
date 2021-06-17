using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    GameObject overworld;
    InventoryManager inventoryManager;
    public string combatSceneName = "CombatScene";
    public string pauseSceneName = "PauseScene";

    void Awake(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void PackEverything(){
        if (GameObject.Find("overworld")==null){
            overworld = Instantiate(new GameObject(), Vector3.zero,Quaternion.identity);
            overworld.name = "overworld";
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
            foreach(GameObject go in allObjects)
            if (go !=null && go != this.gameObject && go.transform.parent==null && go != overworld && go){
                go.transform.SetParent(overworld.transform);
            }
        }
    }
    void Start(){
        inventoryManager = GetComponent<InventoryManager>();
    }

    public void TransitionScene(string sceneName){
        inventoryManager.SaveJSON();
        SceneManager.LoadScene(sceneName);
    }
    public void LoadCheckpoint(string sceneName){
        inventoryManager.LoadJSON();
        SceneManager.LoadScene(sceneName);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PlayroomWB"){
            PackEverything();
        }
        inventoryManager.LoadJSON();
    }
    public void EnterCombat(){
        SceneManager.LoadScene(combatSceneName,LoadSceneMode.Additive);
        overworld.SetActive(false);
    }
    public void ExitCombat(){
        SceneManager.UnloadSceneAsync("CombatScene");
        overworld.SetActive(true);
    }

    public void Pause(){
        SceneManager.LoadScene(pauseSceneName,LoadSceneMode.Additive);
        overworld.SetActive(false);
    }
    public void Unpause(){
        SceneManager.UnloadSceneAsync("PauseScene");
        overworld.SetActive(true);
    }

    public void GameOver(){
        inventoryManager.Reset();
        Destroy(GameObject.Find("CombatScene"));
        SceneManager.LoadScene("GameOver");
    }

    public void Title(){
        SceneManager.LoadScene("MainMenu");
    }
}
