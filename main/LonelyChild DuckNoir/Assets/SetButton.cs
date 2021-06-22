using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button restartLevelButton;
    GameSceneManager manager;
    void Start()
    {
        manager = GameObject.Find("PersistentManager").GetComponent<GameSceneManager>();
        restartLevelButton.onClick.AddListener(delegate {manager.LoadCheckpoint();}) ;
    }
}
