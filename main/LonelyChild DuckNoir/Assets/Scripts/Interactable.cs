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
    [Tooltip("keep less than 0 for no text")]
    public int dialogueID = -1;
    public bool isReady = false;
    [SerializeField] bool oneTimeUse = false;

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
        isReady=false;
        switch(action){
            case interactableAction.ACTIVATE:
                activatable.Activate();
                playerRef.TriggerDialogue(dialogueID);
            break;
            case interactableAction.ITEM:
                inventoryManager.addItem(itemID);//TODO: add a text manager thing for getting the item
                if (dialogueID>-1){
                    playerRef.TriggerDialogue(dialogueID);
                }
            break;
            case interactableAction.TALK:// send textManager string[] from id to UI text frontend
            playerRef.TriggerDialogue(dialogueID);
            break;
        }
        if (oneTimeUse){
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        playerRef.InteractableEntered(this);
    }

    private void OnTriggerExit2D(Collider2D other){
        playerRef.InteractableLeft(this);
    }
}
