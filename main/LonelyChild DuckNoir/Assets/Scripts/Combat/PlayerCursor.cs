using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    private AttackLogic attackLogic;
    public AudioSource audioSource;
    private Camera cam;
    Vector3 realPos;
    Vector3 mousePos;
    Vector3 lerpPos;
    public Rigidbody2D rb;
    public float xBound = 1f;
    public float yBound = 1f;
    public float xNegBound = -1f;
    public float yNegBound = -1f;

    //new//

    public float invincibilityDuration = 1f;
    float invincibilityTimer = 0f;
    bool isFlashing = false;
    bool isNormColor = true;
    public SpriteRenderer cursor;
    public Color cursorNorm;
    public Color cursorFlash;

    //new//

    public float lerpMult = .1f;

    void Start()
    {
        //camera.main is amateur, but there's no reason for cameras to change in battle, so it's probably fine.
        cam = Camera.main;
        attackLogic = transform.parent.GetComponent<AttackLogic>();
        audioSource = attackLogic.bb.Damage;
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (cursor == null)
        {
            cursor = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        {
            //   Debug.Log("X " + mousePos.x + " Y " + mousePos.y);
            // Debug.Log("ERER" + cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane)));
            realPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1f));
            if (realPos.y > yBound)
            {
                realPos = new Vector3(realPos.x, yBound, realPos.z);
            }
            else if (realPos.y < yNegBound)
            {
                realPos = new Vector3(realPos.x, yNegBound, realPos.z);
            }
            if (realPos.x > xBound)
            {
                realPos = new Vector3(xBound, realPos.y, realPos.z);
            }
            else if (realPos.x < xNegBound)
            {
                realPos = new Vector3(xNegBound, realPos.y, realPos.z);
            }
        }
        lerpPos = Vector3.Lerp(gameObject.transform.position, realPos, lerpMult);
        // realPos = new Vector3(realPos.x, realPos.y, gameObject.transform.position.z);

        ///// gameObject.transform.position = lerpPos;
        rb.MovePosition(lerpPos);




        //flashing logic
        if (isFlashing)
        {
            invincibilityTimer += Time.deltaTime;
            isNormColor = !isNormColor;
            switch (isNormColor)
            {
                case true:
                    cursor.color = cursorNorm;
                    break;
                default:
                    cursor.color = cursorFlash;
                    break;
            }
            if(invincibilityTimer > invincibilityDuration)
            {
                isFlashing = false;
                cursor.color = cursorNorm;
                invincibilityTimer = 0f;
            }
        }

    }
    public void Damage(int damage)
    {
        if (isFlashing == false)
        {
            attackLogic.Damage(damage);
            audioSource.Play();
            isFlashing = true;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Projectile")
        {
            // print("THIS WAS CALLED");
            // PlayerCursor playerCursor = col.GetComponent<PlayerCursor>();
            if (isFlashing == false)
            {
                Destroy(col.gameObject);
            }
            Damage(col.gameObject.GetComponent<ProjectileOne>().projectile.damage);
            
        }

    }
}
