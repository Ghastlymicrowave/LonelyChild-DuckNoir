using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class ProjectileOne : MonoBehaviour
{
    public enum ProjectileType { Regular, SineX, SineY, ReverseSineX, TurretOne, LocalRegular };
    public bool destroyOnCollision = true;
    public ProjectileType projectileType;
    //These six are only needed for turrets.
    [SerializeField] GameObject toShoot;
    GameObject cursor;
    [SerializeField] float shootingTrackDuration = .75f;
    [SerializeField] float shootingCooldownDuration = .25f;
    [SerializeField] float shootingTrackSpeed = 1f;
    Rigidbody2D rb;
    private GameObject hypothetical;
    //Not every projectile needs a projectile sound,
    AudioSource audioIfNeeded;
    private bool isTracking = true;
    private float timer = 0f;
    public Projectile projectile;
    Transform theTrans;
    float globalX;
    float globalY;
    GameObject temp = null;
    Vector3 currentDirection = Vector3.zero;

    void Start()
    {
        theTrans = this.gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
        if (projectileType == ProjectileType.TurretOne)
        {
            cursor = GameObject.Find("Player Cursor");


            hypothetical = new GameObject();
            hypothetical.transform.position = theTrans.position;
            hypothetical.transform.rotation = theTrans.rotation;
            hypothetical.name = "Hypothetical";
            currentDirection = (cursor.transform.position - transform.position).normalized;

        }
        globalX = theTrans.position.x;
        globalY = theTrans.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0f;
        float y = 0f;
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
            case ProjectileType.LocalRegular:
                rb.AddRelativeForce(Vector2.left * Time.deltaTime * (projectile.speed * 750f));
                break;
            case ProjectileType.TurretOne:
                timer += Time.deltaTime;
                switch (isTracking)
                {
                    case true:
                        Vector2 targetDirection = (cursor.transform.position - transform.position).normalized;
                        currentDirection = Vector3.Lerp(currentDirection,targetDirection,shootingTrackSpeed * Time.deltaTime).normalized;
                        Debug.Log(currentDirection);
                        transform.rotation = Quaternion.LookRotation(currentDirection,Vector3.forward);
                        if (timer > shootingTrackDuration)
                        {
                            timer = 0f;
                            Vector3 dir = Quaternion.Euler(new Vector3(0f,0f,90f)) * currentDirection;
                            temp = Instantiate(toShoot, hypothetical.transform.position, Quaternion.LookRotation(dir,Vector3.forward));//the bullet
                            

                            isTracking = false;
                        }
                        break;
                    default:
                        timer += Time.deltaTime;
                        if(timer > shootingCooldownDuration)
                        {
                            timer = 0f;
                            isTracking = true;
                        }

                        break;
                }
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
