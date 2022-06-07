using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Laser : MonoBehaviour
{
    // Components and Gameobjects
    private WeaponManager weaponManager;
    private AudioSource audioSource;
    private ParticleSystem shootEffect;


    // Logic Variables
    private RaycastHit hitInfo;
    private LayerMask layer;

    private float timeTillNextShot;
    void Start()
    {


        shootEffect = GameObject.Find("LaserShootEffect").GetComponent<ParticleSystem>();
        weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
        audioSource = GetComponent<AudioSource>();

        layer = LayerMask.GetMask("IgnorePlayerLayer");
        
    }
 void Update()
    {
        if(weaponManager.laserAmmo >0){

            timeTillNextShot -= Time.deltaTime;

            if (Input.GetButton("Fire1") && timeTillNextShot<=0)
            {
                Shoot();
                timeTillNextShot = 1f;
            }
        }
        
    }


    void Shoot(){
        
        weaponManager.loseAmmo(5);
        shootEffect.Play();
        audioSource.Play();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo,200f,~layer))
        {


            if (hitInfo.collider.gameObject.tag == "Enemy")
            {
                hitInfo.collider.gameObject.GetComponent<Enemy>().takeDamage(30f);
            }
        }
    }

}
