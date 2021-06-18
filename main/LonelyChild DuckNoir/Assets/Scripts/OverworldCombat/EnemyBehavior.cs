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
    public string ToLoad = "combatScene";
     player_main pm = null;
    int currentPatrol = 0;
    int patrolChunk = 0;
    bool patrolBackwards = false; 
    [SerializeField] Transform positionContainer;
    Transform[][] positionChunks;
    [SerializeField] float defaultRespawnTime = 10f;
    float currentRespawnTime = 0f;
    bool spawned = true;
    [SerializeField] float unspawnedZ;
    float spawnedZ;
    GameObject visualGhost;
    public bool stillInScene = true;
    EnemyClass thisEnemy;
    SpriteRenderer drawSprite;
    Sprite[,] spriteList;
    private void Start()
    {
        drawSprite = sprite.gameObject.GetComponent<SpriteRenderer>();
        if (camControl == null)
        {
            camControl = GameObject.Find("CameraControl").GetComponent<CameraControl>();
        }
        if (pm == null)
        {
            player = GameObject.Find("Player");
            pm = player.GetComponent<player_main>();
        }
        spawnedZ = transform.position.z;
        tm = GameObject.Find("PersistentManager").GetComponent<TextManager>();
        inventoryManager = tm.gameObject.GetComponent<InventoryManager>();
        gameSceneManager = tm.gameObject.GetComponent<GameSceneManager>();
        visualGhost = transform.GetChild(0).gameObject;
        positionChunks = new Transform[positionContainer.childCount][];
        for(int i = 0; i < positionContainer.childCount; i++){
            Transform thisChunk = positionContainer.GetChild(i);
            positionChunks[i] = new Transform[thisChunk.childCount];
            for(int a = 0; a < thisChunk.childCount; a++){
                positionChunks[i][a] = thisChunk.GetChild(a);
            }
        }
        thisEnemy = EnemyLibrary.GetRawEnemyFromId(enemyID);
        spriteList = thisEnemy.GetSprites();
    }
    private void Update()
    {
        if (stillInScene){
            if (currentRespawnTime>0){
                currentRespawnTime = Mathf.Max(0f,currentRespawnTime-Time.deltaTime);
            }else if (spawned==false){
                TryRespawn();
            }
        }
        sprite.LookAt(camControl.activeCam.transform.position, Vector3.back);
        SetSpriteFacing();
        if (pm != null)
        {
            if (!pm.canMove||!spawned)
            {
                return;
            }
        }
        
        if (patrol && isMoving)
        {
            Vector3 targetPos = positionChunks[patrolChunk][currentPatrol].position;
            targetPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, (speed / 3) * Time.deltaTime);
            if (Vector3.Distance(positionChunks[patrolChunk][currentPatrol].position, transform.position) < .5f)
            {
                //Debug.Log("enemy "+enemyID.ToString()+" chunk:"+patrolChunk.ToString()+" of "+positionChunks.Length.ToString()+"   patrol:"+currentPatrol.ToString()+" of "+positionChunks[patrolChunk].Length.ToString());
                if (patrolBackwards){
                    if (currentPatrol > 0){
                        currentPatrol--;
                    }else{
                        if (Random.value>0.5f){
                            patrolBackwards = !patrolBackwards;
                        }else{
                            currentPatrol = positionChunks[patrolChunk].Length-1;
                        }
                    }
                }else{
                    if (currentPatrol < positionChunks[patrolChunk].Length-1){
                        currentPatrol++;
                    }else{
                        if (Random.value>0.5f){
                            patrolBackwards = !patrolBackwards;
                        }else{
                            currentPatrol = 0;
                        }
                    }
                }
                //Debug.Log("NEW enemy "+enemyID.ToString()+" chunk:"+patrolChunk.ToString()+" patrol:"+currentPatrol.ToString());
            }
        }
        else if (chasePlayer)
        {
            Vector3 targetPos = player.transform.position;
            targetPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }

    }

    void SetSpriteFacing(){
        Vector3 facing = positionChunks[patrolChunk][currentPatrol].position;
        facing.z = transform.position.z;
        facing-= transform.position;
        Vector3 camFacing = camControl.transform.position;
        camFacing.z = transform.position.z;
        Vector3 playerPos = player.transform.position;
        playerPos.z = transform.position.z;
        camFacing = playerPos - camFacing;
        float upDown = Vector3.Dot(facing,camFacing)*-.5f*2f;//down -1 back 1
        camFacing = camControl.activeCam.transform.right;
        camFacing.z = transform.position.z;
        float rightLeft = Vector3.Dot(facing,camFacing)*-.5f*2f;//right 1 left -1
        //0 front
        //1 back
        //2 right side
        if (Mathf.Abs(upDown)>Mathf.Abs(rightLeft)){//updown priority
            if (upDown>0f){//back
                drawSprite.sprite = spriteList[1,0];
                drawSprite.flipX = false;
            }else{//front
                drawSprite.sprite = spriteList[0,0];
                drawSprite.flipX = false;
            }
        }else{//right left priority
            if (rightLeft>0f){//right
                drawSprite.sprite = spriteList[2,0];
                drawSprite.flipX = false;
            }else{//left
                drawSprite.sprite = spriteList[2,0];
                drawSprite.flipX = true;
            }
        }
    }

    void TryRespawn(){
        patrolChunk = Random.Range(0,positionChunks.Length);
        currentPatrol = Random.Range(0,positionChunks[patrolChunk].Length);
        Vector3 targetPos = positionChunks[patrolChunk][currentPatrol].position;
        targetPos.z = player.transform.position.z;
        if (Vector3.Distance(player.transform.position,targetPos)>10f){
            spawned=true;
            visualGhost.SetActive(true);
            //TODO: add a spawned animation, probably by smoothing the position from a set z value. (so it arrises from the floor)
        }
    }

    public void DespawnGhost(){
        visualGhost.SetActive(false);
        spawned=false;
        currentRespawnTime = defaultRespawnTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player" && spawned)//pathfind
        {
            patrol = false;
            player = col.gameObject;
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player"  && spawned)//trigger combat
        {
            DespawnGhost();
            inventoryManager.enemyID = enemyID;
            gameSceneManager.EnterCombat();
        }
    }
}
