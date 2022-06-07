using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    //Components and Gameobjects
    private GameManager gameManager;
    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private Slider slider;
    private GameObject parentObject;
    private ParticleSystem laserBeam;
    private EnemyLaserLookAt laserBeamScript;
    private AudioSource laserBeamAudio;

    // Logic variables
    private RaycastHit hitInfo;
    private RaycastHit FOVInfo;
    float timeLeftToShoot = 0f;
    private bool isAlive = true;
    private bool isFacing = false;
    private LayerMask layer;

    // Enemy characteristics variables
    [Tooltip("Health points of enemy.")]
    [SerializeField] private float health =200f;

    [Tooltip("Distance from player to trigger enemy shooting.")]
    [SerializeField] private float triggerArea = 30f;

    [Tooltip("Enemy agent movement speed.")]
    [SerializeField] private float speed = 10f;

    [Tooltip("Fire rate in seconds of enemy laser.")]
    [SerializeField] private float timeBetweenShots = 3f;

    [Tooltip("Time in seconds between aiming and shooting ( Miss rate ).")]
    [SerializeField] private float timeBetweenAimAndShoot = 1f;
 

    
    void Start()
    {

        // Initializing Components and Gameobjects
        parentObject = transform.parent.gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        agent = parentObject.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        laserBeam = GetComponentInChildren<ParticleSystem>();
        laserBeamScript =GetComponentInChildren<EnemyLaserLookAt>();
        laserBeamAudio = GetComponent<AudioSource>();
        slider = GetComponentInChildren<Slider>();

        // Initializing characteristic variables
        slider.maxValue = health;
        slider.value = health;
        agent.speed = speed;
        layer = LayerMask.GetMask("IgnoreEnemyLayer");

        
        // Starting coroutine for checking if the player is in the mecha's feild of view 
        StartCoroutine("checkFOV");
        

        
    }

    void Update()
    {
        if(isAlive){

            // Making the enemy face the player
            parentObject.transform.LookAt(new Vector3(player.position.x,parentObject.transform.position.y,player.position.z));
            
            // Calculating the distance left to start shooting 
            float distanceToPlayer = Vector3.Distance(transform.position, player.position); 

            if(distanceToPlayer>triggerArea || !isFacing ){
                agent.isStopped = false;
                agent.destination = player.position;
                animator.Play("WalkAction");
                
            }
            else{
                animator.Play("EnemyShoot");
                agent.isStopped = true;
                StartCoroutine("Shoot");

            }
        }
    }

    // This coroutine is used to cast a ray and check if the player is in the enemy's field of view
    IEnumerator checkFOV(){

        while(isAlive){

            Ray ray = new Ray(new Vector3(transform.position.x,transform.position.y+1f,transform.position.z), player.position- transform.position);
            if(Physics.Raycast(ray, out FOVInfo,100f,~layer)){

                if(FOVInfo.collider.gameObject.tag == "Player"){
                    isFacing = true;
                }
                else{
                    isFacing = false;
                }

            }

            yield return new WaitForSeconds(1f);

        }

            

    }

    // This function is fired when the enemy takes damage from the player
    public void takeDamage(float damage){
        health-=damage;
        slider.value=health;

        if(health<=0){
            Die();
        }

    }

    // This function is fired when the players health is <= 0
    private void Die(){
        isAlive=false;
        animator.Play("DieAction");
        GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject,2f);
        gameManager.subtractEnemy();
    }

    // This coroutine controls how fast and how often the enemy shoots
    IEnumerator Shoot(){

        if(timeLeftToShoot>0){
            timeLeftToShoot -= Time.deltaTime;
            yield return null;
        }
        else{
            timeLeftToShoot = timeBetweenShots;

            // A ray is created and locked to then be cased after timeBetweenAimAndShoot seconds
            // This allows the enemy to miss the player when they are moving alot
            Ray ray = new Ray(new Vector3(transform.position.x,transform.position.y+1f,transform.position.z), player.position- transform.position);
            Vector3 lockedPos = player.position;
            laserBeamScript.lookAtPos = lockedPos;
            

            yield return new WaitForSeconds(timeBetweenAimAndShoot);
            
            if(Physics.Raycast(ray, out hitInfo,100f,~layer)){
                laserBeam.Play();
                laserBeamAudio.Play();
                
                if(hitInfo.collider.gameObject.tag == "Player"){
                    gameManager.shot = true;
                }
                
                
            }
            
        }
    }
}
