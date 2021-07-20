using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    int maxStatus;
    [SerializeField] int maxRand = 1;
    [SerializeField] float minSpd = 0.8f;
    [SerializeField] float maxSpd = 1.2f;
    [SerializeField] SpriteRenderer thisSprite;
    [SerializeField] Sprite crucifiedSprite;
    [SerializeField] Sprite ascendedSprite;
    [SerializeField] Sprite weakendedSprite;
    Animator animator;
    battleBehavior battleBehavior;
    void Start(){
        animator = GetComponent<Animator>();
        battleBehavior = GameObject.FindObjectOfType<battleBehavior>();
    }
    public void SetRand(){
        if (animator!=null){
            
        }else{
            Debug.Log("Animator is null!");
            animator = GetComponent<Animator>();
            if (animator!=null){
                animator.SetFloat("RandomSpeed",Random.Range(minSpd,maxSpd));
                animator.SetInteger("RandomAnim",Random.Range(0,maxRand));
            }else{
                Debug.Log("animator still null, must not be attached");
            }
            
        }
        
    }
    protected int clampStatus(int status){
        return Mathf.Clamp(status,0,maxStatus);
    }
    public virtual void SetStatus(int status){

    }

    public void Ascended(){
        if (ascendedSprite!=null){
            thisSprite.sprite = ascendedSprite;
        }
    }
    public void Crucified(){
        if (crucifiedSprite!=null){
            thisSprite.sprite = crucifiedSprite;
        }
    }
    public void Weakened(){
        if (weakendedSprite!=null){
            thisSprite.sprite = weakendedSprite;
        }
    }

    public void Attack(){
        animator.Play("Attack");
    }
    public void SetIdleSpd(float spd){
        animator.SetFloat("IdleSpeed",spd);
    }
    public void EndAnim(battleBehavior.endCon condition){
        switch(condition){
            case battleBehavior.endCon.SENTIMENT:
            animator.Play("Sentiment");

            break;
            case battleBehavior.endCon.RUN:
            animator.Play("Run");

            break;
            case battleBehavior.endCon.CRUCIFIX:
            animator.Play("Crucifix");
            break;
        }
    }

    public void EndCombat(){
        battleBehavior.EndCombat();
    }
}
