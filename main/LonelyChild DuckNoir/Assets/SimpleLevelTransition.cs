using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLevelTransition : MonoBehaviour
{
    [SerializeField] string levelToChangeTo;
    public void Transition(){
        GameObject.Find("PersistentManager").GetComponent<GameSceneManager>().TransitionScene(levelToChangeTo);
    }
}
