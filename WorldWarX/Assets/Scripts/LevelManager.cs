using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private GameObject controls; 

    void Start(){
        if( SceneManager.GetActiveScene().buildIndex ==0){
            controls = GameObject.Find("Controls");
            controls.SetActive(false);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    // This function is fired to load the main scene
    public void loadGame(){
        SceneManager.LoadScene(1);
    }

    // This function is used to show the controls
    public void showControls(){
        controls.SetActive(true);

    }

    // This function is used to hide the controls
    public void hideControls(){
        controls.SetActive(false);

    }

    // This function is used to quit the game
    public void exitGame(){
        Application.Quit();
    }
}
