using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public class InventoryManager : MonoBehaviour
{
    public int enemyID;//current enemy you're fighting, if -1, you're not fighting an enemy and encounters won't happen
    public Vector3 playerPosOnStart;
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
        public string[] inspect;//text shown when inspecting item
        //TODO add an image
        public ivItem(string _name, string _description, string[] _inspect, string _methodName, string[] _useText, int _id)
        {
            name = _name;
            description = _description;
            methodName = _methodName;
            useText = _useText;
            id = _id;
            inspect = _inspect;
        }
    }

    public void Reset(){
        attacks.Clear();
        attacks.Add(AttackActions.Flashlight);
        items.Clear();
        items.Add(GetItemFromId(ItemsEnum.Apple));

        attacks.Add(AttackActions.Theremin);
        items.Add(GetItemFromId(ItemsEnum.Ball));
    }

    void Start()
    {
        items = new List<ivItem>();
        Reset();
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
                    "An ordinary apple, eat it to regain 5 health in combat.",
                    new string[] {"just an ordinary red apple. Has a nice smell."},
                    "Apple",
                    TextManager.stringsToArray("you used an apple"), (int)ItemsEnum.Apple);
            case ItemsEnum.Ball:
                return new ivItem(
                    "Ball",
                    "A small rubber ball",
                    new string[] {"This old rubber ball still has some of it's bounce"},
                    "",
                    TextManager.stringsToArray("you used a ball"), (int)ItemsEnum.Ball);
            case ItemsEnum.Photo:
                return new ivItem(
                    "Photo",
                    "It means nothing to you, but it might be a bit much for someone in particular...",
                    new string[]{"You don't recognize the people in this photo. Were they important?"},
                    "",
                    TextManager.stringsToArray("you used a Photo"), (int)ItemsEnum.Photo);
            case ItemsEnum.Key:
                return new ivItem(
                    "Key",
                    "This looks like a 'key' item. Heh.",
                    new string[]{"This key appears to be for use on locked doors."},
                    "",
                    TextManager.stringsToArray("you used a Photo"), (int)ItemsEnum.Key);   
            default: return null;
        }
    }

    public void AddItem(ItemsEnum item)
    {
        Debug.Log("adding item from enum: "+items.ToString());
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
}
