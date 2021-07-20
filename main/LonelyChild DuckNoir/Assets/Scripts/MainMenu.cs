using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    public bool IsMain = false;

    void Start()
    {

        if (textScroller == null)
        {
            textScroller = FindObjectOfType<TextScroller>();
        }
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        gameSceneManager = tm.gameObject.GetComponent<GameSceneManager>();

        if (IsMain){
            Debug.Log(inventoryManager.IsFreshSave());
            if (!inventoryManager.IsFreshSave())
            {
                ContinueButton.SetActive(true);
                inventoryManager.LoadJSON();
            }
            else
            {
                inventoryManager.ResetSave();
                inventoryManager.LoadJSON();
            }
        }

       

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; //just in case it got reset from pause menu
    }

    public void PlayButton()
    {
        CloseStuff();
        inventoryManager.ResetSave();
        inventoryManager.LoadJSON();
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
        Debug.Log("CHECKPOINT SCENE: "+inventoryManager.checkpointScene);
        gameSceneManager.LoadScene(inventoryManager.checkpointScene);
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
        credits.transform.GetChild(1).GetChild(2).GetComponent<Scrollbar>().value = 1f;
    }

    public void LoadMainMenu(){
        gameSceneManager.LoadScene("MainMenu");
    }
    
    public void CloseStuff()
    {
        if (optionsPanel != null && optionsPanel.gameObject.activeSelf)
        {
            optionsPanel.Cancel();
        }
        if (credits != null){
        credits.SetActive(false);
        }
        if (howToPlay!=null){
        howToPlay.SetActive(false);
        }

    }






}
