using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransTrigger : MonoBehaviour
{
    GameSceneManager manager;
    [SerializeField] string scene;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("PersistentManager").GetComponent<GameSceneManager>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Player"){
            manager.TransitionScene(scene);
        }
    }
}
