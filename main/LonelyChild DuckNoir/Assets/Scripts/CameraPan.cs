using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public enum CameraPanMovement { Xaxis, Yaxis };
    //public Transform pointHigher;
    //public Transform pointLower;
    public Transform player;
    [SerializeField] CameraPanMovement cpm;
    public float speed = 0.2f;
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    public Vector3 GetTargetPos(){
        switch (cpm)
        {
            case CameraPanMovement.Yaxis:
                return new Vector3(this.transform.position.x, player.position.y, this.transform.position.z);
            default:

                return new Vector3(player.position.x, this.transform.position.y, this.transform.position.z);
        }
    }
//what used to be in the update function.
            /*
        switch (cpm)
        {
            case CameraPanMovement.Yaxis:
                if (this.transform.position.y > player.position.y)
                {

                    if (pointHigher.position.y < this.transform.position.y)
                    {
                        this.transform.position = new Vector3(this.transform.position.x, pointHigher.position.y, this.transform.position.z);
                    }
                    else
                    {
                        this.transform.position = new Vector3(this.transform.position.x, player.position.y, this.transform.position.z);
                    }
                }
                else if (this.transform.position.y < player.position.y)
                {

                    if (pointLower.position.y > this.transform.position.y)
                    {
                        this.transform.position = new Vector3(this.transform.position.x, pointLower.position.y, this.transform.position.z);
                    }
                    else
                    {
                        this.transform.position = new Vector3(this.transform.position.x, player.position.y, this.transform.position.z);
                    }
                }

                break;
            default:
                if (this.transform.position.x > player.position.x)
                {

                    if (pointHigher.position.x < this.transform.position.x)
                    {
                        this.transform.position = new Vector3(pointHigher.position.x, this.transform.position.y, this.transform.position.z);
                    }
                    else
                    {
                        this.transform.position = new Vector3(player.position.x, this.transform.position.y, this.transform.position.z);
                    }
                }
                else if (this.transform.position.x < player.position.x)
                {

                    if (pointLower.position.x > this.transform.position.x)
                    {
                        this.transform.position = new Vector3(pointLower.position.x, this.transform.position.y, this.transform.position.z);
                    }
                    else
                    {
                        this.transform.position = new Vector3(player.position.x, this.transform.position.y, this.transform.position.z);
                    }
                }
                break;
        }
        */
}
