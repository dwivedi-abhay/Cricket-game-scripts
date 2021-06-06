using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAndInfo : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject infoCanvas;
    public static bool isPaused = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                OpenPauseMenu();
            }
        }
    }

    public void OpenInfoMenu()
    {
        infoCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        infoCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    

}
