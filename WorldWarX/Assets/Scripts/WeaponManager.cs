using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{


    // Components and Gameobjects
    private GameObject defaultgun;
    private GameObject laser;
    private GameObject shotgun;

    private Shoot_Default defaultScript;
    private Shoot_Laser laserScript;
    private Shoot_Shotgun shotgunScript;

    private GameObject gunPivot;

    //UI elements
    private Image defaultSprite;
    private Image laserSprite;
    private Image shotgunSprite;
    private TextMeshProUGUI defaultAmmoText;
    private TextMeshProUGUI laserAmmoText;
    private TextMeshProUGUI shotgunAmmoText;

    // Logic variables
    private string [] weapons = {"Default","Laser","Shotgun"};
    private int activeWeapon = 0;
    private bool isRotating = false;

    // Weapon variables
    [Tooltip("Max amount of ammo in default gun")]
    [SerializeField] private int maxDefaultAmmo = 500;

    [Tooltip("Max amount of ammo in laser gun")]
    [SerializeField] private int maxLaserAmmo = 200;

    [Tooltip("Max amount of ammo in shotgun")]
    [SerializeField] private int maxShotgunAmmo = 200;
    public int defaultAmmo;
    public int laserAmmo;
    public int shotgunAmmo;


    
    void Start()
    {

        // Initializing Components and Gameobjects
        gunPivot = GameObject.Find("GunPivot");
        defaultgun = GameObject.Find("DefaultGun");
        laser = GameObject.Find("LaserGun");
        shotgun = GameObject.Find("Shotgun");

        defaultAmmoText = GameObject.Find("DefaultAmmoText").GetComponent<TextMeshProUGUI>();
        laserAmmoText = GameObject.Find("LaserAmmoText").GetComponent<TextMeshProUGUI>();
        shotgunAmmoText = GameObject.Find("ShotgunAmmoText").GetComponent<TextMeshProUGUI>();

        defaultSprite = GameObject.Find("DefaultGunSprite").GetComponent<Image>();
        laserSprite = GameObject.Find("LaserGunSprite").GetComponent<Image>();
        shotgunSprite = GameObject.Find("ShotgunSprite").GetComponent<Image>();

        defaultScript = defaultgun.GetComponent<Shoot_Default>();
        laserScript = laser.GetComponent<Shoot_Laser>();
        shotgunScript = shotgun.GetComponent<Shoot_Shotgun>();

        defaultAmmo = maxDefaultAmmo;
        laserAmmo = maxLaserAmmo;
        shotgunAmmo = maxShotgunAmmo;

        // Setting up UI elements
        defaultAmmoText.text = defaultAmmo + "/" + maxDefaultAmmo;
        laserAmmoText.text = laserAmmo + "/" + maxLaserAmmo;
        shotgunAmmoText.text = shotgunAmmo + "/" + maxShotgunAmmo;

        defaultAmmoText.color = new Color(255,255,255,255);
        defaultSprite.color = new Color(255,255,255,255);
        laserAmmoText.color = new Color (0,0,0,255);
        laserSprite.color = new Color (0,0,0,255);
        shotgunAmmoText.color = new Color (0,0,0,255);
        shotgunSprite.color = new Color (0,0,0,255);

        // Initializing start weapon
        defaultScript.enabled = true;
        laserScript.enabled = false;
        shotgunScript.enabled = false;
        
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q) && !isRotating){
            // transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x %360, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z ));
            changeWeapon();
            
        }
        
    }

    private void changeWeapon(){

        // Activate and deactivate scripts in order to switch control to next weapon 
        switch(activeWeapon){
            case 0:
                defaultScript.enabled = false;
                laserScript.enabled = true;

                defaultAmmoText.color = new Color (0,0,0,255);
                defaultSprite.color = new Color (0,0,0,255);

                laserAmmoText.color = new Color(255,255,255,255);
                laserSprite.color = new Color(255,255,255,255);

                StartCoroutine("rotateWeapon");
                break;
            case 1:
                laserScript.enabled = false;
                shotgunScript.enabled = true;

                laserAmmoText.color = new Color (0,0,0,255);
                laserSprite.color = new Color (0,0,0,255);

                shotgunAmmoText.color = new Color(255,255,255,255);
                shotgunSprite.color = new Color(255,255,255,255);

                StartCoroutine("rotateWeapon");
                break;
            case 2:
                shotgunScript.enabled = false;
                defaultScript.enabled = true;

                shotgunAmmoText.color = new Color (0,0,0,255);
                shotgunSprite.color = new Color (0,0,0,255);

                defaultAmmoText.color = new Color(255,255,255,255);
                defaultSprite.color = new Color(255,255,255,255);

                StartCoroutine("rotateWeapon");
                break;
        }
        activeWeapon = (activeWeapon+1)%3;

    }

    // This coroutine is used in order to create a transition between weapons
    // A central pivot point of all three weapons is rotated 
    IEnumerator rotateWeapon(){

        isRotating = true;

        for (int i = 0; i < 120; i++)
        {
            gunPivot.transform.Rotate(1,0,0);
            yield return null;
        }
        isRotating = false;

    }

    // This function is fired when the player collects ammo from an ammo crate
    public void claimAmmo(){
        switch(activeWeapon){
            case 0:
                defaultAmmo+=150;
                if(defaultAmmo>maxDefaultAmmo)defaultAmmo=maxDefaultAmmo;
                defaultAmmoText.text=defaultAmmo + "/" + maxDefaultAmmo;
                break;
            case 1:
                laserAmmo+=80;
                if(laserAmmo>maxLaserAmmo)laserAmmo=maxLaserAmmo;
                laserAmmoText.text=laserAmmo + "/" + maxLaserAmmo;
                break;
            case 2:
                shotgunAmmo+=60;
                if(shotgunAmmo>maxShotgunAmmo)shotgunAmmo=maxShotgunAmmo;
                shotgunAmmoText.text=shotgunAmmo + "/" + maxShotgunAmmo;
                break;
        }
    }

    // This function is fired when the player shoots
    public void loseAmmo(int ammo){
        switch(activeWeapon){
            case 0:
                defaultAmmo-= ammo;
                defaultAmmoText.text = defaultAmmo + "/" +maxDefaultAmmo;
                break;
            case 1:
                laserAmmo-= ammo;
                laserAmmoText.text = laserAmmo + "/" +maxLaserAmmo;
                break;
            case 2:
                shotgunAmmo-= ammo;
                shotgunAmmoText.text = shotgunAmmo + "/" +maxShotgunAmmo;
                break;
        }
    }
}
