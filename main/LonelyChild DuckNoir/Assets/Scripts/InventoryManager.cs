using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public class InventoryManager : MonoBehaviour
{
    public int enemyID;//current enemy you're fighting, if -1, you're not fighting an enemy and encounters won't happen
    public List<ivItem> items;
    public List<AttackActions> attacks; //stored as attack id's
    public List<int> ghostsRoaming; //not yet defeated, used to spawn one in overworld when scene is loaded, removed when defeated
    public List<int> ghostsAscended; // list of ghosts beaten with sentimental victory
    public List<int> ghostsCrucified; // list of ghosts beaten with crucifix
    public class ivItem
    {
        public readonly string name;
        public readonly string description;
        public string methodName; //name of method to be used out of combat on player_main NOT IN RN
        public string[] useText;
        public int id;
        //TODO add an image
        public ivItem(string _name, string _description, string _methodName, string[] _useText, int id)
        {
            name = _name;
            description = _description;
            methodName = _methodName;
            useText = _useText;
        }
    }



    void Start()
    {
        items = new List<ivItem>();
        //debug only for now
        attacks.Add(AttackActions.Flashlight);
        items.Add(GetItemFromId(ItemsEnum.Apple));
    }

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("manager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public ivItem GetItemFromId(ItemsEnum item)
    {
        switch (item)
        {//using switch because no loaded memory and fast
            case ItemsEnum.Apple:
                return new ivItem(
                    "Apple",
                    "An ordinary apple",
                    "",
                    TextManager.stringsToArray("you used an apple"), (int)ItemsEnum.Apple);
            case ItemsEnum.Ball:
                return new ivItem(
                    "Ball",
                    "A small rubber ball",
                    "",
                    TextManager.stringsToArray("you used a ball"), (int)ItemsEnum.Apple);
            default: return null;
        }
    }

    public void AddItem(ItemsEnum item)
    {
        items.Add(GetItemFromId(item));
    }
    public void AddItem(ivItem item)
    {
        items.Add(item);
    }
    public void RemoveItem(ivItem item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }
    public void AddAttack(AttackActions attack)
    {
        if (!attacks.Contains(attack))
        {
            attacks.Add(attack);
        }
    }

    public static Sprite LoadAttackSprite(AttackActions attack)
    {
        switch (attack)
        {//TODO: fill this out with sprites
            default: return Resources.Load("2D Assets/Programmer Art/ghosttemp",typeof(Sprite)) as Sprite;
        }
    }

    public static Sprite LoadItemSprite(int itemID)
    {
        switch (itemID)
        {//TODO: fill this out with sprites
            default: return Resources.Load("2D Assets/Programmer Art/ghosttemp",typeof(Sprite)) as Sprite;
        }
    }

    public static Sprite LoadTalkSprite(int itemID)
    {
        switch (itemID)
        {//TODO: fill this out with sprites
            default: return Resources.Load("2D Assets/Programmer Art/ghosttemp",typeof(Sprite)) as Sprite;
        }
    }


    public void callItem(ivItem item)
    { //This can be moved into BattleBehavior
        if (item.methodName != "")
        {
            Invoke(item.methodName, 0f);
        }
        else
        {
            Debug.Log("Trying to invoke an empty method name");
        }
    }


}
