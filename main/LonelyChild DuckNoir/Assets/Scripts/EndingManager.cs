using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    InventoryManager inventoryManager;
    TextManager tm;
    public enum EndingState {Bad, Neutral, Good}
    public EndingState endingState;
    public string goodScene;
    public string badScene;
    public string neutralScene;
    void Start()
    {
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        //set ending to neutral as default state.
        endingState = EndingState.Neutral;

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (inventoryManager.ghostsAscended.Count == 0 && inventoryManager.ghostsCrucified.Count > 0)
        {
            //bad end
            endingState = EndingState.Bad;
        }
        else if(inventoryManager.ghostsAscended.Count > 0 && inventoryManager.ghostsCrucified.Count == 0)
        {
            //good end
            endingState = EndingState.Good;
        }
        else
        {
            //neutral end
            endingState = EndingState.Neutral;
        }
        if (col.gameObject.tag == "Player")
        {
            switch(endingState)
            {
                case EndingState.Bad:
                SceneManager.LoadScene(badScene);
                break;
                case EndingState.Good:
                SceneManager.LoadScene(goodScene);
                break;
                default:
                SceneManager.LoadScene(neutralScene);
                break;
            }

        }

    }
}
