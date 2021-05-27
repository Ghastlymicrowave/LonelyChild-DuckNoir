using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_main : MonoBehaviour
{
    [Range(0.001f,0.5f)]public float maxSpd = 2f;
    [Range(0.001f,0.5f)]public float initalSpd = .1f;
    [Range(0.1f,5f)]public float secToMax = 2f;//seconds it takes to reach max spd from itial speed
    [Range(0f,0.5f)]public float Kfriction =.2f;//in percent of speed
    [Range(0f,0.5f)]public float Sfriction = .5f;//in units/s

    public float rotatationSpd = 0.1f;

    float hinput = 0f;
    float vinput = 0f;
    bool isMoving = false;
    float currentSpd = 0;
    public float spdAccel;
    Vector2 facing = Vector2.zero;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hinput = Input.GetAxis("Horizontal");
        vinput = Input.GetAxis("Vertical");
        isMoving = (hinput!=0f || vinput!=0f);
        if (isMoving){
            if (currentSpd<initalSpd){//moving from standstill
                currentSpd = initalSpd;
                facing = new Vector2(hinput,vinput).normalized;
            }else{
                currentSpd = Mathf.Min(spdAccel+currentSpd,maxSpd);
                if (Vector2.Angle(new Vector2(hinput,vinput),facing)>160f){
                    facing = Vector2.Lerp(facing,new Vector2(hinput,vinput).normalized,0.51f).normalized;
                }else{
                    facing = Vector2.Lerp(facing,new Vector2(hinput,vinput).normalized,rotatationSpd).normalized;//rotate angle with lerp
                }
                
            }
        }else{//not actively moving, apply friction
            currentSpd = Mathf.Max(0f,currentSpd-currentSpd*Kfriction);
            if (currentSpd<Sfriction){
                currentSpd=0;
            }
        }

        if (currentSpd!=0){
            //rb.MovePosition((Vector2)transform.position + new Vector2(facing.x,facing.y)*currentSpd * Time.deltaTime*100);
            rb.MovePosition((Vector2)transform.position + new Vector2(facing.x,facing.y)*currentSpd);
        }else{
            rb.velocity = Vector2.zero;
        }
    }
}
