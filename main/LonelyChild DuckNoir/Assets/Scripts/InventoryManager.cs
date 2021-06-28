using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using System.IO;
public class InventoryManager : MonoBehaviour
{
    string json = "LonelyChild";
    public int enemyID;//current enemy you're fighting, if -1, you're not fighting an enemy and encounters won't happen
    public List<ivItem> items;
    public List<AttackActions> attacks; //stored as attack id's
    public List<int> ghostsRoaming; //not yet defeated, used to spawn one in overworld when scene is loaded, removed when defeated
    public List<int> ghostsAscended; // list of ghosts beaten with sentimental victory
    public List<int> ghostsCrucified; // list of ghosts beaten with crucifix
    GameSceneManager gameSceneManager;
    InventoryManager inventoryManager;
    public string checkpointScene = "";
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
        gameSceneManager = gameObject.GetComponent<GameSceneManager>();
        inventoryManager = gameSceneManager.GetComponent<InventoryManager>();
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
            case ItemsEnum.StaircaseKey:
                return new ivItem(
                    "Staircase Key",
                    "I might actually be able to get somewhere with this.",
                    new string[]{"I want to say this was to unlock the gate by the showing room."},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.StaircaseKey,
                    "2D Assets/Items/Key"); 
            case ItemsEnum.MasterBedroomKey:
                return new ivItem(
                    "Master Bedroom Key",
                    "I was never really allowed in the master bedroom. Could be dicey.",
                    new string[]{"This looks like a 'key' item. Heh.", "For use on locked doors."},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.MasterBedroomKey,
                    "2D Assets/Items/Key"); 
            case ItemsEnum.ShowingRoomKey:
                return new ivItem(
                    "Showing Room Key",
                    "Opens the place where parents to be meet us for the first time.",
                    new string[]{"Not really how I anticipated my first time through that door, but oh well."},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.ShowingRoomKey,
                    "2D Assets/Items/Key"); 
            case ItemsEnum.LibraryKey:
                return new ivItem(
                    "Library Key",
                    "The best place in the whole orphanage.",
                    new string[]{"To open doors with keys, approach the door and press 'Tab' or 'I' to open up the menu.",
                    "From there, scroll down to the proper key and press 'use with'."},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.LibraryKey,
                    "2D Assets/Items/Key"); 
            case ItemsEnum.Fire_Poker:
                return new ivItem(
                    "Fire Poker",
                    "An old metal fire-poker, for poking fire, and other things.",
                    new string[]{"Watch where you point this thing, you could poke someone's eye out!"},
                    "",
                    new string[]{""},
                    (int)ItemsEnum.Fire_Poker,
                    "2D Assets/Weapons/Fireplace_Poker");
            case ItemsEnum.Hourglass:
                return new ivItem(
                    "Hourglass",
                    "A time measuring device as old as... well... time.",
                    new string[]{"Looking at this hourglass, you can't help but think of time. \nAnd how little of it you have; that we all have.",
                    "\"What am I going to do with my life?\"\n\"What is the point OF life?\"",
                    "You can feel yourself getting more full of yourself by the second.\nYou should stop."},
                    "",
                    new string[]{""},
                    (int)ItemsEnum.Hourglass,
                    "2D Assets/Items/Hourglass");
            case ItemsEnum.Manual:
                return new ivItem(
                    "Ghost Hunting Manual",
                    "A ghost hunting manual. Some pages are missing, seemingly ripped out.",
                    new string[]{"You crack open the manual and have a read. The manual reads thusly:", "\"If you are unfortunate enough to face a ghost, here's what you do.\"",
                    "\"1: Talk to the ghost. Say the right thing, and the next steps will go smoother.\"",
                    "\"2: Attack the ghost's senses until the ghost emmits energy.\"", "\"You have 5 bulbs. One will light up whenever you damage the ghost by 20%.\"\n\"All five bulbs should be lit up for the next step.\"",
                    "\"Now, you've got some options.\"",
                    "\"3A: Use your crucifix and destroy the ghost. Or....\"",
                    "\"Make sure you've said the right thing first and then...\"",
                    "\"3B: Hold out an item that means something to the ghost.\"",
                    "\"Congrats! you've just hunted your first ghost!\""},
                    "",
                    new string[]{""},
                    (int)ItemsEnum.Manual,
                    "2D Assets/Items/Old_Book");
            case ItemsEnum.Teddy_Bear:
                return new ivItem(
                    "Teddy Bear",
                    "A familiar looking plush.",
                    new string[]{"Looking closer, it seems strangely clean."},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.Teddy_Bear,
                    "2D Assets/Items/Teddy_Sans_Blood"); 
            case ItemsEnum.Russian_Doll:
                return new ivItem(
                    "Russian Doll",
                    "Smells like coal.",
                    new string[]{"This thing looks old."},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.Russian_Doll,
                    "2D Assets/Items/Doll"); 
            case ItemsEnum.Eraser:
                return new ivItem(
                    "Chalk Eraser",
                    "A worn and dusty chalk eraser.",
                    new string[]{"Resisting the urge to smack this this thing and release the chalk powder is exhausting."},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.Eraser,
                    "2D Assets/Items/Eraser"); 
            case ItemsEnum.Spinning_Toy:
                return new ivItem(
                    "Spinning Toy",
                    "An idle plaything.",
                    new string[]{"You never really liked these things,\nbut some people just can't leave their hands to themselves."},
                    "",
                    new string[]{""}, 
                    (int)ItemsEnum.Spinning_Toy,
                    "2D Assets/Items/Fidget_Spinner"); 
            case ItemsEnum.KeyRing:
                return new ivItem(
                    "Key Ring",
                    "A ring used for storing keys",
                    new string[]{"With this handy key ring you'll be able to open door without needing to open your inventory!",
                    "Just interact with a door and if you have the key, you'll automatically unlock and open it!",
                    "You're not quite sure what an \"Inventory\" is. Is this a page torn out of some game manual?"},
                    "",
                    new string[]{""},
                    (int)ItemsEnum.KeyRing,
                    "2D Assets/Items/Keychain.png");
            case ItemsEnum.Scissors:
                return new ivItem(
                    "Scissors",
                    "Big ol' scissors to cut big ol' things.",
                    new string[]{"This conveniently placed pair of scissors are big enough to cut some tough rope.",
                    "If you used this item, it could get you out of a tight spot!"
                    },
                    "",
                    new string[]{""},
                    (int)ItemsEnum.Scissors,
                    "2D Assets/Items/Scissors.png");
            default: return null;
        }
    }

    public class ivAttack{//soon to replace the big switch
        public string spritePath;
        public string name;
        public string desc;
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
            name = "Ruler";
            desc = "A wooden ruler, used for measuring things. Or a weapon for bad people.";
        }
    }
    class Flashlight : ivAttack{
        public Flashlight(){
            spritePath = "2D Assets/Weapons/Flashlight";
            name = "Flashlight";
            desc = "A flashlight, or a 'torch' if you're a brit. This thing's pretty bright! That would totally hurt a ghost.";
        }
    }

    class Theremin : ivAttack{
        public Theremin(){
            spritePath = "2D Assets/Weapons/Theremin";
            name = "Theremin";
            desc = "It makes some weird spooky noises. You remember reading somewhere that it hurts ghosts.";
        }
    }

    class Fire_Poker : ivAttack{
        public Fire_Poker(){
            spritePath = "2D Assets/Weapons/Fireplace_Poker";
            name = "Fireplace Poker";
            desc = "The firepoker has some weight to it. You shudder to think of what this could have been used for.";
        }
    }

    class Garlic : ivAttack{
        public Garlic(){
            spritePath = "2D Assets/Items/Garlic";
            name = "Garlic";
            desc = "Smells disgusting! Surely ghosts will hate it too?";
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

    public string jsonFile(){
        return Application.persistentDataPath+"/"+json+".json";;
    }

    public void ResetSave(){
        string file = jsonFile();
        SaveData save = new SaveData();
        save.ascended = new int[0];
        save.crucified = new int[0];
        save.roaming = new int[3]{1,2,3};
        save.attacks = new int[1]{(int)AttackActions.Flashlight};
        save.items = new int[1]{(int)ItemsEnum.Apple};
        //change back to PlayroomWB if needed
        save.checkpointScene = "SecondFloor";
        
        File.WriteAllText(file,JsonUtility.ToJson(save));
    }
    public void SaveJSON(){
        string file = jsonFile();
        SaveData save = new SaveData();
        save.ascended=ghostsAscended.ToArray();
        save.attacks = new int[attacks.Count];
        for(int i =0; i < attacks.Count; i++){
            save.attacks[i] = (int)attacks[i];
        }
        save.crucified = ghostsCrucified.ToArray();
        save.roaming = ghostsRoaming.ToArray();
        save.checkpointScene = gameSceneManager.SceneName();
        
        File.WriteAllText(file,JsonUtility.ToJson(save));
    }
    public bool JSONExists(){
        return (File.Exists(jsonFile()));
    }

    public bool IsFreshSave(){
        string file = jsonFile();
        if (File.Exists(file)){
            string contents = File.ReadAllText(file);
            SaveData save = new SaveData();
            try {
                save = JsonUtility.FromJson<SaveData>(contents);
                if (save.checkpointScene != "MainMenu"||save.checkpointScene != "SecondFloor"){
                    return true;
                }else{
                    return false;
                }
            }catch{
                return true;
            }
        }else{
            return true;
        }
    }

    public void LoadJSON(){
        string file = jsonFile();
        if (File.Exists(file)){
            string contents = File.ReadAllText(file);
            SaveData save = new SaveData();
            try {
                save = JsonUtility.FromJson<SaveData>(contents);
            }catch{
                ResetSave();
                LoadJSON();
            }
            items = new List<ivItem>();
            attacks = new List<AttackActions>();
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
            inventoryManager.checkpointScene = save.checkpointScene;

        }else{
            ResetSave();
            LoadJSON();
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
        public string checkpointScene;
    }