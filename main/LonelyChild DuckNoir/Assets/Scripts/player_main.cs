using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_main : MonoBehaviour
{
    [Range(.01f,0.5f)]public float maxSpd = 2f;
    [Range(.001f,0.1f)]public float initalSpd = .1f;
    [Range(0.1f,5f)]public float secToMax = 2f;//seconds it takes to reach max spd from itial speed
    [Range(0f,1f)]public float Kfriction =.2f;//in percent of speed
    [Range(0f,0.1f)]public float Sfriction = .5f;//in units/s


    float currentSpd = 0;
    private float spdAccel {get {return (maxSpd-initalSpd / secToMax);}}
    Vector2 facing = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
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
                currentSpd = Mathf.Min(spdAccel+currentSpd,maxSpd);
                facing = Vector2.Lerp(facing,new Vector2(hinput,vinput).normalized,0.6f).normalized;//rotate angle with lerp
            }
        }else{//not actively moving, apply friction
            currentSpd = Mathf.Max(0f,currentSpd-currentSpd*Kfriction);
            if (currentSpd<Sfriction){
                currentSpd=0;
            }
        }

        if (currentSpd!=0){
            transform.position+= new Vector3(facing.x,0f,facing.y)*currentSpd;
        }
    }
}
