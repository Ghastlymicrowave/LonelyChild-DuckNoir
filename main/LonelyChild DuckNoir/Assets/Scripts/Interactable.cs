using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class Interactable : MonoBehaviour
{
    InventoryManager inventoryManager;
    TextManager textManager;
    [SerializeField] interactableAction action;
    [SerializeField] Combat.ItemsEnum item;
    [SerializeField] int textID;
    [SerializeField] Activatable activatable;
    static player_main playerRef;
    [Tooltip("keep less than 0 for no text")]
    [SerializeField] int dialogueID = -1;
    public bool isReady = false;
    public bool isBusy = false;
    [SerializeField] bool oneTimeUse = false;

    public GameObject indicator;

    [SerializeField] bool needsItemUse = false;
    [SerializeField] ItemsEnum requiredItem;
    [SerializeField] int notCompleteItemUseTextID;//id of text to trigger when interacting while item hasn't been used
    void Start(){
        inventoryManager = GameObject.Find("PersistentManager").GetComponent<InventoryManager>();
        textManager = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        if (playerRef==null){playerRef = GameObject.Find("Player").GetComponent<player_main>();}
        
    }
    public enum interactableAction{
        ACTIVATE,//activates an interactable
        ITEM,//gives the player an item
        TALK//just gives the player some text
    }
    public void Trigger(){
        if (needsItemUse){
            if (dialogueID>-1){
                    playerRef.TriggerDialogue(notCompleteItemUseTextID);
                }
            return;
        }
        isReady=false;
        switch(action){
            case interactableAction.ACTIVATE:
                activatable.Activate();
                playerRef.TriggerDialogue(dialogueID);
            break;
            case interactableAction.ITEM:
                inventoryManager.AddItem(item);//TODO: add a text manager thing for getting the item
                if (dialogueID>-1){
                    playerRef.TriggerDialogue(dialogueID);
                }
            break;
            case interactableAction.TALK:// send textManager string[] from id to UI text frontend
            playerRef.TriggerDialogue(dialogueID);
            break;
        }
        if (oneTimeUse){
            playerRef.InteractableLeft(this);
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        playerRef.InteractableEntered(this);
    }

    private void OnTriggerExit2D(Collider2D other){
        playerRef.InteractableLeft(this);
    }

    public void CheckItemUse(int itemID){
        if ((int)requiredItem == itemID){
            Trigger();
        }else{
            if (dialogueID>-1){
                playerRef.TriggerDialogue(-1);
            }
        }
    }
}
