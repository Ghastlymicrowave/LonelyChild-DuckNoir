using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    InventoryManager inventoryManager;
    TextManager textManager;
    [SerializeField] interactableAction action;
    [SerializeField] int itemID;
    [SerializeField] int textID;
    [SerializeField] Activatable activatable;
    static player_main playerRef;
    public int dialogueID = 0;
    public bool isReady = false;

    public GameObject indicator;

    void Start(){
        inventoryManager = GameObject.Find("PersistentManager").GetComponent<InventoryManager>();
        textManager = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        if (playerRef==null){playerRef = GameObject.Find("Player").GetComponent<player_main>();}
        
    }
    public enum interactableAction{
        ACTIVATE,
        ITEM,
        TALK
    }
    public void Trigger(){
        switch(action){
            case interactableAction.ACTIVATE:
                activatable.Activate();
            break;
            case interactableAction.ITEM:
                inventoryManager.addItem(itemID);//TODO: add a text manager thing for getting the item
            break;
            case interactableAction.TALK:// send textManager string[] from id to UI text frontend
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        playerRef.InteractableEntered(this);
    }

    private void OnTriggerExit2D(Collider2D other){
        playerRef.InteractableLeft(this);
    }
}
