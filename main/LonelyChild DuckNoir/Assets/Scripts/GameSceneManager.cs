using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    GameObject overworld;
    InventoryManager inventoryManager;
    EnemyManager enemyManager;
    [SerializeField] AudioClip[] combatAudio;
    [SerializeField] AudioClip[] overworldAudio;
    public string combatSceneName = "CombatScene";
    public string pauseSceneName = "PauseScene";
    public bool loadCheckpoint = false;

    public int currentLevel = 0;

    public AudioClip GetCombatAudio(){//0, 1, 2 
        return combatAudio[currentLevel];
    }
    public AudioClip GetOverworldAudio(){//0, 1, 2 
        return overworldAudio[currentLevel];
    }

    void Awake(){
        inventoryManager = GetComponent<InventoryManager>();
        
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
        loadCheckpoint =true;
        inventoryManager.LoadJSON();
        SceneManager.LoadScene(inventoryManager.checkpointScene);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadCheckpoint){
            PackEverything();
            loadCheckpoint = false;
            enemyManager = GameObject.Find("Enemy Handler").GetComponent<EnemyManager>();
        }

        switch(scene.name){
            case "FirstFloor": currentLevel = 0; break;
            case "SecondFloor": currentLevel = 1; break;
            case "Basement": currentLevel = 2; break;
            default: break;
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
