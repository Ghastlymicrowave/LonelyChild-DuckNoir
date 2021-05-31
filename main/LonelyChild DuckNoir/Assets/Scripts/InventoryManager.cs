using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public class InventoryManager : MonoBehaviour
{
    public List<ivItem> items;
    public List<AttackActions> attacks; //stored as attack id's
    public class ivItem{
        public readonly string name;
        public readonly string description;
        public string methodName; //name of method to be used out of combat on player_main NOT IN RN
        public string[] useText;

        //TODO add an image
        public ivItem(string _name, string _description, string _methodName, string[] _useText){
            name = _name;
            description = _description;
            methodName = _methodName;
            useText = _useText;
        }
    }


    
    void Start(){
        items = new List<ivItem>();
    }

    void Awake(){
        GameObject[] objs = GameObject.FindGameObjectsWithTag("manager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public ivItem GetItemFromId(ItemsEnum item){
        switch(item){//using switch because no loaded memory and fast
            case ItemsEnum.Thing: return new ivItem(
                "fart item",
                "this an item",
                "",
                TextManager.stringsToArray("this is the use text"));
            case ItemsEnum.OtherThing: return new ivItem(  
                "fart item 2",
                "after the first one",
                "",
                TextManager.stringsToArray("this is the use text"));
            default: return null;
        }
    }

    public void AddItem(ItemsEnum item){
        items.Add(GetItemFromId(item));
    }
    public void AddItem(ivItem item){
        items.Add(item);
    }
    public void RemoveItem(ivItem item){
        if (items.Contains(item)){
            items.Remove(item);
        }
    }
    public void AddAttack(AttackActions attack){
        if (!attacks.Contains(attack)){
            attacks.Add(attack);
        }
    }

    public Sprite LoadAttackSprite(AttackActions attack){
        switch(attack){//TODO: fill this out with sprites
            default:return Resources.Load("2D Assets/Programmer Art/ghosttemp") as Sprite;
        } 
    } 

    public Sprite LoadItemSprite(int itemID){
        switch(itemID){//TODO: fill this out with sprites
            default:return Resources.Load("2D Assets/Programmer Art/ghosttemp") as Sprite;
        } 
    } 


    public void callItem(ivItem item){ //This can be moved into BattleBehavior
        if (item.methodName!=""){
            Invoke(item.methodName,0f);
        }else{
            Debug.Log("Trying to invoke an empty method name");
        }
    }


}
