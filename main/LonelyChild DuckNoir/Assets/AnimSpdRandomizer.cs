using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpdRandomizer : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float minSpd;
    [SerializeField] float maxSpd;
    void Start(){
        animator.SetFloat("spd",Random.Range(minSpd,maxSpd));
    }
    public void RandomizeSpeed(){
        animator.SetFloat("spd",Random.Range(minSpd,maxSpd));
    }
}
