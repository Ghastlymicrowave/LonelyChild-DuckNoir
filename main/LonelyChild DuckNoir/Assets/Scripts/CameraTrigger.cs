using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public BoxCollider2D boxCol;
    public triggerType type;
    public AllowedAxis axis;
    public enum triggerType{
        INSTANT,
        TIME
    }

    public enum AllowedAxis{
        X,
        Y,
        Z
    }

    public enum TransitionCurve{
        LINEAR,
        INOUTSIN,
    }
    public static float followSpeed = 0.2f;
    public float transitionTime;
    public TransitionCurve curve;
    [HideInInspector] public GameObject cam;
    
    public static CameraControl camControl;
    public static GameObject player;

    Vector3 transStartPos;
    Vector3 transEndPos;
    float transTime;
    Quaternion originalAngle;
    void Awake(){
        cam = transform.GetChild(0).gameObject;
        transEndPos = cam.transform.position;
        originalAngle = cam.transform.rotation;
        cam.SetActive(false);
    }
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        
        if (camControl==null){camControl = GameObject.Find("CameraControl").GetComponent<CameraControl>();}
        if (player==null){player = GameObject.Find("Player");}
        transTime = transitionTime;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Triggered();
        Debug.Log("Triggered");
    }

    public void Triggered(){
        if (type==triggerType.INSTANT){
            camControl.ChangeCamera(this);
        }else{
            if (camControl.activeCam!=cam){
                transStartPos = camControl.activeCam.transform.position;
                cam.transform.rotation = camControl.activeCam.transform.rotation;
                camControl.ChangeCamera(this);
                cam.transform.position = transStartPos;
                transTime = 0f;
            }
        }
    }

    public Quaternion SetAxis(Quaternion inAngles){
        Vector3 angles = originalAngle.eulerAngles;
        Vector3 inAngle = inAngles.eulerAngles;
        switch(axis){
            case AllowedAxis.X:
                return Quaternion.Euler(inAngle.x,angles.y,angles.z);
            case AllowedAxis.Y:
                return Quaternion.Euler(angles.x,inAngle.y,angles.z);
            case AllowedAxis.Z:
                return Quaternion.Euler(angles.x,angles.y,inAngle.z);
            default:
                return Quaternion.identity;
        }
    }

    private void LateUpdate(){
        //TODO: Rotate to face player
        if (transTime<transitionTime){
            transTime+=Time.deltaTime;
            if (transTime >= transitionTime){
                transTime = transitionTime;
                cam.transform.position = Vector3.Lerp(transStartPos,transEndPos,1f); 
            }else{
                float percent = (transTime/transitionTime);
                switch(curve){
                    case TransitionCurve.INOUTSIN:
                        percent = (float)System.Math.Sin((double)(percent*Mathf.PI - Mathf.PI/2)) *.5f+.5f;
                    break;
                    case TransitionCurve.LINEAR:
                    break;
                }
                float follow = followSpeed * percent;
                
                Quaternion a = cam.transform.rotation;
                Quaternion b = Quaternion.LookRotation(player.transform.position - cam.transform.position, new Vector3(0f,0f,-1f));
                cam.transform.rotation = Quaternion.Lerp(a,b,follow);
                cam.transform.position = Vector3.Lerp(transStartPos,transEndPos,percent);
            }
        }else{
            if (camControl.activeCam == cam){
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation,Quaternion.LookRotation(player.transform.position - cam.transform.position, new Vector3(0f,0f,-1f)),followSpeed);
            }
        }
    }
}
