using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Levels Screen");
    }

    public void UnlimitedOvers()
    {
        SceneManager.LoadScene("Unlimited Overs");
    }

    public void QuitGame()
    {
        PlayerPrefs.DeleteKey("lastScore");
        PlayerPrefs.DeleteKey("life");
        Application.Quit();
        print("Quit");
    }

}
