using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    int maxStatus;
    protected int clampStatus(int status){
        return Mathf.Clamp(status,0,maxStatus);
    }
    public virtual void SetStatus(int status){

    }
}
