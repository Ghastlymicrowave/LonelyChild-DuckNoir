using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilEye : Activatable
{
    // Start is called before the first frame update
    Mouth mouth;
    Animator anim;
    void Start(){
        anim = GetComponent<Animator>();
        mouth =  GameObject.Find("Mouth").GetComponent<Mouth>();
        mouth.eyesRequired.Add(this);
    }
    public override void Activate(){
        if (mouth.eyesRequired.Contains(this)){
            mouth.eyesRequired.Remove(this);
        }
        mouth.Activate();
        //anim.Play("EyePoked",0);
    }
}
