using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    TextManager tm;
    InventoryManager inventoryManager;
    [SerializeField] GameObject[] ghosts;
    EnemyBehavior[] ghostBehaviors;
    void Start()
    {
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        try{
            ghostBehaviors = new EnemyBehavior[ghosts.Length];
            for(int i = 0; i < ghosts.Length; i++){
                ghostBehaviors[i] = ghosts[i].GetComponent<EnemyBehavior>();
                inventoryManager.ghostsRoaming.Add(ghostBehaviors[i].enemyID);
            }
        }catch{
            Debug.LogWarning("No enemies are listed in the ghosts array in EnemyManager in PersistentManager!");
        }
    }

    public void ReLoaded(){
        try{
            if (ghosts.Length>0){
                foreach (EnemyBehavior ghost in ghostBehaviors)
                {
                    if (!inventoryManager.ghostsRoaming.Contains(ghost.enemyID))
                    {
                        ghost.stillInScene = false;
                    }
                }
            }
        }catch{
            Debug.LogWarning("No enemies are listed in the ghosts array in EnemyManager in PersistentManager!");
        }
        
    }
}
