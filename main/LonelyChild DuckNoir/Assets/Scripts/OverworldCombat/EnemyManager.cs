using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
     TextManager tm;
    InventoryManager inventoryManager;
    public int IDBase = 0;
    public GameObject[] ghosts;
    void Start()
    {
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        int index = IDBase;
        foreach (GameObject ghost in ghosts)
        {
            if (!inventoryManager.ghostsRoaming.Contains(index))
            {
                ghosts[index].SetActive(false);
            }
            index += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
