using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableDoor : Activatable
{
    // Start is called before the first frame update
    Animator anim;
    BoxCollider2D thisCol;
    void Start()
    {
        anim = GetComponent<Animator>();
        thisCol = GetComponent<BoxCollider2D>();
    }

    public override void Activate(){
        anim.Play("DoorOpen",0);
        thisCol.enabled = false;
    }
    public void Disable(){
        gameObject.SetActive(false);
    }
}
