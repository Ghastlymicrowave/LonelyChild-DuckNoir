using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLogic : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    void OnMouseDown()
    {
        if (!interactable.isBusy)
        {
            interactable.isReady = true;
        }
    }
    
    private void Update(){
        transform.parent.transform.LookAt(Camera.main.transform.position,Vector3.back);
    }
}
