using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCam : MonoBehaviour
{
    [SerializeField] bool verticallyRotate = false;
    void Update()
    {
        if (verticallyRotate){
            transform.LookAt(Camera.main.transform.position, Vector3.back);
        }else{
            Vector3 pos = Camera.main.transform.position;
            pos.z = transform.position.z;
            transform.LookAt(pos, Vector3.back);
        }
        
    }
}
