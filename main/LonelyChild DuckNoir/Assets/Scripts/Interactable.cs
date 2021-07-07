using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class Interactable : MonoBehaviour
{
    [SerializeField] SimpleSoundCue cue;
    InventoryManager inventoryManager;
    TextManager textManager;
    GameSceneManager gameSceneManager;
    [SerializeField] interactableAction action;
    [SerializeField] Combat.ItemsEnum item;
    [SerializeField] Activatable activatable;
    [SerializeField] AttackActions attack;
    static ThirdPersonPlayer playerRef;
    [Tooltip("keep less than 0 for no text")]
    [SerializeField] int dialogueID = -1;//KEEP -1 IF YOU WANT TO USE RANDOM
    [SerializeField] int[] randomdialogueIDs;
    public bool isReady = false;
    public bool isBusy = false;
    [SerializeField] bool oneTimeUse = false;
    [SerializeField] bool DisableOnUse = true;

    public GameObject indicator;

    [SerializeField] bool needsItemUse = false;
    [SerializeField] ItemsEnum requiredItem;
    [SerializeField] bool deleteRequiredItem;
    [SerializeField] int notCompleteItemUseTextID;//id of text to trigger when interacting while item hasn't been used, like to inspect
    [SerializeField] int usedRequiredItemTextId;
    [SerializeField] string sceneName;
    [SerializeField] bool useKeyRing = false;
    void Start(){
        inventoryManager = GameObject.Find("PersistentManager").GetComponent<InventoryManager>();
        gameSceneManager = inventoryManager.gameObject.GetComponent<GameSceneManager>();
        textManager = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        if (playerRef==null){playerRef = GameObject.Find("Player").GetComponent<ThirdPersonPlayer>();}
        if (randomdialogueIDs ==null){randomdialogueIDs = new int[0];}
    }

    public enum interactableAction{
        ACTIVATE,//activates an interactable
        ITEM,//gives the player an item
        TALK,//just gives the player some text
        ATTACK,//Gives the player a new attack
        ITEMandATTACK,
        CHANGELEVEL
    }

    public bool HasItemUse(){
        return needsItemUse;
    }
    bool HasKeyRing(){
        bool hasRing = false;
        bool hasItem = false;
        for (int i = 0 ; i < inventoryManager.items.Count; i++){
            if (inventoryManager.items[i].id == (int)ItemsEnum.KeyRing){
                hasRing = true;
            }else if (inventoryManager.items[i].id == (int)requiredItem){
                hasItem = true;
            }
        }
        return (useKeyRing && needsItemUse && hasRing && hasItem);
    }
    public void Trigger(){
        bool rand = (randomdialogueIDs.Length>0);
        isReady=false;
        if (needsItemUse){
            if (HasKeyRing()){
                needsItemUse = false;
                Trigger();
                return;
            }
            if (dialogueID>-1||rand){
                    playerRef.TriggerDialogue(notCompleteItemUseTextID);
                }
            return;
        }
        if (cue !=null){
            cue.Trigger();
            if (oneTimeUse){
                cue.gameObject.transform.SetParent(this.gameObject.transform.parent);
            }
        }
        switch(action){
            case interactableAction.ACTIVATE:
                activatable.Activate();
                if (rand){
                        playerRef.TriggerDialogue(randomdialogueIDs[Random.Range(0,randomdialogueIDs.Length)]);
                    }else if (dialogueID>-1){
                        playerRef.TriggerDialogue(dialogueID);
                    }
            break;
            case interactableAction.ITEM:
                Debug.Log(item.ToString()+" added to inventory");
                Debug.Log(inventoryManager);
                inventoryManager.AddItem(item);//TODO: add a text manager thing for getting the item
                if (rand){
                    playerRef.TriggerDialogue(randomdialogueIDs[Random.Range(0,randomdialogueIDs.Length)]);
                }else if (dialogueID>-1){
                    playerRef.TriggerDialogue(dialogueID);
                }
            break;
            case interactableAction.TALK:// send textManager string[] from id to UI text frontend
            if (rand){
                playerRef.TriggerDialogue(randomdialogueIDs[Random.Range(0,randomdialogueIDs.Length)]);
            }else if (dialogueID>-1){
                playerRef.TriggerDialogue(dialogueID);
            }
            break;
            case interactableAction.ATTACK:
            if (rand){
                playerRef.TriggerDialogue(randomdialogueIDs[Random.Range(0,randomdialogueIDs.Length)]);
            }else if (dialogueID>-1){
                playerRef.TriggerDialogue(dialogueID);
            }
            inventoryManager.AddAttack(attack);
            break;
            case interactableAction.ITEMandATTACK:
            if (rand){
                playerRef.TriggerDialogue(randomdialogueIDs[Random.Range(0,randomdialogueIDs.Length)]);
            }else if (dialogueID>-1){
                playerRef.TriggerDialogue(dialogueID);
            }
            inventoryManager.AddAttack(attack);
            inventoryManager.AddItem(item);
            break;
            case interactableAction.CHANGELEVEL:
            if (rand){
                playerRef.TriggerDialogue(randomdialogueIDs[Random.Range(0,randomdialogueIDs.Length)]);
            }else if (dialogueID>-1){
                playerRef.TriggerDialogue(dialogueID);
            }
            gameSceneManager.TransitionScene(sceneName);
            break;
        }
        if (oneTimeUse){
            playerRef.InteractableLeft(this);
            this.enabled = false;
            indicator.SetActive(false);
            if (DisableOnUse){
                this.gameObject.SetActive(false);
            }
        }
        
    }

    void OnTriggerEnter(Collider other){
        if (other.tag=="PlayerInteract"){
            playerRef.InteractableEntered(this);
        }
        Debug.Log(other.tag);   
    }

    void OnTriggerExit(Collider other){
        if (other.tag=="PlayerInteract"){
            playerRef.InteractableLeft(this);
        }
    }

    public void CheckItemUse(InventoryManager.ivItem item){
        if (needsItemUse==false){
            Debug.Log("no need to use item");
            playerRef.UseItem(item);
            return;
        }
        if ((int)requiredItem == item.id){
            Debug.Log("used right item");
            needsItemUse = false;
            if (deleteRequiredItem){
                inventoryManager.items.Remove(item);
            }
            if(useKeyRing){
                Trigger();
            }else{
                playerRef.TriggerDialogue(usedRequiredItemTextId);
            }
        }else{
            if (dialogueID>-1){
                Debug.Log("Item didn't work");
                playerRef.TriggerDialogue(0);
                Debug.Log("Used Item: "+item.id.ToString()+" needs item: "+((int)requiredItem).ToString());
            }
        }
    }
}
