using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{
    [Range(0.001f, 0.5f)] [SerializeField] float maxSpd = 2f;
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
    float hinput;
    float vinput;
    float currentSpeed;
    bool mouseLocked = true;
    [SerializeField] Rigidbody2D rb;
    Vector2 currentRotation;
    Vector2 currentRotationLerpTarget;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        rotation.x -= Input.GetAxis("Mouse X");
        rotation.y += -Input.GetAxis("Mouse Y");
        currentRotationLerpTarget += rotation;
        currentRotationLerpTarget.y = Mathf.Clamp(currentRotationLerpTarget.y,minYRotation,maxYRotation);

        currentRotation.x = Mathf.Lerp(currentRotation.x,currentRotationLerpTarget.x,HmouseSmoothing);
        currentRotation.y = Mathf.Lerp(currentRotation.y,currentRotationLerpTarget.y,VmouseSmoothing);
        Vector3 camPos = cameraTransform.localPosition;
        //total range is max - min
        //set to 0 out of max
        camPos.y = yInital + ((currentRotation.y-minYRotation)/(maxYRotation-minYRotation) * yRange);
        camPos.z = zInital + ((currentRotation.y-minYRotation)/(maxYRotation-minYRotation) * zRange);
        camPos.x = xInital + ((currentRotation.y-minYRotation)/(maxYRotation-minYRotation) * xRange);
        cameraTransform.localPosition = camPos;

        Vector3 targetDirection = Rotate(Vector2.up,currentRotation.x);
        bodyTransform.rotation = Quaternion.LookRotation(targetDirection,new Vector3(0f,0f,-1f));
        targetDirection = cameraTransform.forward;
        Vector3 angle = Quaternion.AngleAxis(currentRotation.y,Vector3.up).eulerAngles;
        angle.z = 0f;
        cameraTransform.localRotation = Quaternion.Euler(new Vector3(currentRotation.y,0f,0f));
        Debug.Log(currentRotation.x.ToString()+":"+currentRotation.y.ToString());
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
