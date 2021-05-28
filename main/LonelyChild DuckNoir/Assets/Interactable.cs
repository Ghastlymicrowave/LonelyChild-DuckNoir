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

    void Start(){
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        textManager = GameObject.Find("TextManager").GetComponent<TextManager>();
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
                inventoryManager.addItem(itemID);
            break;
            case interactableAction.TALK:// send textManager string[] from id to UI text frontend
            break;
        }
    }
}
