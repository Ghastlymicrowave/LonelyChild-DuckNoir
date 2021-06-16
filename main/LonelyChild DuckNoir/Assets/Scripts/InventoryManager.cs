using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using System.IO;
public class InventoryManager : MonoBehaviour
{
    string json = "LonelyChild";
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
        public string spritePath;
        //TODO add an image
        public ivItem(string _name, string _description, string[] _inspect, string _methodName, string[] _useText, int _id, string _spritePath)
        {
            name = _name;
            description = _description;
            methodName = _methodName;
            useText = _useText;
            id = _id;
            inspect = _inspect;
            spritePath = _spritePath;
        }
    }
    public void Reset(){
        attacks.Clear();
        attacks.Add(AttackActions.Flashlight);
        items.Clear();
        items.Add(GetItemFromId(ItemsEnum.Apple));
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

    public static ivItem GetItemFromId(ItemsEnum item)
    {
        switch (item)
        {//using switch because no loaded memory and fast
            case ItemsEnum.Apple:
                return new ivItem(
                    "Apple",
                    "An ordinary apple, eat it to regain 5 health in combat.",
                    new string[] {"just an ordinary red apple. Has a nice smell."},
                    "Apple",
                    new string[]{""},
                    (int)ItemsEnum.Apple,
                    "2D Assets/Items/Apple"); 
            case ItemsEnum.Ball:
                return new ivItem(
                    "Ball",
                    "A small rubber ball",
                    new string[] {"This old rubber ball still has some of it's bounce"},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.Ball,
                    "2D Assets/Items/Ball");
            case ItemsEnum.Photo:
                return new ivItem(
                    "Photo",
                    "It means nothing to you, but it might be a bit much for someone in particular...",
                    new string[]{"You don't recognize the people in this photo. Were they important?"},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.Photo,
                    "2D Assets/Items/Polaroid");
            case ItemsEnum.Key:
                return new ivItem(
                    "Key",
                    "This looks like a 'key' item. Heh.",
                    new string[]{"This key appears to be for use on locked doors."},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.Key,
                    "2D Assets/Items/Key");   
            case ItemsEnum.Fire_Poker:
                return new ivItem(
                    "Fire Poker",
                    "An old metal fire-poker, for poking fire, and other things.",
                    new string[]{"Watch where you point this thing, you could poke someone's eye out!"},
                    "",
                    new string[]{""},
                    (int)ItemsEnum.Fire_Poker,
                    "2D Assets/Items/Fireplace_Poker");
            default: return null;
        }
    }

    public class ivAttack{//soon to replace the big switch
        public string spritePath;
    }
    public static ivAttack GetAttackFromId(AttackActions action){
        switch(action){
            case AttackActions.Flashlight:
            return new Flashlight();
            case AttackActions.Fire_Poker:
            return new Fire_Poker();
            case AttackActions.Garlic:
            return new Garlic();
            case AttackActions.Theremin:
            return new Theremin();
            case AttackActions.Ruler:
            return new Ruler(); 
            default: return null;
        }
    }

    class Ruler : ivAttack{
        public Ruler(){
            spritePath = "2D Assets/Weapons/Flashlight";//TODO: change this sprite
        }
    }
    class Flashlight : ivAttack{
        public Flashlight(){
            spritePath = "2D Assets/Weapons/Flashlight";
        }
    }

    class Theremin : ivAttack{
        public Theremin(){
            spritePath = "2D Assets/Weapons/Theremin";
        }
    }

    class Fire_Poker : ivAttack{
        public Fire_Poker(){
            spritePath = "2D Assets/Weapons/Fireplace_Poker";
        }
    }

    class Garlic : ivAttack{
        public Garlic(){
            spritePath = "2D Assets/Items/Garlic";
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
    public void RemoveItem(int itemId){
        RemoveItem(items.Find(ivItem => ivItem.id == itemId));
    }

    public void AddAttack(AttackActions attack)
    {
        Debug.Log("adding attack: "+ attack.ToString());
        if (!attacks.Contains(attack))
        {
            attacks.Add(attack);
        }
    }

    public static Sprite LoadAttackSprite(AttackActions attack)
    {
        return Resources.Load(GetAttackFromId(attack).spritePath,typeof(Sprite)) as Sprite;
    }

    public static Sprite LoadItemSprite(int itemID)
    {
        return Resources.Load(GetItemFromId((ItemsEnum)itemID).spritePath,typeof(Sprite)) as Sprite;
    }

    public static Sprite LoadTalkSprite(int itemID)
    {
        switch (itemID)
        {//TODO: fill this out with sprites
            default: return Resources.Load("2D Assets/Programmer Art/ghosttemp",typeof(Sprite)) as Sprite;
        }
    }

    public void ResetSave(){
        string file = Application.persistentDataPath+"/"+json+".json";
        string contents = ""; //default level data upon reset
        if (File.Exists(file)){
            File.WriteAllText(file,contents);
        }
    }
    public void SaveJSON(){
        string file = Application.persistentDataPath+"/"+json+".json";
        string outString = "";
        File.WriteAllText(file,outString);
    }
    public void LoadJSON(){
        string file = Application.persistentDataPath+"/"+json+".json";
        if (File.Exists(file)){
            string contents = File.ReadAllText(file);
            SaveData save = JsonUtility.FromJson<SaveData>(contents);
            items.Clear();
            for (int i = 0; i < save.items.Length;i++){
                items.Add(GetItemFromId((Combat.ItemsEnum)save.items[i]));
            }
            for (int i = 0; i < save.attacks.Length;i++){
                AddAttack((AttackActions)save.attacks[i]);
            }
            ghostsRoaming.Clear();
            ghostsRoaming.AddRange(save.roaming);
            ghostsAscended.Clear();
            ghostsAscended.AddRange(save.ascended);
            ghostsCrucified.Clear();
            ghostsCrucified.AddRange(save.crucified);

        }else{
            ResetSave();
        }
    }
}
[System.Serializable]
public class SaveData{
        public int[] items;//item id's only for the time being
        public int[] attacks;
        public int[] roaming;
        public int[] ascended;
        public int[] crucified;
    }