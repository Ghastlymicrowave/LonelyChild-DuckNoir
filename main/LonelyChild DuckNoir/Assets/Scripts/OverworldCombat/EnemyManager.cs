using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    TextManager tm;
    InventoryManager inventoryManager;
    public int IDBase = 0;
    [SerializeField] GameObject[] ghosts;
    EnemyBehavior[] ghostBehaviors;
    void Start()
    {
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        

        ghostBehaviors = new EnemyBehavior[ghosts.Length];
        for(int i = 0; i < ghosts.Length; i++){
            ghostBehaviors[i] = ghosts[i].GetComponent<EnemyBehavior>();
        }
        
    }

    public void ReLoaded(){
        foreach (EnemyBehavior ghost in ghostBehaviors)
        {
            if (!inventoryManager.ghostsRoaming.Contains(ghost.enemyID))
            {
                ghost.stillInScene = false;
            }
        }
    }
}
