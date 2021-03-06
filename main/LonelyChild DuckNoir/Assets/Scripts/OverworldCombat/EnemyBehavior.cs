using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{
    GameSceneManager gameSceneManager;
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
    ThirdPersonPlayer pm = null;
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
    float targetSpd = 1f;
    float currentSpd =1f;
    Sprite[,] spriteList;
    [SerializeField] LayerMask raycastMask;
    private void Start()
    {
        drawSprite = sprite.gameObject.GetComponent<SpriteRenderer>();
        if (pm == null)
        {
            player = GameObject.Find("Player");
            pm = player.GetComponent<ThirdPersonPlayer>();
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
                Debug.Log("still in scene, trying to respawn");
                currentRespawnTime = Mathf.Max(0f,currentRespawnTime-Time.deltaTime);
                if (currentRespawnTime<=0){
                    if (spawned==false){
                        TryRespawn();
                    }
                }
            }
        }
        if (Camera.main!=null){
            sprite.LookAt(Camera.main.transform.position, Vector3.back);
        }
        
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
            if (Random.Range(0f,1f)<.2f){
                targetSpd = Random.Range(.6f,1f);
            }
            currentSpd = Mathf.Lerp(currentSpd,targetSpd,0.01f);
            float maxDist = Vector2.Distance(positionChunks[patrolChunk][currentPatrol].position, transform.position)/400f * currentSpd * speed/3f;
            maxDist = Mathf.Clamp(maxDist,speed/6f * Time.deltaTime,(speed / 3f) * Time.deltaTime * currentSpd);
            transform.position = Vector3.MoveTowards(transform.position, targetPos,maxDist);
            if (Vector2.Distance(positionChunks[patrolChunk][currentPatrol].position, transform.position) < .5f)
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
        Vector3 facing = positionChunks[patrolChunk][currentPatrol].position;//enemy facing direction
        if (!patrol){
         facing = player.transform.position;   
        }
        facing.z = 0f;
        facing-= new Vector3(transform.position.x,transform.position.y,0f);
        facing.Normalize();
        Vector3 camFacing = Camera.main.transform.forward;
        camFacing.z = 0f;
        camFacing.Normalize();
        //Debug.Log("facing:"+facing.ToString()+"camFacing: "+camFacing.ToString());
        float upDown = Vector3.Dot(facing,camFacing)*-.5f*2f;//down -1 back 1
        camFacing = Camera.main.transform.right;
        camFacing.z = 0f;
        float rightLeft = Vector3.Dot(facing,camFacing)*-.5f*2f;//right 1 left -1
        //0 front
        //1 back
        //2 right side
        
        if (Mathf.Abs(upDown)>Mathf.Abs(rightLeft)){//updown priority
            if (upDown>0f){//front
                drawSprite.sprite = spriteList[0,0];
                drawSprite.flipX = false;
            }else{//back
                drawSprite.sprite = spriteList[1,0];
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
            patrol = true;
            //TODO: add a spawned animation, probably by smoothing the position from a set z value. (so it arrises from the floor)
        }
    }

    public void DespawnGhost(){
        visualGhost.SetActive(false);
        spawned=false;
        currentRespawnTime = defaultRespawnTime;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player"){
            Vector3 thisPos = transform.position;
            Vector3 thatPos = player.transform.position;
            thatPos.z = thisPos.z;
            float dist = Vector3.Distance(thisPos,thatPos);
            RaycastHit hit;
            Physics.Raycast(transform.position,player.transform.position-transform.position, out hit,dist,raycastMask);
            Debug.Log(col.gameObject.tag);
            Debug.Log(hit.collider);
            Debug.DrawRay(transform.position,(player.transform.position-transform.position).normalized*dist,Color.green,1f);
            if (spawned && (hit.collider==null || hit.collider.tag == "PlayerInteract"))//pathfind
            {
                patrol = false;
                player = col.gameObject;
                Debug.Log("TRUE");
            }
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
