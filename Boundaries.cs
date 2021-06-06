using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boundaries : MonoBehaviour
{

    BallCollision ballCollision;
    int count = 0;
    ShowBoundaries showBoundaries;
    int score;
    int highscore = 0;
    float endSceneTime = 3f;
    bool goneForBoundary = false;




    // Start is called before the first frame update
    void Start()
    {

        float boundary = new Vector2(-2f, -67f).magnitude;

        score = PlayerPrefs.GetInt("lastScore");
        highscore = PlayerPrefs.GetInt("highScore");



    }

    // Update is called once per frame
    void Update()
    {
        ShowingBoundaryCanvas();
        if (PlayerPrefs.GetInt("lastScore") > highscore)
        {
            highscore = PlayerPrefs.GetInt("lastScore");
            PlayerPrefs.SetInt("highScore", highscore);
        }

    }

    private void ShowingBoundaryCanvas()
    {
        ballCollision = GameObject.FindObjectOfType<BallCollision>();
        if (count == 0)
        {
            if ((new Vector2(gameObject.transform.position.x, gameObject.transform.position.z).magnitude >= 94f || ballCollision.HitBoundary()) && (ballCollision.HitFielder() == false))
            {
                ballCollision = GameObject.FindObjectOfType<BallCollision>();
                showBoundaries = GameObject.FindObjectOfType<ShowBoundaries>();
                int groundBounce = ballCollision.CountBounce();

                if (groundBounce == 1)
                {
                    showBoundaries.ShowSix();
                    score = PlayerPrefs.GetInt("lastScore");
                    score += 6;
                    PlayerPrefs.SetInt("lastScore", score);
                }
                else
                {
                    showBoundaries.ShowFour();
                    score = PlayerPrefs.GetInt("lastScore");
                    score += 4;
                    PlayerPrefs.SetInt("lastScore", score);
                }
                count = 1;
                PlayerPrefs.SetInt("lastScore", score);
                goneForBoundary = true;
                StartCoroutine(RestartScene());
            }
            
        }
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(endSceneTime);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("lastScore");
        PlayerPrefs.DeleteKey("life");
    }

    public int ShowScore()
    {
        
        return score;
        
    }

    public int HighScoreToItsUI()
    {
        return highscore;
    }

    public bool GoneForBoundary()
    {
        return goneForBoundary;
    }


}
