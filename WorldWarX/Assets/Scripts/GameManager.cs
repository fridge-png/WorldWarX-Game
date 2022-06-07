using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; 

 

    // Components and Gameobjects
    [Tooltip("Enemy prefab.")]
    [SerializeField] private GameObject enemy;

    [Tooltip("Object positions where enemies spawn.")]
    [SerializeField] GameObject [] spawnPoints;

    private Animator shotAlarmAnimator;

    // UI Elements
    private Slider healthBarUI;
    private TextMeshProUGUI waveText;
    private TextMeshProUGUI scoreText;
    private Animator waveTextAnimator;

    // Logic variables
    public bool shot = false;
    private float timeLeftToSpawn=0f;
    private int currentWaveIndex = 0;
    private int enemiesLeftToSpawn;
    private int enemiesLeft;

    // Game Variables
    [Tooltip("Number of enemies in each wave from first to last.")]
    [SerializeField] private int [] waveEnemiesNum = {3,5,7};

    [Tooltip("Health points of player.")]
    [SerializeField] private float health = 100f;
    private float score = 0;
    
    void Awake(){

        // Singleton pattern
        if(instance != null){
            Destroy(this);
        }
        else{
            instance = this;
        }

    }

    void Start(){
        
        // Initializing components
        healthBarUI = GameObject.Find("HealthBar").GetComponent<Slider>();
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        waveText = GameObject.Find("WaveText").GetComponent<TextMeshProUGUI>();
        waveTextAnimator = GameObject.Find("WaveText").GetComponent<Animator>();
        shotAlarmAnimator = GameObject.Find("ShotAlarm").GetComponent<Animator>();

        enemiesLeftToSpawn = waveEnemiesNum[currentWaveIndex];
        enemiesLeft = waveEnemiesNum[currentWaveIndex];

        // Setting up UI elements
        waveText.text = "Wave " + (currentWaveIndex+1);
        waveTextAnimator.Play("WaveTextEntry");
        scoreText.text = "Score: " + score;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        timeLeftToSpawn-= Time.deltaTime;

        // Spawning enemies
        if(timeLeftToSpawn<=0 && enemiesLeftToSpawn>0){
            Instantiate(enemy, spawnPoints[Random.Range(0,spawnPoints.Length)].transform.position,Quaternion.identity);
            timeLeftToSpawn=3f;
            enemiesLeftToSpawn--;
        }

        // Starting next round if all enemies are dead
        if(enemiesLeft<=0){
            startNewWave();
        }


        // Take damage when player is shot
        if(shot){
            takeDamage();
        }
        
    }

    // This function is fired when the player takes damage
    private void takeDamage(){
        shotAlarmAnimator.Play("ShotAlarm");
        // Less health is deducted when player is at lower health
        if(health>30)health -=10f;
        else health -=5f;
        healthBarUI.value = health;
        shot = false;

        // Load death scene if player dies
        if(health<=0){
            SceneManager.LoadScene(2);
        }
    }

    // This function is fired when an enemy dies
    public void subtractEnemy(){
        score++;
        scoreText.text = "Score: " + score;
        enemiesLeft--;
    }

    // This function is fired when all enemies are dead and a new wave is starting
    private void startNewWave(){

        // If all waves are done, the win scene is loaded
        if(currentWaveIndex+1 >= waveEnemiesNum.Length){
            SceneManager.LoadScene(3);

        }
        else{
            waveTextAnimator.Play("WaveTextEntry",-1, 0f);

            currentWaveIndex++;
            enemiesLeft=waveEnemiesNum[currentWaveIndex];
            enemiesLeftToSpawn=waveEnemiesNum[currentWaveIndex];
            waveText.text = "Wave " + (currentWaveIndex+1);


        }

    }
}
