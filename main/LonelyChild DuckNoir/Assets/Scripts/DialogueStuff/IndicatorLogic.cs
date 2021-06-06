using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLogic : MonoBehaviour
{
    static CameraControl camControl;
    public Interactable interactable;
    private void Start(){
        if (camControl==null){
            camControl=GameObject.Find("CameraControl").GetComponent<CameraControl>();
        }
    }
    void OnMouseDown()
    {
        if (!interactable.isBusy)
        {
            //interactable.isReady = true;
        }
    }
    
    private void Update(){
        transform.parent.transform.LookAt(camControl.activeCam.transform.position,Vector3.back);
    }
}
