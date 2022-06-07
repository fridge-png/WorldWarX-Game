using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Default : MonoBehaviour
{

    // Components and Gameobjects
    private WeaponManager weaponManager;
    private AudioSource audioSource;
    private ParticleSystem shootEffect;

    // Logic variables
    private RaycastHit hitInfo;
    private float timeTillNextShot = 0.1f;
    private LayerMask layer;

    // Gun variables
    [Tooltip("Fire rate in seconds of default weapon.")]
    [SerializeField] private float timeBetweenShots = 0.1f; 
    

    void Start()
    {

        shootEffect = GameObject.Find("ShootEffect").GetComponent<ParticleSystem>();
        weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
        audioSource = GetComponent<AudioSource>();

        layer = LayerMask.GetMask("IgnorePlayerLayer");
        
    }

    void Update()
    {

        // Shooting weapon logic
        if(weaponManager.defaultAmmo >0){

            timeTillNextShot -= Time.deltaTime;

            if (Input.GetButton("Fire1") && timeTillNextShot<=0)
            {
                Shoot();
                timeTillNextShot = timeBetweenShots;
            }
        }
        
    }


    // This function is used to shoot the default weapon
    void Shoot(){
        
        weaponManager.loseAmmo(1);
        shootEffect.Play();
        audioSource.Play();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo,200f,~layer))
        {
            if (hitInfo.collider.gameObject.tag == "Enemy")
            {
                hitInfo.collider.gameObject.GetComponent<Enemy>().takeDamage(5f);
            }
        }
    }

}
