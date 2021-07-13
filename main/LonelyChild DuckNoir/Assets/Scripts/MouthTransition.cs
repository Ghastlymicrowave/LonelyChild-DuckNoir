using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthTransition : Activatable
{
    [SerializeField] Animator anim;
    void Start(){
        anim = GetComponent<Animator>();
    }
    public override void Activate()
    {
        anim.Play("Trans");
    }
}
