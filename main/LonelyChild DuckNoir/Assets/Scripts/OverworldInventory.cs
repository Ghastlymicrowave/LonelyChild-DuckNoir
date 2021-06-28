using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Combat;
public class OverworldInventory : MonoBehaviour
{
    bool menuOpen = false;
    [SerializeField] Animator animator;
    Text buttonTxt;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] GameObject WeaponsContent;
    [SerializeField] GameObject ItemsContent;
    ThirdPersonPlayer player;
    InventoryManager inventoryManager;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<ThirdPersonPlayer>();
        inventoryManager = GameObject.Find("PersistentManager").GetComponent<InventoryManager>();
    }

    void Update(){
        if (Input.GetButtonDown("Inventory")){
            ToggleMenu();
        }
    }

    public void ToggleMenu(){
        if (!player.canMove){
            Debug.Log("refusing to open inventory because of player.canMove");
            return;
        }
        menuOpen = !menuOpen;
        if (menuOpen){
            player.SetMouseMode(false);
            animator.Play("Open",0);
            GenerateItems();
        }else{
            player.SetMouseMode(true);
            animator.Play("Close",0);
        }
        player.InventoryOpen = menuOpen;
        
    }

    public void CloseMenu(){
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("InventoyIsClosed")&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Close")){
            animator.Play("Close",0);
            menuOpen = false;
        }
        player.SetMouseMode(true);
        player.InventoryOpen = menuOpen;
    }

    void GenerateItems(){//TODO: add generate weapons
        for(int i = ItemsContent.transform.childCount-1; i > -1; i -= 1){
            Destroy(ItemsContent.transform.GetChild(i).gameObject);
        }
        foreach( InventoryManager.ivItem i in inventoryManager.items){
            AddItem(i);
        }
        for(int i = WeaponsContent.transform.childCount-1; i > -1; i -= 1){
            Destroy(WeaponsContent.transform.GetChild(i).gameObject);
        }
        foreach( AttackActions i in inventoryManager.attacks){
            AddWeapon(i);
        }
    }

    public void UseItem(InventoryManager.ivItem item){
        Debug.Log("using");
        CloseMenu();
        player.UseItemOnInteractable(item);
    }

    public void InspectItem(InventoryManager.ivItem item){
        Debug.Log("inspecting");
        CloseMenu();
        player.TriggerDialogue(item.inspect);
    }

    public void InspectWeapon(InventoryManager.ivAttack item){
        CloseMenu();
        //player.TriggerDialogue(item.inspect);
    }

    void AddWeapon(AttackActions weapon){
        GameObject newItemObj = Instantiate(itemPrefab,Vector3.zero,Quaternion.identity);
        newItemObj.transform.SetParent(WeaponsContent.transform,false);
        newItemObj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = InventoryManager.LoadAttackSprite(weapon);//image
        InventoryManager.ivAttack atk = InventoryManager.GetAttackFromId(weapon);
        newItemObj.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = atk.name;//name
        newItemObj.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = atk.desc;//description

        GameObject inspectButton = newItemObj.transform.GetChild(0).GetChild(3).gameObject;
        //inspectButton.GetComponent<Button>().onClick.AddListener(delegate {InspectItem(item);});
        GameObject useButton = newItemObj.transform.GetChild(0).GetChild(4).gameObject;
        inspectButton.SetActive(false);
        useButton.SetActive(false);
    }

    void AddItem(InventoryManager.ivItem item){
        GameObject newItemObj = Instantiate(itemPrefab,Vector3.zero,Quaternion.identity);
        newItemObj.transform.SetParent(ItemsContent.transform,false);
        newItemObj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = InventoryManager.LoadItemSprite(item.id);//image
        newItemObj.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.name;//name
        newItemObj.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = item.description;//description

        GameObject inspectButton = newItemObj.transform.GetChild(0).GetChild(3).gameObject;
        GameObject useButton = newItemObj.transform.GetChild(0).GetChild(4).gameObject;
        if (item.inspect != new string[]{""}){//inspect button
            inspectButton.GetComponent<Button>().onClick.AddListener(delegate {InspectItem(item);});//
        }else{
            inspectButton.SetActive(false);
        }
        
        if (item.methodName !="" || player.ValidRequiresItem()){//use button
            useButton.GetComponent<Button>().onClick.AddListener(delegate {UseItem(item);});//UseItem(item)
            if (player.ValidRequiresItem()){
                useButton.transform.GetChild(0).GetComponent<Text>().text = "use with";
            }
        }else{
            useButton.SetActive(false);
        }
    }
    
}
