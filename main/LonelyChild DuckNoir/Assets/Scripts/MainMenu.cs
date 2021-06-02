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

    void Start()
    {
        if (textScroller == null)
        {
            textScroller = FindObjectOfType<TextScroller>();
        }
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayButton()
    {
        foreach (int ghost in ghostIDs)
        {
            inventoryManager.ghostsRoaming.Add(ghost);
        }
        SceneManager.LoadScene(ToLoad);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void HowToPlayButton()
    {
        if (textScroller.theCanvas.active == false)
        {
            string[] toScroll = TextManager.GetTextByID(textID);
            textScroller.ScrollText(toScroll, player_Main);
        }
    }
}
