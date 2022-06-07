using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{

    // Components and Gameobjects
    private Animator animator;
    private GameObject text;
    private WeaponManager weaponManager;

    // Logic variables
    private bool isClose = false;
    private float timeLeftForClaim = 0f;
    void Start()
    {

        animator = GetComponent<Animator>();
        text = transform.Find("ClaimAmmoText").gameObject;
        text.SetActive(false);
        weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
        
    }

    void Update()
    {
        timeLeftForClaim -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.E) && isClose){
            if(timeLeftForClaim<=0){
                weaponManager.claimAmmo();
                timeLeftForClaim = 5f;}
        }
        
    }

    void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
            isClose = true;
            animator.Play("OpenChestAction");
            text.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col){
        if(col.tag == "Player"){
            isClose = false;
            animator.Play("IdleCrate");
            text.SetActive(false);
        }
    }
}
