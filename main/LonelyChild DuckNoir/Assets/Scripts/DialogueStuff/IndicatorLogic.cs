using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLogic : MonoBehaviour
{
    
    public Interactable interactable;
    void OnMouseDown()
    {
        interactable.isReady = true;
    }
}
