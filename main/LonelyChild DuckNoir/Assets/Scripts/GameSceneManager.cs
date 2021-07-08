using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    Camera overworldCam;
    GameObject overworld;
    InventoryManager inventoryManager;
    SettingsManager settings;
    EnemyManager enemyManager;
    [SerializeField] AudioClip[] combatAudio;
    [SerializeField] AudioClip[] overworldAudio;
    public string combatSceneName = "CombatScene";
    public string pauseSceneName = "PauseScene";
    public int currentLevel = 0;
    bool newScene = false;

    public AudioClip GetCombatAudio(){//0, 1, 2 
        return combatAudio[currentLevel];
    }
    public AudioClip GetOverworldAudio(){//0, 1, 2 
        return overworldAudio[currentLevel];
    }

    void Awake(){
        inventoryManager = GetComponent<InventoryManager>();
        settings = GetComponent<SettingsManager>();
    }
    void Start(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name!="MainMenu"){
            PackEverything();   
        }
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
        inventoryManager.SaveJSON(sceneName);
        SceneManager.LoadScene(sceneName);
        newScene = true;
    }
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    public string SceneName(){
        return SceneManager.GetActiveScene().name;
    }
    public void LoadCheckpoint(){
        inventoryManager.LoadJSON();
        Debug.Log(inventoryManager.checkpointScene);
        SceneManager.LoadScene(inventoryManager.checkpointScene);
        newScene = true;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("loading scene: "+ scene.name);
        if (scene.name == "FirstFloor" || scene.name == "SecondFloor" || scene.name == "Basement"){
            Debug.Log("packing scene");
            PackEverything();
            enemyManager = GameObject.Find("Enemy Handler").GetComponent<EnemyManager>();
            overworldCam = GameObject.Find("MainCam").GetComponent<Camera>();
        }

        SettingsManager manager = GetComponent<SettingsManager>();
        manager.Invoke("UpdateSounds",1f);

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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (overworldCam !=null){
            overworldCam.gameObject.SetActive(false);
        }
    }
    public void ExitCombat(){
        SceneManager.UnloadSceneAsync("CombatScene");
        overworld.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.FindObjectOfType<EnemyManager>().ReLoaded();
        overworldCam.gameObject.SetActive(true);

    }

    public void Pause(){
        SceneManager.LoadScene(pauseSceneName,LoadSceneMode.Additive);
        overworld.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Unpause(){
        SceneManager.UnloadSceneAsync("PauseScene");
        overworld.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        settings.UpdatePlayer();
        settings.UpdateSounds();
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
