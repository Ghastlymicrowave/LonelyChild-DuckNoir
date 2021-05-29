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
    private CameraControl camControl;
    void Start()
    {
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
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

    void Update()
    {
        if (!canMove)
        {
            interactableTarget.isBusy = true;
            return;
        }
        if (interactableTarget != null)
        {
            interactableTarget.isBusy = false;
        }
        hinput = Input.GetAxis("Horizontal");
        vinput = Input.GetAxis("Vertical");
        isMoving = (hinput != 0f || vinput != 0f);
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

            rb.MovePosition((Vector2)transform.position + standardFacing);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        interactHitbox.transform.localPosition = facing * interactHitboxOffset;

        if (interactableTarget != null)
        {
            if (interactableTarget.isReady||Input.GetButtonDown("Submit"))//if triggered from mouse click or interact button
            {
                interactableTarget.Trigger();
            }
        }
        UpdateSpriteFacing();
    }


    public void UpdateSpriteFacing(){
        //up is -z
        //plane is xy
    }
    public void InteractableEntered(Interactable thisInteractable)
    {
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

    public void TriggerDialogue(int textID){
        string[] toScroll = tm.GetTextByID(textID);
        textScroller.ScrollText(toScroll, this);
    }
    
}
