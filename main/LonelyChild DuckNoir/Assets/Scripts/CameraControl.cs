using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject backupCam;
    public GameObject activeCam;
    public List<CameraTrigger> cameras;
    CameraControl cameraControl;
    void Start()
    {
        backupCam = GameObject.Find("BackupCamera");
        activeCam = backupCam;
        cameras = new List<CameraTrigger>();
        cameraControl = GetComponent<CameraControl>();
    }

    public void ChangeCamera(CameraTrigger origin){
        activeCam.SetActive(false);
        origin.cam.gameObject.SetActive(true);
        activeCam = origin.cam.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameras.Count>0){
            if (cameras[0]!=activeCam){
                cameras[0].Triggered();
            }
        }
    }
}
