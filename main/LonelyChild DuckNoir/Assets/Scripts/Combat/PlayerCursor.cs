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
    public float xBound = 1f;
    public float yBound = 1f;
    public float xNegBound = -1f;
    public float yNegBound = -1f;

    public float lerpMult = .1f;

    void Start()
    {
        //camera.main is amateur, but there's no reason for cameras to change in battle, so it's probably fine.
        cam = Camera.main;
        attackLogic=transform.parent.GetComponent<AttackLogic>();
        audioSource = attackLogic.bb.Damage;
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
        gameObject.transform.position = lerpPos;
    }
    public void Damage(int damage)
    {
        attackLogic.Damage(damage);
        audioSource.Play();
    }
        void OnTriggerEnter2D(Collider2D col)
    {
        
        if(col.gameObject.tag == "Projectile")
        {
           // print("THIS WAS CALLED");
           // PlayerCursor playerCursor = col.GetComponent<PlayerCursor>();
            Damage(col.gameObject.GetComponent<ProjectileOne>().projectile.damage);
            Destroy(col.gameObject);
        }
        
    }
}
