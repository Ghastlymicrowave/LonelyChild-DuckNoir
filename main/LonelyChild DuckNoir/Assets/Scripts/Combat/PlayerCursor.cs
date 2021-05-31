using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public battleBehavior bb;
    private Camera cam;
    Vector3 realPos;
    Vector3 mousePos;
    Vector3 lerpPos;

    public float lerpMult = .1f;

    void Start()
    {
        //camera.main is amateur, but there's no reason for cameras to change in battle, so it's probably fine.
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
            {
             //   Debug.Log("X " + mousePos.x + " Y " + mousePos.y);
           // Debug.Log("ERER" + cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane)));
            realPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        }
        lerpPos = Vector3.Lerp(gameObject.transform.position, realPos, lerpMult);
        // realPos = new Vector3(realPos.x, realPos.y, gameObject.transform.position.z);
        gameObject.transform.position = lerpPos;
    }
    public void Damage(int damage)
    {
        //call damage function in battle behavior
    }
}
