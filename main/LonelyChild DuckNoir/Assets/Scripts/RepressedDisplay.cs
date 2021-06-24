using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepressedDisplay : DisplayEnemy
{
    SpriteRenderer rend;
    Animator anim;
    int status;
    void Start(){
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    [SerializeField] Sprite[] statusSprites;//
    //happy
    //crying happy
    //crying sad 
    //angry
    public override void SetStatus(int newstatus)
    {
        status = base.clampStatus(newstatus);
        
        anim.Play("GhostBounce",0);
        //trigger a bounce
    }
    public void ChangeSprite(){
        rend.sprite = statusSprites[status];
    }
}
