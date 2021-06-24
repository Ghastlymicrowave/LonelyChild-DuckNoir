using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{
    [Range(1f, 5f)] [SerializeField] float maxSpd = 2f;
    [Range(0.001f, 0.5f)] [SerializeField] float initalSpd = .1f;
    [SerializeField] float spdAccel = 0.2f;
    [Range(0f, 0.5f)] [SerializeField] float Kfriction = .2f;//in percent of speed
    [Range(0f, 0.5f)] [SerializeField] float Sfriction = .5f;//in units/s
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform bodyTransform;
    [SerializeField] float maxYRotation =20f;
    [SerializeField] float minYRotation =20f;
    [SerializeField] float xInital;
    [SerializeField] float xRange;
    [SerializeField] float yInital;
    [SerializeField] float yRange;
    [SerializeField] float zInital;
    [SerializeField] float zRange;
    [Range(0.01f,1f)]public float HmouseSmoothing = 0.3f;
    [Range(0.01f,1f)]public float VmouseSmoothing = 0.3f;
    [Range(0.01f,1f)]public float HmouseSensitivity = 0.3f;
    [Range(0.01f,1f)]public float VmouseSensitivity = 0.3f;
    float hinput;
    float vinput;
    float currentSpeed;
    bool mouseLocked = true;
    [SerializeField] Rigidbody2D rb;
    Vector2 currentRotation;
    Vector2 currentRotationLerpTarget;
    Vector3 cameraPositionLerpTarget;
    Vector3 currentCameraLocalPos;
    Vector3 currentCameraBounceback;
    Vector3 currentCameraBouncebackTarget;
    InventoryManager inventoryManager;
    [SerializeField] LayerMask cameraCollisionMask;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject persistentManager;
        persistentManager = GameObject.Find("PersistentManager");
        if (persistentManager!=null){
            inventoryManager = persistentManager.GetComponent<InventoryManager>();
            UpdateOptions(inventoryManager.gameObject.GetComponent<SettingsManager>().GetOptions());
        }else{
            Debug.Log("the player can't access the persistent manager! If you know this, ignore this message");
        }
        currentCameraBounceback = Vector3.zero;
        currentCameraLocalPos = Vector3.zero;
    }

    public void UpdateOptions(float[] inputArray){
        HmouseSmoothing = inputArray[0];
        VmouseSmoothing = inputArray[1];
        HmouseSensitivity = inputArray[2];
        VmouseSensitivity = inputArray[3];
    }
    
    //thisAnimator.SetBool("Moving",isMoving);
    /*if (isMoving)
    {
        if (currentSpd < initalSpd)
        {//moving from standstill
            currentSpd = initalSpd;
            //facing = new Vector2(hinput, vinput).normalized;
        }
        else
        {
            currentSpd = Mathf.Min(spdAccel + currentSpd, maxSpd);
            if (Vector2.Angle(new Vector2(hinput, vinput), facing) > 160f)
            {
                facing = Vector2.Lerp(facing, new Vector2(hinput, vinput).normalized, 0.51f).normalized;
            }
            else
            {
                facing = Vector2.Lerp(facing, new Vector2(hinput, vinput).normalized, rotatationSpd).normalized;//rotate angle with lerp
            }

        }
    }
    else
    {//not actively moving, apply friction
        currentSpd = Mathf.Max(0f, currentSpd - currentSpd * Kfriction);
        if (currentSpd < Sfriction)
        {
            currentSpd = 0;
        }
    }*/

    public static Vector2 Rotate(Vector2 v, float delta) {
        delta *= Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    void Look(){
        Vector2 rotation = Vector2.zero;
        rotation.x -= Input.GetAxis("Mouse X") * (HmouseSensitivity *1.5f);
        rotation.y += -Input.GetAxis("Mouse Y") * (VmouseSensitivity *1.5f);
        currentRotationLerpTarget += rotation;
        currentRotationLerpTarget.y = Mathf.Clamp(currentRotationLerpTarget.y,minYRotation,maxYRotation);

        currentRotation.x = Mathf.Lerp(currentRotation.x,currentRotationLerpTarget.x,HmouseSmoothing);
        currentRotation.y = Mathf.Lerp(currentRotation.y,currentRotationLerpTarget.y,VmouseSmoothing);
        //total range is max - min
        //set to 0 out of max
        cameraPositionLerpTarget.y = yInital + ((currentRotation.y-minYRotation)/(maxYRotation-minYRotation) * yRange);
        cameraPositionLerpTarget.z = zInital + ((currentRotation.y-minYRotation)/(maxYRotation-minYRotation) * zRange);
        cameraPositionLerpTarget.x = xInital + ((currentRotation.y-minYRotation)/(maxYRotation-minYRotation) * xRange);
        currentCameraLocalPos = Vector3.Lerp(currentCameraLocalPos,cameraPositionLerpTarget,1f);//TODO: camera positon smooth

        float castRadius = .5f;
        //this isn't exactly camPos transform point because the camera is a child of the camera container which is the thing that moves

        RaycastHit castInfo;

//go from player postion to camera position

        float camDistToPlayer = Mathf.Abs(cameraTransform.GetChild(0).localPosition.y) +.2f;

        //Collider[] overlap = Physics.OverlapSphere(cameraTransform.parent.TransformPoint(cameraPositionLerpTarget + cameraTransform.GetChild(0).localPosition),castRadius,cameraCollisionMask);
        Vector3 origin = cameraTransform.parent.TransformPoint(cameraPositionLerpTarget) + cameraTransform.GetChild(0).forward*0.75f ;
        Vector3 direction = -cameraTransform.GetChild(0).forward;
        Physics.SphereCast(origin,castRadius,direction,out castInfo,camDistToPlayer,cameraCollisionMask);
        Debug.DrawRay(origin,direction * camDistToPlayer,Color.green,0.1f);
        if (castInfo.collider!=null){
            Debug.Log(castInfo.collider.gameObject.name);
            currentCameraBouncebackTarget = -direction * (camDistToPlayer - castInfo.distance);//(Mathf.Clamp(-castInfo.distance,-2f,2f))
            //Debug.DrawLine(cameraTransform.parent.TransformPoint(cameraPositionLerpTarget + cameraTransform.GetChild(0).localPosition),castInfo.point,Color.cyan,2f);
        }else{
            currentCameraBouncebackTarget = Vector3.zero;
        }

        currentCameraBounceback = Vector3.Lerp(currentCameraBounceback,currentCameraBouncebackTarget,0.1f);//TODO: camera bounceback smooth
        
        cameraTransform.localPosition = currentCameraLocalPos;
        cameraTransform.position += currentCameraBounceback;
        //cameraTransform.position += currentCameraBounceback;
        
        
        Vector3 targetDirection = Rotate(Vector2.up,currentRotation.x);
        bodyTransform.rotation = Quaternion.LookRotation(targetDirection,new Vector3(0f,0f,-1f));
        targetDirection = cameraTransform.forward;
        Vector3 angle = Quaternion.AngleAxis(currentRotation.y,Vector3.up).eulerAngles;
        angle.z = 0f;
        cameraTransform.localRotation = Quaternion.Euler(new Vector3(currentRotation.y,0f,0f));
        //Debug.Log(currentRotation.x.ToString()+":"+currentRotation.y.ToString());
    }

    void Move(){
        hinput = Input.GetAxis("HMove");
        vinput = Input.GetAxis("VMove");
        bool isMoving = (hinput != 0f || vinput != 0f);
        Vector2 direction = new Vector2(hinput,vinput);
        if (isMoving){
            if (currentSpeed<initalSpd){//moving from standstill
                currentSpeed = initalSpd;
            }else{
                currentSpeed = Mathf.Min(spdAccel + currentSpeed, maxSpd);
            }
        }else{
            currentSpeed = Mathf.Max(0f,currentSpeed - currentSpeed * Kfriction);
            if (currentSpeed < Sfriction){
                currentSpeed = 0f;
            }
        }
        rb.velocity = Rotate(direction.normalized,currentRotation.x) * currentSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (mouseLocked)
        {
            Look();
            Move();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            mouseLocked = !mouseLocked;
            if (mouseLocked)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        
    }
}
