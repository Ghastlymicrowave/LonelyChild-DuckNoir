using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class ProjectileOne : MonoBehaviour
{
    public enum ProjectileType { Regular, SineX, SineY, ReverseSineX };
    public ProjectileType projectileType;

    public Projectile projectile;
    Transform theTrans;
    float globalX;
    float globalY;

    void Start()
    {
        theTrans = this.gameObject.transform;
        globalX = theTrans.position.x;
        globalY = theTrans.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;
        switch (projectileType)
        {
            case ProjectileType.Regular:
                y = theTrans.position.y - (projectile.speed * Time.deltaTime);
                theTrans.position = new Vector3(theTrans.position.x, y, theTrans.position.z);
                break;
            case ProjectileType.SineX:
                y = theTrans.position.y - (projectile.speed * Time.deltaTime);
                x = globalX + Mathf.Sin(Time.time * projectile.secondarySpeed) * projectile.secondaryValue;
                theTrans.position = new Vector3(x, y, theTrans.position.z);
                break;
            case ProjectileType.ReverseSineX:
                y = theTrans.position.y - (projectile.speed * Time.deltaTime);
                x = globalX - Mathf.Sin(Time.time * projectile.secondarySpeed) * projectile.secondaryValue;
                theTrans.position = new Vector3(x, y, theTrans.position.z);
                break;
            case ProjectileType.SineY:
                y = globalY + Mathf.Sin(Time.time * projectile.speed) * projectile.secondaryValue;
                theTrans.position = new Vector3(theTrans.position.x, y, theTrans.position.z);
                break;
            default:
                print("You messed up the projectileone");
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if(col.gameObject.tag == "Player")
        {
            print("THIS WAS CALLED");
            PlayerCursor playerCursor = col.GetComponent<PlayerCursor>();
            playerCursor.Damage(projectile.damage);
            Destroy(this.gameObject);
        }
        */
    }
}
