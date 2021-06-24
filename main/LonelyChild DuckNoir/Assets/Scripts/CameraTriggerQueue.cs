using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerQueue : MonoBehaviour
{
    public List<CameraTrigger> cameras;
    CameraControl cameraControl;
    void Start(){
        cameras = new List<CameraTrigger>();
        cameraControl = GetComponent<CameraControl>();
    }
}
