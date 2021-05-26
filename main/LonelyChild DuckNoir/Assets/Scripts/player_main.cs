using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_main : MonoBehaviour
{
    [Range(.1f,10f)]public float maxSpd = 2f;
    [Range(.1f,10f)]public float initalSpd = .1f;
    [Range(0.1f,5f)]public float secToMax = 2f;//seconds it takes to reach max spd from itial speed
    [Range(0f,1f)]public float Kfriction =.2f;//in percent of speed
    [Range(0f,5f)]public float Sfriction = .5f;//in units/s


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
        float hinput = Input.GetAxis("Horizontal");
        float vinput = Input.GetAxis("Vertical");
        bool isMoving = (hinput!=0f || vinput!=0f);
        
        if (isMoving){
            if (currentSpd<initalSpd){//moving from standstill
                currentSpd = initalSpd;
                facing = new Vector2(hinput,vinput).normalized;
            }else{
                currentSpd = Mathf.Min(spdAccel*Time.deltaTime+currentSpd,maxSpd);
                if (Vector2.Angle(new Vector2(hinput,vinput),facing)>160f){
                    facing = Vector2.Lerp(facing,new Vector2(hinput,vinput).normalized,0.51f).normalized;
                }else{
                    facing = Vector2.Lerp(facing,new Vector2(hinput,vinput).normalized,0.05f).normalized;//rotate angle with lerp
                }
                
            }
        }else{//not actively moving, apply friction
            currentSpd = Mathf.Max(0f,currentSpd-currentSpd*Kfriction);
            if (currentSpd<Sfriction){
                currentSpd=0;
            }
        }

        if (currentSpd!=0){
            rb.velocity = new Vector2(facing.x,facing.y)*currentSpd;
        }else{
            rb.velocity = Vector2.zero;
        }
    }
}
