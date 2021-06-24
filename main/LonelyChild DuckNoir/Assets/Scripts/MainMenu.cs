using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public int[] ghostIDs;
    public string ToLoad = "PlayroomWB";
    public player_main player_Main;

    public TextScroller textScroller;
    public int textID;

    TextManager tm;
    InventoryManager inventoryManager;
    GameSceneManager gameSceneManager;
    [SerializeField] OptionsMenu optionsPanel;

    void Start()
    {
        
        if (textScroller == null)
        {
            textScroller = FindObjectOfType<TextScroller>();
        }
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        gameSceneManager = tm.gameObject.GetComponent<GameSceneManager>();
    }

    public void PlayButton()
    {
        foreach (int ghost in ghostIDs)
        {
            inventoryManager.ghostsRoaming.Add(ghost);
        }
        //inventoryManager.playerPosOnStart = new Vector3(999999f, 999999f, 999999f);
        inventoryManager.ResetSave();
        gameSceneManager.LoadInitalScene(inventoryManager.checkpointScene);//to start new game, otherwise, load current data and use that scene
        
    }

    public void OpenOptions(){
        optionsPanel.gameObject.SetActive(true);
        optionsPanel.InitMenu();
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    public void HowToPlayButton()
    {
        if (optionsPanel!=null&&optionsPanel.gameObject.activeSelf){
            optionsPanel.CloseOptions();
        }
        if (textScroller.theCanvas.activeSelf == false)
        {
            string[] toScroll = TextManager.GetTextByID(textID);
            textScroller.ScrollText(toScroll, player_Main);
        }
    }
}
