using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableDoor : Activatable
{
    // Start is called before the first frame update
    Animator anim;
   [SerializeField] BoxCollider2D thisCol;
    [SerializeField] string toPlay = "DoorOpen";
   // [SerializeField] AudioSource aS;
    void Start()
    {
        anim = GetComponent<Animator>();
        if (thisCol == null)
        {
            thisCol = GetComponent<BoxCollider2D>();
        }
    }

    public override void Activate(){
        anim.Play(toPlay,0);
        //if(aS != null)
       // {
       //     aS.Play();
       // }
        thisCol.enabled = false;
    }
    public void Disable(){
        gameObject.SetActive(false);
    }
}
