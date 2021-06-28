using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{
    bool lookSide = true;
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
    [Range(0.01f,1f)]public float CameraSmooth = 0.3f;
    float hinput;
    float vinput;
    float currentSpeed;
    bool mouseLocked = true;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 currentRotation;
    [SerializeField] Vector2 currentRotationLerpTarget;
    Vector3 cameraPositionLerpTarget;
    Vector3 currentCameraLocalPos;
    Vector3 currentCameraBounceback;
    Vector3 currentCameraBouncebackTarget;
    InventoryManager inventoryManager;
    GameSceneManager gameSceneManager;
    TextManager tm;
    [SerializeField] LayerMask cameraCollisionMask;
    public TextScroller textScroller;
    Interactable interactableTarget;
    [SerializeField] GameObject interactHitbox;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool InventoryOpen = false;
    float dontUseTime = .1f;
    Animator thisAnimator;

    Vector2 lockedRotation = Vector2.zero;

    Quaternion spotlightCurrent;
    Quaternion spotlightLerp;
    [SerializeField] Transform spotlight;
    [SerializeField] Transform camLight;
    bool UseCamLight = false;
    
    void Start()
    {
        thisAnimator = GetComponent<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject persistentManager;
        persistentManager = GameObject.Find("PersistentManager");
        if (persistentManager!=null){
            inventoryManager = persistentManager.GetComponent<InventoryManager>();
            gameSceneManager = persistentManager.GetComponent<GameSceneManager>();
            tm = persistentManager.GetComponent<TextManager>();
            UpdateOptions(inventoryManager.gameObject.GetComponent<SettingsManager>().GetOptions());
        }else{
            Debug.LogWarning("the player can't access the persistent manager! If you know this, ignore this message");
        }
        currentCameraBounceback = Vector3.zero;
        currentCameraLocalPos = Vector3.zero;
        if (textScroller == null)
        {
            textScroller = FindObjectOfType<TextScroller>();
        }
        spotlightCurrent = Quaternion.LookRotation(new Vector3(0,1f,0f),Vector3.back);
        spotlightLerp = Quaternion.LookRotation(new Vector3(0,1f,0f),Vector3.back);
        CheckCams();
    }

    public void UpdateOptions(float[] inputArray){
        HmouseSmoothing = Mathf.Clamp(1-Mathf.Pow(inputArray[0],2),0.001f,1f);
        VmouseSmoothing = Mathf.Clamp(1-Mathf.Pow(inputArray[1],2),0.001f,1f);
        HmouseSensitivity = inputArray[2];
        VmouseSensitivity = inputArray[3];
        CameraSmooth = Mathf.Clamp(1-Mathf.Pow(inputArray[6],2),0.001f,1f);
        //.75 - 1*.74
        //.75 - 0
        CheckCams();
        
    }

    public void CheckCams(){
        camLight.gameObject.SetActive(UseCamLight);
        spotlight.gameObject.SetActive(!UseCamLight);
    }

    public bool ValidRequiresItem(){
        if (interactableTarget!=null){
            return interactableTarget.HasItemUse();
        }else{
            return false;
        }
        
    }

    public static Vector2 Rotate(Vector2 v, float delta) {
        delta *= Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    void Look(bool locked = false){
        Vector2 rotation = Vector2.zero;
        rotation.x -= Input.GetAxis("Mouse X") * (HmouseSensitivity *1.5f);
        rotation.y += -Input.GetAxis("Mouse Y") * (VmouseSensitivity *1.5f);
        if (Input.GetButtonDown("changeLookSide")){
            lookSide = !lookSide;
        }
        if (!locked){
            currentRotationLerpTarget += rotation;
            currentRotationLerpTarget.y = Mathf.Clamp(currentRotationLerpTarget.y,minYRotation,maxYRotation);
        }else{
            currentRotationLerpTarget += rotation * .5f;
            currentRotationLerpTarget.y = Mathf.Clamp(currentRotationLerpTarget.y,minYRotation,maxYRotation);
            currentRotationLerpTarget.x = Mathf.Lerp(currentRotationLerpTarget.x,lockedRotation.x,0.01f);
        }
        currentRotation.x = Mathf.Lerp(currentRotation.x,currentRotationLerpTarget.x,HmouseSmoothing);
        currentRotation.y = Mathf.Lerp(currentRotation.y,currentRotationLerpTarget.y,VmouseSmoothing);
        //total range is max - min
        //set to 0 out of max
        cameraPositionLerpTarget.y = yInital + ((currentRotation.y-minYRotation)/(maxYRotation-minYRotation) * yRange);
        cameraPositionLerpTarget.z = zInital + ((currentRotation.y-minYRotation)/(maxYRotation-minYRotation) * zRange);
        cameraPositionLerpTarget.x = xInital + ((currentRotation.y-minYRotation)/(maxYRotation-minYRotation) * xRange);
        if (!lookSide){
            cameraPositionLerpTarget.x = -cameraPositionLerpTarget.x;
        }
        currentCameraLocalPos = Vector3.Lerp(currentCameraLocalPos,cameraPositionLerpTarget,CameraSmooth);//TODO: camera positon smooth

        float castRadius = .2f;

        //go from player postion to camera position

        float camDistToPlayer = Mathf.Abs(cameraTransform.GetChild(0).localPosition.magnitude) + 0.2f;
        Vector3 origin = cameraTransform.parent.TransformPoint(Vector3.zero) ;
        Vector3 offset = cameraTransform.GetChild(0).transform.position - cameraTransform.position ;
        Vector3 direction = cameraTransform.parent.TransformPoint(cameraPositionLerpTarget) + offset - origin;
        
        RaycastHit hit;

        Physics.SphereCast(origin, castRadius, direction,out hit, camDistToPlayer, cameraCollisionMask);
        if (hit.collider!=null){
            Vector3 point =hit.point + castRadius * hit.normal;
            Vector3 norm = cameraTransform.parent.TransformPoint(cameraPositionLerpTarget) - point;
            Vector3 vec = point - (cameraTransform.parent.TransformPoint(cameraPositionLerpTarget) + offset);
            //find vector between actual cam and point

            currentCameraBouncebackTarget = vec -direction.normalized*.2f;
            Debug.DrawLine(origin,hit.point,Color.black,1f);

            if (currentCameraBounceback.magnitude > currentCameraBouncebackTarget.magnitude){
                currentCameraBounceback = Vector3.Lerp(currentCameraBounceback,currentCameraBouncebackTarget,0.5f);
            }
            currentCameraBounceback = Vector3.Lerp(currentCameraBounceback,currentCameraBouncebackTarget,0.5f);
        }else{
            currentCameraBouncebackTarget = Vector3.zero;
            currentCameraBounceback = Vector3.Lerp(currentCameraBounceback,currentCameraBouncebackTarget,0.01f);
        }

        //TODO: camera bounceback smooth
        
        cameraTransform.localPosition = currentCameraLocalPos;
        cameraTransform.position += currentCameraBounceback;
        
        
        Vector3 targetDirection = Rotate(Vector2.up,currentRotation.x);
        bodyTransform.rotation = Quaternion.LookRotation(targetDirection,new Vector3(0f,0f,-1f));
        targetDirection = cameraTransform.forward;
        Vector3 angle = Quaternion.AngleAxis(currentRotation.y,Vector3.up).eulerAngles;
        angle.z = 0f;
        cameraTransform.localRotation = Quaternion.Euler(new Vector3(currentRotation.y,0f,0f));
    }

    void Move(){
        hinput = Input.GetAxis("HMove");
        vinput = Input.GetAxis("VMove");
        bool isMoving = (hinput != 0f || vinput != 0f);
        thisAnimator.SetBool("Moving",isMoving);
        Vector2 direction = new Vector2(hinput,vinput);
        if (isMoving){
            if (currentSpeed<initalSpd){//moving from standstill
                currentSpeed = initalSpd;
            }else{
                currentSpeed = Mathf.Min(spdAccel + currentSpeed, maxSpd);
            }

            if (UseCamLight){
                thisAnimator.SetFloat("DirectionX",0f);
                thisAnimator.SetFloat("DirectionY",1f);
            }else{
                thisAnimator.SetFloat("DirectionX",hinput);
                thisAnimator.SetFloat("DirectionY",vinput);
            }
            
            spotlightLerp = Quaternion.LookRotation(new Vector3(direction.x,direction.y,0f),Vector3.back);
        }else{
            currentSpeed = Mathf.Max(0f,currentSpeed - currentSpeed * Kfriction);
            if (currentSpeed < Sfriction){
                currentSpeed = 0f;
            }
        }
        rb.velocity = Rotate(direction.normalized,currentRotation.x) * currentSpeed;
    }


    public void SetMouseMode(bool locked){
            if (!locked)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (!UseCamLight){
            spotlightCurrent = Quaternion.Lerp(spotlightCurrent,spotlightLerp,0.2f);
            spotlight.localRotation = spotlightCurrent;
        }
        if (canMove&&!InventoryOpen)
        {
            Look();
            Move();
            
        }
        else
        {
            if (!InventoryOpen){
                Look(true);
            }
            

            if (interactableTarget!=null){
                interactableTarget.isBusy = true;
            }
            thisAnimator.SetBool("Moving",false);
            return;
        }

        if (InventoryOpen){
            thisAnimator.SetBool("Moving",false);
            return;
        }

        if (interactableTarget != null)
        {
            interactableTarget.isBusy = false;
        }
        
        if (interactableTarget != null && canMove)
        {
            if (interactableTarget.isReady||Input.GetButtonDown("Interact") && dontUseTime ==0)//if triggered from mouse click or interact button
            {
                interactableTarget.Trigger();
                lockedRotation = currentRotationLerpTarget;
                
                dontUseTime = .2f;
            }
        }
        

        if (Input.GetButtonDown("Pause")){
            SetMouseMode(false);
            gameSceneManager.Pause();
        }

        if (dontUseTime > 0 ){
            dontUseTime = Mathf.Max(dontUseTime - Time.deltaTime,0f);
        }

    }

    public void InteractableEntered(Interactable thisInteractable)
    {
        //the change 8:49 pm
        if(interactableTarget != null)
        {
            InteractableLeft(interactableTarget);
        }
        if (interactableTarget != thisInteractable)
        {
            interactableTarget = thisInteractable;
            interactableTarget.indicator.SetActive(true);
        }
    }
    public void InteractableLeft(Interactable thisInteractable)
    {
        if (interactableTarget == thisInteractable)
        {
            interactableTarget.indicator.SetActive(false);
            interactableTarget = null;
        }
    }

    public void UseItemOnInteractable(InventoryManager.ivItem item){
        if(interactableTarget!=null){
            interactableTarget.CheckItemUse(item);
        }else{
            UseItem(item);
        }
    }
    public void UseItem(InventoryManager.ivItem item){//if can't use on interactable, use normally
        if (item.methodName!=""){
            Invoke(item.methodName,0f);
        }
    }

    public void TriggerDialogue(int textID){
        Debug.Log("triggering dialogue: "+textID.ToString());
        string[] toScroll = TextManager.GetTextByID(textID);
        textScroller.ScrollText(toScroll, this);
    }
    public void TriggerDialogue(string[] text){
        string[] toScroll = text;
        textScroller.ScrollText(toScroll, this);
    }
}
