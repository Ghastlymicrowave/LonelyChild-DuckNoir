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
    GameObject toShootAt;
    [SerializeField] float shootingTrackDuration = .75f;
    [SerializeField] float shootingCooldownDuration = .25f;
    [SerializeField] float shootingTrackSpeed = 1f;
    private float trackPercent = 0f;
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

    void Start()
    {
        theTrans = this.gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
        if (projectileType == ProjectileType.TurretOne)
        {
            toShootAt = new GameObject("toShootAt");
            cursor = GameObject.Find("Player Cursor");


            hypothetical = new GameObject();
            hypothetical.transform.position = theTrans.position;
            hypothetical.transform.rotation = theTrans.rotation;
            hypothetical.name = "Hypothetical";
            // hypothetical.transform.SetParent(theTrans);

        }
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
            case ProjectileType.LocalRegular:
                rb.AddRelativeForce(Vector2.left * Time.deltaTime * (projectile.speed * 750f));
                break;
            case ProjectileType.TurretOne:
                /*
                    timer += Time.deltaTime;
                    toShootAt.transform.position = cursor.transform.position;
                    switch (isTracking)
                    {
                        case true:
                            hypothetical.transform.LookAt(toShootAt.transform);
                            trackPercent = shootingTrackDuration / timer;
                            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, hypothetical.transform.rotation, Mathf.Pow(-shootingTrackSpeed, Time.deltaTime));
                            if (timer > shootingTrackDuration)
                            {

                                timer = 0f;
                                // isTracking = false;
                                if (hypothetical.transform.rotation.y > 0f)
                                {
                                    temp = Instantiate(toShoot, hypothetical.transform.position, new Quaternion(0f, 0f, (hypothetical.transform.rotation.x * 4f), hypothetical.transform.rotation.w));

                                }
                                else
                                {
                                    temp = Instantiate(toShoot, hypothetical.transform.position, new Quaternion(0f, 0f, hypothetical.transform.rotation.x, hypothetical.transform.rotation.w));
                                }
                                print(hypothetical.transform.rotation.x);








                                //GameObject temp = Instantiate(toShoot, hypothetical.transform.position, new Quaternion(0f, 0f, hypothetical.transform.rotation.x, hypothetical.transform.rotation.w));
                                //temp.transform.LookAt(toShootAt.transform);
                            }
                            break;
                        default:

                            break;
                    }
    */
                timer += Time.deltaTime;
                toShootAt.transform.position = cursor.transform.position;
                switch (isTracking)
                {
                    case true:
                        hypothetical.transform.LookAt(toShootAt.transform, Vector3.left);
                        trackPercent = shootingTrackDuration / timer;
                      
                      float  zz = Mathf.Lerp(this.transform.rotation.z, hypothetical.transform.rotation.x, shootingTrackSpeed * Time.deltaTime);
                        print(zz);
                        this.transform.rotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, zz, this.transform.rotation.w);
                        if (timer > shootingTrackDuration)
                        {
                            timer = 0f;
                            temp = Instantiate(toShoot, hypothetical.transform.position, new Quaternion(0f, 0f, this.transform.rotation.z, 
                            hypothetical.transform.rotation.w));

                           // print(hypothetical.transform.rotation.x);
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
