using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ivItem> items;
    public class ivItem{
        public readonly string name;
        public readonly string description;
        public string methodName; //name of method stored in battleBehavior
        public ivItem(string _name, string _description, string _methodName){
            name = _name;
            description = _description;
            methodName = _methodName;
        }
    }

    
    void Start(){
        items = new List<ivItem>();
    }

    public ivItem GetItemFromId(int id){
        switch(id){//using switch because no loaded memory and fast
            case 0: return new ivItem("fart item","this an item","");
            case 1: return new ivItem("fart item 2","after the first one","");
            default: return null;
        }
    }

    public void addItem(int id){
        items.Add(GetItemFromId(id));
    }
    public void addItem(ivItem item){
        items.Add(item);
    }

    public void callItem(ivItem item){ //This can be moved into BattleBehavior
        if (item.methodName!=""){
            Invoke(item.methodName,0f);
        }else{
            Debug.Log("Trying to invoke an empty method name");
        }
    }


}
