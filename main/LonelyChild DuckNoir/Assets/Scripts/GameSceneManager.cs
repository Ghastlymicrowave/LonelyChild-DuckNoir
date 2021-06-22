using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    GameObject overworld;
    InventoryManager inventoryManager;
    EnemyManager enemyManager;
    public string combatSceneName = "CombatScene";
    public string pauseSceneName = "PauseScene";
    public bool loadCheckpoint = false;

    void Awake(){
        inventoryManager = GetComponent<InventoryManager>();
        enemyManager = GameObject.Find("Enemy Handler").GetComponent<EnemyManager>();
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

    public void TransitionScene(string sceneName){
        loadCheckpoint = true;
        inventoryManager.SaveJSON();
        SceneManager.LoadScene(sceneName);
    }
    public void LoadInitalScene(string sceneName){
        loadCheckpoint =true;
        inventoryManager.LoadJSON();
        SceneManager.LoadScene(inventoryManager.checkpointScene);
    }
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    public string SceneName(){
        return SceneManager.GetActiveScene().name;
    }
    public void LoadCheckpoint(){
        inventoryManager.LoadJSON();
        SceneManager.LoadScene(inventoryManager.checkpointScene);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadCheckpoint){
            PackEverything();
            loadCheckpoint = false;
        }
    }
    public void EnterCombat(){
        SceneManager.LoadScene(combatSceneName,LoadSceneMode.Additive);
        overworld.SetActive(false);
    }
    public void ExitCombat(){
        SceneManager.UnloadSceneAsync("CombatScene");
        overworld.SetActive(true);
        enemyManager.ReLoaded();
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
