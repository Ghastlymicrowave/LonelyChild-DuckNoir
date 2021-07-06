using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : Activatable
{
    Animator anim;
    [SerializeField] GameObject downwardInteractable;
    [SerializeField] GameObject lookInteractable;
    [SerializeField] GameObject[] teethGroups;
    public List<EvilEye> eyesRequired;
    public bool open = false;
    void Start(){
        anim = GetComponent<Animator>();
    }
    public override void Activate()
    {
        teethGroups[eyesRequired.Count].SetActive(false);
        if (eyesRequired.Count==0){
            open = true;
            downwardInteractable.SetActive(true);
            lookInteractable.SetActive(false);
        }
        //play animation
    }
}
