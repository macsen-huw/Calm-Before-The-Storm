using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{

    public GameObject deathMenu;
    public AudioManager audioManager;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsDead()
    {
        return isDead;
    }
    public void HasDied()
    {

        isDead = true;
        //Stop the game
        audioManager.PauseAll();

        deathMenu.SetActive(true);
    }


    //Make sure the game doesn't stay in the death state
    void ResetObjects()
    {
        isDead = false;
        audioManager.ResumeAll();

        deathMenu.SetActive(false);
    }

    //Temporarily back to the start of Section B again
    public void Respawn()
    {
        ResetObjects();
        SceneManager.LoadScene("Main Scene");
    }

    //Take back to main menu
    public void ReturnToMenu()
    {
        ResetObjects();
        SceneManager.LoadScene("Main Menu");
    }

}
