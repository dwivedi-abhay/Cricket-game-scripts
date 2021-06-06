using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndMenu : MonoBehaviour
{

    public void RestartGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.DeleteKey("lastScore");
        PlayerPrefs.DeleteKey("life");
        PlayerPrefs.DeleteKey("ballsLeft");
        PlayerPrefs.DeleteAll();
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    public void QuitGame()
    {
        PlayerPrefs.DeleteKey("lastScore");
        PlayerPrefs.DeleteKey("life");
        PlayerPrefs.DeleteKey("ballsLeft");
        PlayerPrefs.DeleteAll();
        Application.Quit();
        
    }

    public void NextLevel()
    {
        PlayerPrefs.DeleteKey("lastScore");
        PlayerPrefs.DeleteKey("life");
        PlayerPrefs.DeleteKey("ballsLeft");
        PlayerPrefs.DeleteAll();
        Scene scene = SceneManager.GetActiveScene();
        int sceneCount = SceneManager.sceneCount;
        if (scene.buildIndex + 1 > (sceneCount - 2))
        {
            SceneManager.LoadScene("Levels Screen");
        }
        else
        {
            SceneManager.LoadScene(scene.buildIndex + 1);
        }
    }

    public void MainMenu()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Start Screen");
        Time.timeScale = 1f;
    }
}
