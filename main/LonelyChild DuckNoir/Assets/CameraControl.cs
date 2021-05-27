using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject backupCam;
    public GameObject activeCam;
    void Start()
    {
        backupCam = GameObject.Find("BackupCamera");
        activeCam = backupCam;
    }

    public void ChangeCamera(CameraTrigger origin){
        activeCam.SetActive(false);
        origin.cam.gameObject.SetActive(true);
        activeCam = origin.cam.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
