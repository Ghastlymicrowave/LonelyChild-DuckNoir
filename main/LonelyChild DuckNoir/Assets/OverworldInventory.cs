using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Combat;
public class OverworldInventory : MonoBehaviour
{
    bool menuOpen = false;
    [SerializeField] Button toggleButton;
    [SerializeField] Animator animator;
    Text buttonTxt;
    [SerializeField] GameObject itemPrefab;
    GameObject content;
    player_main player;
    InventoryManager inventoryManager;
    void Start()
    {
        buttonTxt = toggleButton.transform.GetChild(0).GetComponent<Text>();
        content = transform.GetChild(0).GetChild(0).gameObject;
        player = GameObject.Find("Player").GetComponent<player_main>();
        inventoryManager = GameObject.Find("PersistentManager").GetComponent<InventoryManager>();
    }

    public void ToggleMenu(){
        menuOpen = !menuOpen;
        if (menuOpen){
            buttonTxt.text="Close";
            animator.Play("Open",0);
            GenerateItems();
        }else{
            buttonTxt.text="Open";
            animator.Play("Close",0);
        }
    }

    void GenerateItems(){
        for(int i = content.transform.childCount-1; i > -1; i -= 1){
            Destroy(content.transform.GetChild(i).gameObject);
        }
        foreach( InventoryManager.ivItem i in inventoryManager.items){
            AddItem(i);
        }
    }

    public void UseItem(InventoryManager.ivItem item){
        Debug.Log("using");
        player.UseItemOnInteractable(item);
    }

    public void InspectItem(InventoryManager.ivItem item){
        Debug.Log("inspecting");
        player.TriggerDialogue(item.inspect);
    }

    void AddItem(InventoryManager.ivItem item){
        GameObject newItemObj = Instantiate(itemPrefab,Vector3.zero,Quaternion.identity);
        newItemObj.transform.SetParent(content.transform);
        newItemObj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = InventoryManager.LoadItemSprite(item.id);//image
        newItemObj.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.name;//name
        newItemObj.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = item.description;//description

        GameObject useButton = newItemObj.transform.GetChild(0).GetChild(4).gameObject;
        GameObject inspectButton = newItemObj.transform.GetChild(0).GetChild(3).gameObject;
        if (item.inspect != new string[]{""}){//inspect button
            inspectButton.GetComponent<Button>().onClick.AddListener(delegate {InspectItem(item);});
        }else{
            inspectButton.SetActive(false);
        }
        
        if (item.methodName !=""){//use button
            useButton.GetComponent<Button>().onClick.AddListener(delegate {UseItem(item);});
        }else{
            useButton.SetActive(false);
        }
        
    }
    
}
