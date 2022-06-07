using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Shotgun : MonoBehaviour
{
    // Components and Gameobjects
    private WeaponManager weaponManager;
    private AudioSource audioSource;
    private ParticleSystem shootEffect;

    // Logic variables
    private RaycastHit hitInfo;
    private float timeTillNextShot = 0f;
    private LayerMask layer;

    // Gun variables
    [Tooltip("Fire rate in seconds of shotgun weapon.")]
    [SerializeField] private float timeBetweenShots = 1f; 

    void Start()
    {

        shootEffect = GameObject.Find("ShotgunShootEffect").GetComponent<ParticleSystem>();
        weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
        audioSource = GetComponent<AudioSource>();

        layer = LayerMask.GetMask("IgnorePlayerLayer");
        
    }
    void Update()
    {

        // Shooting weapon logic
       if(weaponManager.shotgunAmmo>0){

            timeTillNextShot -= Time.deltaTime;

            if (Input.GetButton("Fire1") && timeTillNextShot<=0)
            {
                timeTillNextShot = timeBetweenShots;
                Shoot();

            }
        }
        
    }

    // This function is used to shoot the shotgun
    void Shoot(){
        
        weaponManager.loseAmmo(10);
        shootEffect.Play();
        audioSource.Play();


        int shotNum = 10;

        // Firing multiple shots with random offsets creating a burst effect
        while(shotNum>0){
            shotNum--;

            float randomXOffset = Random.Range(-0.5f,0.5f);
            float randomYOffset = Random.Range(-0.5f,0.5f);

            Vector3 rayDirection = new Vector3(Camera.main.transform.forward.x+randomXOffset, Camera.main.transform.forward.y + randomYOffset, Camera.main.transform.forward.z);
            if (Physics.Raycast(transform.position,rayDirection, out hitInfo,50f,~layer))
            {

                if (hitInfo.collider.gameObject.tag == "Enemy")
                {
                    hitInfo.collider.gameObject.GetComponent<Enemy>().takeDamage(15f);
                }
            }

        }
    }
}
