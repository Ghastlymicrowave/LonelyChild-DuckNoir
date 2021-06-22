using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCam : MonoBehaviour
{
    CameraControl cameraControl;
    void Start()
    {
        cameraControl = GameObject.Find("CameraControl").GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraControl.activeCam.transform.position, Vector3.back);
    }
}
