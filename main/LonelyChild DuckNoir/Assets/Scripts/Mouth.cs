using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : Activatable
{
    Animator anim;
    [SerializeField] GameObject downwardInteractable;
    [SerializeField] GameObject lookInteractable;
    [SerializeField] GameObject[] teethGroups;
    [SerializeField] SimpleSoundCue activate;
    [SerializeField] SimpleSoundCue enter;
    public List<EvilEye> eyesRequired;
    public bool open = false;
    void Start(){
        anim = GetComponent<Animator>();
    }
    public override void Activate()
    {
        if (eyesRequired.Count<3){
            teethGroups[eyesRequired.Count].SetActive(false);
        }
        if (open){
            anim.Play("Trans",0);
            enter.Play();
        }else if (eyesRequired.Count==0){
            activate.Play();
            open = true;
            downwardInteractable.SetActive(true);
            lookInteractable.SetActive(false);
        }
    }
}
