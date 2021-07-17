using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class ProjectileOne : MonoBehaviour
{
    public enum ProjectileType {Regular, SineX, SineY, ReverseSineX, TurretOne, LocalRegular, Homing, GoodThenBad,None };
    public bool destroyOnCollision = true;
    [SerializeField] SpriteRenderer spriteForGoodThenBad;
    [SerializeField] BoxCollider2D colForGoodThenBad;
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
    float startTime;
    AttackLogic logic;
    float logicDuration;

    float fullTimer = 0f;
    Color tempp;

    void Start()
    {
        //projectile.speed *= 0.0001f;
        theTrans = this.gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
        if (projectileType == ProjectileType.TurretOne)
        {
            cursor = GameObject.Find("Player Cursor");

            currentDirection = (cursor.transform.position - transform.position).normalized;

        }
        if(projectileType == ProjectileType.GoodThenBad)
        {
            colForGoodThenBad.enabled = false;
        }
        if (projectileType == ProjectileType.Homing){
            cursor = GameObject.Find("Player Cursor");
        }
        if (projectileType == ProjectileType.LocalRegular){

        }
        globalX = theTrans.position.x;
        globalY = theTrans.position.y;
        startTime = Time.time;
        logic = GameObject.FindObjectOfType<AttackLogic>();
        logicDuration = logic.attack.duration - logic.timer;
        UpdateStuff();
    }

    // Update is called once per frame
    void Update()
    {
        fullTimer += Time.deltaTime;
        UpdateStuff();
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
    void UpdateStuff()
    {
          logicDuration -= Time.deltaTime;
        if (logicDuration<=0){
            Destroy(gameObject);
        }
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
                x = globalX - Mathf.Sin(Time.time - startTime) * projectile.secondaryValue;
                theTrans.position = new Vector3(x, y, theTrans.position.z);
                break;
            case ProjectileType.SineY:
                y = globalY + Mathf.Sin(Time.time - startTime) * projectile.secondaryValue;
                theTrans.position = new Vector3(theTrans.position.x, y, theTrans.position.z);
                break;
            case ProjectileType.Homing:
                Vector2 targ = (cursor.transform.position - transform.position).normalized;
                rb.velocity = Vector2.Lerp(rb.velocity.normalized,targ,shootingTrackSpeed * Time.deltaTime).normalized * projectile.speed;
                break;
            case ProjectileType.GoodThenBad:
                if(fullTimer < 1f)
                {
                    tempp = spriteForGoodThenBad.color;
                    tempp.a = fullTimer;
                    spriteForGoodThenBad.color = tempp;
                }
                else
                {
                    tempp = spriteForGoodThenBad.color;
                    tempp.a = 1f;
                    spriteForGoodThenBad.color = tempp;
                    colForGoodThenBad.enabled = true;
                }
                break;
            case ProjectileType.LocalRegular:
                rb.velocity = -transform.right * projectile.speed;
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
                            temp = Instantiate(toShoot,transform.position, Quaternion.LookRotation(dir,Vector3.forward));//the bullet
                            

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
                print("You reached default in the projectileone");
                break;
        }
    }
}
