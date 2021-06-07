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
    [SerializeField] Activatable activatable;
    [SerializeField] AttackActions attack;
    static player_main playerRef;
    [Tooltip("keep less than 0 for no text")]
    [SerializeField] int dialogueID = -1;
    public bool isReady = false;
    public bool isBusy = false;
    [SerializeField] bool oneTimeUse = false;

    public GameObject indicator;

    [SerializeField] bool needsItemUse = false;
    [SerializeField] ItemsEnum requiredItem;
    [SerializeField] int notCompleteItemUseTextID;//id of text to trigger when interacting while item hasn't been used, like to inspect
    [SerializeField] int usedRequiredItemTextId;
    void Start(){
        inventoryManager = GameObject.Find("PersistentManager").GetComponent<InventoryManager>();
        textManager = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        if (playerRef==null){playerRef = GameObject.Find("Player").GetComponent<player_main>();}
        Debug.Log(inventoryManager);
    }
    public enum interactableAction{
        ACTIVATE,//activates an interactable
        ITEM,//gives the player an item
        TALK,//just gives the player some text
        ATTACK//Gives the player a new attack
    }

    public bool HasItemUse(){
        return needsItemUse;
    }
    public void Trigger(){
        isReady=false;
        if (needsItemUse){
            if (dialogueID>-1){
                    playerRef.TriggerDialogue(notCompleteItemUseTextID);
                }
            return;
        }
        switch(action){
            case interactableAction.ACTIVATE:
                activatable.Activate();
                playerRef.TriggerDialogue(dialogueID);
            break;
            case interactableAction.ITEM:
                Debug.Log(item.ToString()+" added to inventory");
                Debug.Log(inventoryManager);
                inventoryManager.AddItem(item);//TODO: add a text manager thing for getting the item
                if (dialogueID>-1){
                    playerRef.TriggerDialogue(dialogueID);
                }
            break;
            case interactableAction.TALK:// send textManager string[] from id to UI text frontend
            playerRef.TriggerDialogue(dialogueID);
            break;
            case interactableAction.ATTACK:
            playerRef.TriggerDialogue(dialogueID);
            inventoryManager.AddAttack(attack);
            break;
        }
        if (oneTimeUse){
            playerRef.InteractableLeft(this);
            this.gameObject.SetActive(false);
        }
    }

    public void Test(){
        Debug.Log("THIS IS A TEST");
    }
    private void OnTriggerEnter2D(Collider2D other){
        playerRef.InteractableEntered(this);
    }

    private void OnTriggerExit2D(Collider2D other){
        playerRef.InteractableLeft(this);
    }

    public void CheckItemUse(InventoryManager.ivItem item){
        if (needsItemUse==false){
            playerRef.UseItem(item);
            return;
        }
        if ((int)requiredItem == item.id){
            needsItemUse = false;
            playerRef.TriggerDialogue(usedRequiredItemTextId);
        }else{
            if (dialogueID>-1){
                Debug.Log("Item didn't work");
                playerRef.TriggerDialogue(-1);
            }
        }
    }
}
