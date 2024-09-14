using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public DeathScreen deathScreen;
    public AudioManager audioManager;
    private bool gamePaused = false;

    void Update()
    {
        //Check whether Escape has been pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            //Not allowed to pause while dead
            if(!deathScreen.IsDead())
            {
                if (gamePaused)
                    Resume();
                else
                    Pause();
            }
            

        }

    }

    public void Pause()
    {
        gamePaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

        audioManager.PauseAll();
    }


    public void Resume()
    {
        gamePaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

        audioManager.ResumeAll();
    }

     public void BackToMenu()
    {
        //Resume so we're not stuck on pause
        gamePaused = false;
        Time.timeScale = 1;


        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
