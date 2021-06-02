using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using UnityEngine.UI;
public class Submenu : MonoBehaviour
{
    GameObject content;
    InventoryManager inventoryManager;
    battleBehavior battleBehavior; 
    void Start()
    {
        content = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        inventoryManager = GameObject.Find("PersistentManager").GetComponent<InventoryManager>();
        battleBehavior = GameObject.Find("BattleBehavior").GetComponent<battleBehavior>();
    }

    public void ClearItems(){
        if (content.transform.childCount>0){
            for(int i = content.transform.childCount-1; i > -1; i--){
                Destroy(content.transform.GetChild(i).gameObject);
            }
        }
        
    }

    public void OpenMenu(ButtonEnum actionType){
        ClearItems();
        switch(actionType){
            case ButtonEnum.Attack:
                for (int i = 0; i < inventoryManager.attacks.Count;i++){
                    Sprite spr = InventoryManager.LoadAttackSprite(inventoryManager.attacks[i]);
                    AddItem(spr,(int)inventoryManager.attacks[i]);
                }
            break;
            case ButtonEnum.Talk:
                for (int i = 0; i < battleBehavior.enemy.talkActions.Length;i++){
                    Sprite spr = InventoryManager.LoadTalkSprite((int)battleBehavior.enemy.talkActions[i]);
                    AddItem(spr,(int)battleBehavior.enemy.talkActions[i]);
                }
            break;
            case ButtonEnum.Items:
                for (int i = 0; i < inventoryManager.items.Count;i++){
                    Sprite spr = InventoryManager.LoadItemSprite((int)inventoryManager.items[i].id);
                    AddItem(spr,(int)inventoryManager.items[i].id);
                }
            break;
        }
    }

    public void AddItem(Sprite spr, int actionID){
        Debug.Log(spr);
        GameObject item = Instantiate(Resources.Load("Prefabs/SubmenuItem") as GameObject,content.transform);
        Image image = item.GetComponent<Image>();
        image.sprite = spr;
        Button btn = item.GetComponent<Button>();
        btn.onClick.AddListener(delegate {battleBehavior.ExternalSubButtonPressed(actionID);});
    }
}
