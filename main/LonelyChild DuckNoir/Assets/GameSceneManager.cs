using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    GameObject overworld;
    public string combatSceneName = "combatTest";
    void Start()
    {
        overworld = Instantiate(new GameObject(), Vector3.zero,Quaternion.identity);
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
        foreach(GameObject go in allObjects)
            if (go != this.gameObject && go.transform.parent==null && go != overworld && go){
                go.transform.parent = overworld.transform;
            }
    }

    public void EnterCombat(){
        SceneManager.LoadScene(combatSceneName,LoadSceneMode.Additive);
        overworld.SetActive(false);
    }
    public void ExitCombat(){
        Destroy(GameObject.Find("CombatScene"));
        overworld.SetActive(true);
    }

    public void GameOver(){
        Destroy(GameObject.Find("CombatScene"));
        SceneManager.LoadScene("GameOver");
    }
}
