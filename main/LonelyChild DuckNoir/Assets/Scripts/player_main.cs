using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_main : MonoBehaviour
{
    [Range(0.001f, 0.5f)] [SerializeField] float maxSpd = 2f;
    [Range(0.001f, 0.5f)] [SerializeField] float initalSpd = .1f;
    [Range(0.1f, 5f)] [SerializeField] float secToMax = 2f;//seconds it takes to reach max spd from itial speed
    [Range(0f, 0.5f)] [SerializeField] float Kfriction = .2f;//in percent of speed
    [Range(0f, 0.5f)] [SerializeField] float Sfriction = .5f;//in units/s

    [SerializeField] float rotatationSpd = 0.1f;
    [SerializeField] float interactHitboxOffset = 1f;

    float hinput = 0f;
    TextManager tm;
    InventoryManager inventoryManager;
    float vinput = 0f;
    bool isMoving = false;
    float currentSpd = 0;
    [SerializeField] float spdAccel;
    Vector2 facing = Vector2.zero;
    Rigidbody2D rb;
    Interactable interactableTarget;
    GameObject interactHitbox;

    public TextScroller textScroller;
    public bool canMove = true;
    public bool InventoryOpen = false;
    private CameraControl camControl;
    private Animator thisAnimator;
    private GameObject spriteObj;
    private GameSceneManager gameSceneManager;
    private float dontUseTime = .1f;
    private AudioSource overworldAudio;

    //playerposonstart
    public bool MovePlayerOnStart = true;
      [HideInInspector] public CameraTrigger cam;

    void Start()
    {
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        gameSceneManager = tm.gameObject.GetComponent<GameSceneManager>();
        overworldAudio = GetComponent<AudioSource>();
        overworldAudio.clip = gameSceneManager.GetOverworldAudio();
        overworldAudio.Play();
        spriteObj = transform.GetChild(1).gameObject;
        thisAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        interactableTarget = null;
        interactHitbox = transform.GetChild(0).gameObject;
        camControl = GameObject.Find("CameraControl").GetComponent<CameraControl>();


        if (textScroller == null)
        {
            textScroller = FindObjectOfType<TextScroller>();
        }
    }

    public static Vector2 rotate(Vector2 v, float delta) {
        delta *= Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    static Vector2 angleToVec2(float angle){
        angle *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
    }

    public bool ValidRequiresItem(){
        if (interactableTarget!=null){
            return interactableTarget.HasItemUse();
        }else{
            return false;
        }
        
    }

    void Update()
    {
        Vector3 camPos = camControl.activeCam.transform.position;
        camPos.z = spriteObj.transform.position.z;
        spriteObj.transform.LookAt(camPos,Vector3.back);
        if (!canMove)
        {
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
        hinput = Input.GetAxis("HMove");
        vinput = Input.GetAxis("VMove");
        isMoving = (hinput != 0f || vinput != 0f);
        thisAnimator.SetBool("Moving",isMoving);
        if (isMoving)
        {
            if (currentSpd < initalSpd)
            {//moving from standstill
                currentSpd = initalSpd;
                facing = new Vector2(hinput, vinput).normalized;
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
        }

        

        if (currentSpd != 0)
        {
            Vector2 standardFacing = new Vector2(facing.x, facing.y) * currentSpd;

            Vector2 vectorOffset = transform.position - camControl.activeCam.transform.position;
            vectorOffset = vectorOffset.normalized;
            float angleOffset = Mathf.Atan2( vectorOffset.x,vectorOffset.y )  * Mathf.Rad2Deg; 
            standardFacing = rotate(standardFacing,-angleOffset);

            Debug.DrawLine(camControl.activeCam.transform.position,camControl.activeCam.transform.position+(Vector3)angleToVec2(-angleOffset+90)*20f);
            //Debug.DrawLine(transform.position,transform.position+(Vector3)angleToVec2(-angleOffset-90)*20f);

            rb.MovePosition((Vector2)transform.position + standardFacing);
            float facingOffset = Mathf.Atan2( standardFacing.x,standardFacing.y )  * Mathf.Rad2Deg; 
            //Debug.Log(facingOffset-angleOffset);
            //facing out, -angleOffset+90 and updated facing face the same way

            thisAnimator.SetFloat("Angle",facingOffset-angleOffset);
            //UpdateSpriteFacing(-angleOffset);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        interactHitbox.transform.localPosition = facing * interactHitboxOffset;

        if (interactableTarget != null && canMove)
        {
            if (interactableTarget.isReady||Input.GetButtonDown("Interact") && dontUseTime ==0)//if triggered from mouse click or interact button
            {
                interactableTarget.Trigger();
                dontUseTime = .2f;
            }
        }
        

        if (Input.GetButton("Pause")){
            gameSceneManager.Pause();
        }

        if (dontUseTime > 0 ){
            dontUseTime = Mathf.Max(dontUseTime - Time.deltaTime,0f);
        }

        /*
        if (Input.GetKeyDown(KeyCode.K)){
            inventoryManager.AddAttack(Combat.AttackActions.Flashlight);
            inventoryManager.AddAttack(Combat.AttackActions.Theremin);
            inventoryManager.AddAttack(Combat.AttackActions.Fire_Poker);
            inventoryManager.AddAttack(Combat.AttackActions.Garlic);
            inventoryManager.AddItem(Combat.ItemsEnum.Apple);
            inventoryManager.AddItem(Combat.ItemsEnum.Ball);
            inventoryManager.AddItem(Combat.ItemsEnum.Photo);
            inventoryManager.AddItem(Combat.ItemsEnum.Key);
        }
        */
    }

    void FindClosestCamera()
    {
        CameraTrigger[] allCam = FindObjectsOfType<CameraTrigger>();
        
        float closestYet = Mathf.Infinity;
        foreach (CameraTrigger ct in allCam)
        {
            float distance = (ct.transform.position - this.transform.position).sqrMagnitude;
            if(distance < closestYet)
            {
                cam = ct;
            }
        }
        if (cam != null)
        {
            camControl.activeCam = cam.gameObject.transform.GetChild(0).gameObject;
            print("findclosestcamera is being called" + cam.gameObject.name);
        }
    }


    public void UpdateSpriteFacing(float angle){
        //up is -z
        //plane is xy

        //This is basically useless rn as I think we'll stick with the unity animator vs setting the frames directly 

        Debug.Log(angle);
        if (Mathf.Abs(angle)<45){//up away from camera

        }else if (Mathf.Abs(angle)>135){//down, facing camera

        }else if (angle>0f){//facing left

        }else{//facing right

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


    // ITEM METHODS:
    void Apple(){
        TriggerDialogue(new string[]{"You're not really that hungry right now","Maybe a ghost will want this?"});
    }
}
