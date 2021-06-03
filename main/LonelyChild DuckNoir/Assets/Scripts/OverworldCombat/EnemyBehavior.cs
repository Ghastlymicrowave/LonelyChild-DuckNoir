using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{
    GameSceneManager gameSceneManager;
    static CameraControl camControl;
    public Transform sprite;
    public bool isMoving = true;
    public bool chasePlayer = true;
    bool patrol = true;
    public float speed = 2f;
    InventoryManager inventoryManager;
    GameObject player;
    public int enemyID = 1;
    TextManager tm;
    public string ToLoad = "combatTest";
    public float constantZ;
    public Transform[] patrolSpots;
    public int startingPatrol = 0;
    private void Start()
    {
        constantZ = transform.position.z;
        if (camControl == null)
        {
            camControl = GameObject.Find("CameraControl").GetComponent<CameraControl>();
        }
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        gameSceneManager = tm.gameObject.GetComponent<GameSceneManager>();
    }
    private void Update()
    {
        sprite.LookAt(camControl.activeCam.transform.position, Vector3.back);
        if (patrol && isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolSpots[startingPatrol].position, (speed / 3) * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, constantZ);
            if (Vector3.Distance(patrolSpots[startingPatrol].position, transform.position) < .5f)
            {
                startingPatrol = Random.Range(0, patrolSpots.Length);
            }
        }
        else if (chasePlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, constantZ);
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            patrol = false;
            player = col.gameObject;

        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inventoryManager.playerPosOnStart = collision.gameObject.transform.position;
            inventoryManager.enemyID = enemyID;
            gameSceneManager.EnterCombat();
        }
    }
}
