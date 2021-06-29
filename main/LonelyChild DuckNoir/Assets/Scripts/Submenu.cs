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
                    AddItem(spr,(int)actionType,(int)inventoryManager.attacks[i]);
                }
            break;
            case ButtonEnum.Talk:
                for (int i = 0; i < battleBehavior.enemy.talkActions[battleBehavior.talkIndex].Length;i++){
                    Sprite spr = InventoryManager.LoadTalkSprite((int)battleBehavior.enemy.talkActions[battleBehavior.talkIndex][i]);
                    AddItem(spr,(int)actionType,(int)battleBehavior.enemy.talkActions[battleBehavior.talkIndex][i]);
                }
            break;
            case ButtonEnum.Items:
                for (int i = 0; i < inventoryManager.items.Count;i++){
                    if (inventoryManager.items[i].id==(int)ItemsEnum.Fire_Poker){continue;}
                    Sprite spr = InventoryManager.LoadItemSprite((int)inventoryManager.items[i].id);
                    AddItem(spr,(int)actionType,(int)inventoryManager.items[i].id);
                }
            break;
        }
    }

    static string RemoveUnderscores(string instring){
        return instring.Replace("_"," ");
    }

    public static string GetActionName(int actionType, int actionID){
        switch(actionType){
            case (int)ButtonEnum.Attack:
            return RemoveUnderscores(((AttackActions)actionID).ToString());
            case (int)ButtonEnum.Talk:
            return RemoveUnderscores(((TalkEnum)actionID).ToString());
            case (int)ButtonEnum.Items:
            return RemoveUnderscores(((ItemsEnum)actionID).ToString());
            default: return "?";
        }
    }

    public void AddItem(Sprite spr,int actionType, int actionID){
        Debug.Log(spr);
        GameObject item = Instantiate(Resources.Load("Prefabs/OverworldEnginePrefabs/SubmenuItem") as GameObject,content.transform);
        Image image = item.GetComponent<Image>();
        image.sprite = spr;
        Button btn = item.GetComponent<Button>();
        btn.onClick.AddListener(delegate {battleBehavior.ExternalSubButtonPressed(actionID);});
        Text text = item.transform.GetChild(0).GetComponent<Text>();
        text.text = GetActionName(actionType,actionID);
    }
}
    