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
        for(int i = content.transform.childCount; i > -1; i--){
            Destroy(content.transform.GetChild(i).gameObject);
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
            break;
            case ButtonEnum.Items:
            break;
        }
    }

    public void AddItem(Sprite spr, int actionID){
        GameObject item = Instantiate(Resources.Load("Assets/Prefabs/SubmenuItem") as GameObject,content.transform);
        Image image = item.GetComponent<Image>();
        image.sprite = spr;
        Button btn = item.GetComponent<Button>();
        btn.onClick.AddListener(delegate {battleBehavior.ExternalSubButtonPressed(actionID);});
    }
}
