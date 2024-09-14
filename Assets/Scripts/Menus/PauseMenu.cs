using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] public GameObject pauseMenu;
    private bool gamePaused = false;


    void Update()
    {
        //Check whether Escape has been pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
                Resume();
            else
                Pause();

        }

    }

    public void Pause()
    {
        gamePaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }


    public void Resume()
    {
        gamePaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
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
