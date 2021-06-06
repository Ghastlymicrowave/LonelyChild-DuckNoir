using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    // Start is called before the first frame update
    virtual public void Activate(){
        Debug.Log("Activate not set up for " + this.GetType().ToString());
    }
}