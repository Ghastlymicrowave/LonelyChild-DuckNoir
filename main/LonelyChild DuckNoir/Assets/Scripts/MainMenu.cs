using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public int[] ghostIDs;
    public ThirdPersonPlayer player_Main;
    public TextScroller textScroller;
    public int textID;

    TextManager tm;
    InventoryManager inventoryManager;
    GameSceneManager gameSceneManager;
    [SerializeField] OptionsMenu optionsPanel;
    [SerializeField] GameObject ContinueButton;

    public GameObject howToPlay;
    public GameObject credits;

    void Start()
    {

        if (textScroller == null)
        {
            textScroller = FindObjectOfType<TextScroller>();
        }
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        gameSceneManager = tm.gameObject.GetComponent<GameSceneManager>();

        if (!inventoryManager.IsFreshSave())
        {
            ContinueButton.SetActive(true);
        }
        else
        {
            inventoryManager.ResetSave();
            inventoryManager.LoadJSON();
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; //just in case it got reset from pause menu
    }

    public void PlayButton()
    {
        CloseStuff();
        foreach (int ghost in ghostIDs)
        {
            inventoryManager.ghostsRoaming.Add(ghost);
        }
        inventoryManager.ResetSave();
        gameSceneManager.TransitionScene("SecondFloor");//to start new game, otherwise, load current data and use that scene

    }

    public void OpenOptions()
    {
        CloseStuff();
        optionsPanel.gameObject.SetActive(true);
        optionsPanel.InitMenu();
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    public void Continue()
    {
        gameSceneManager.TransitionScene(inventoryManager.checkpointScene);
    }
    public void HowToPlayButton()
    {
        CloseStuff();
        howToPlay.SetActive(true);
        /*
        //Legacy How to play code
        if (textScroller.theCanvas.activeSelf == false)
        {
            string[] toScroll = TextManager.GetTextByID(textID);
            textScroller.ScrollText(toScroll, player_Main);
        }
        */
    }
    public void CreditsButton()
    {
        CloseStuff();
        credits.SetActive(true);
    }
    
    public void CloseStuff()
    {
        if (optionsPanel != null && optionsPanel.gameObject.activeSelf)
        {
            optionsPanel.Cancel();
        }
        credits.SetActive(false);
        howToPlay.SetActive(false);

    }






}
