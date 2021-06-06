using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;




public class BallThrower : MonoBehaviour
{
    GameObject ball;
    Vector3 pitchPos;
    Quad quad;
    Vector3 ballDirection;
    int count = 0;
    Rigidbody ballRigidbody;
    public float multiplyingFactor = 1f;
    public Camera cameraAfterBallThrow;
    public Camera cameraBeforeBallThrow;
    public float afterBallThrowSeconds = 1f;
    public float endSceneTime = 15f;
    public int life = 3;
    public int ballsLeft = 10;
    public int overs = 19;
    public int targetScore = 100;
    public int score = 85;
    BallCollision ballCollision;
    //int ballColBat = 0;
    public GameObject endScreen;
    public GameObject scoreScreen;
    public GameObject winScreen;
    bool isEndScreen = false;
    public int totalOvers = 20;
    public GameObject pressSpace;
    public TextMeshProUGUI oversText;
    public TextMeshProUGUI ballsLeftText;
    public TextMeshProUGUI ballsText;
    public TextMeshProUGUI leftRuns;
    public TextMeshProUGUI currRR;
    public TextMeshProUGUI reqRR;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        quad = GameObject.FindObjectOfType<Quad>();

        SetPlayerPrefs();
        GetPlayerPrefs();

        if (PlayerPrefs.GetInt("balls") > 0)
        {
            if (PlayerPrefs.GetInt("balls") % 6 == 0)
            {
                overs++;
                PlayerPrefs.SetInt("overs", overs);
                PlayerPrefs.SetInt("balls", 0);
            }
        }

        SetScoreUI();

        CheckIfWon();

        CheckIfLost();

    }

    // Update is called once per frame
    void Update()
    {
        if (isEndScreen == true)
        {
            pressSpace.SetActive(false);
        }
        if (count == 0 && isEndScreen == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pressSpace.SetActive(false);
                quad = quad.QuadSelection();
                GetComponent<Animator>().SetTrigger("StartRunUp");
                quad.GetComponent<MeshRenderer>().material.color = Color.blue;
                count = 1;

            }
        }

        
        
        oversText.text = PlayerPrefs.GetInt("overs").ToString();
        

    }


    IEnumerator CameraSwitch()
    {
        yield return new WaitForSeconds(afterBallThrowSeconds);
        cameraBeforeBallThrow.enabled = false;
        cameraAfterBallThrow.enabled = true;

    }


    private void CheckIfWon()
    {
        if (PlayerPrefs.GetInt("lastScore") >= targetScore)
        {
            print(" u win");
            isEndScreen = true;
            winScreen.SetActive(true);
            scoreScreen.SetActive(false);
            //**** IMP ***********load win screen and reset all the player prefs
        }
    }

    private void CheckIfLost()
    {
        if ((life == 10 || ballsLeft == 0 || PlayerPrefs.GetInt("overs") == totalOvers) && (PlayerPrefs.GetInt("lastScore") < PlayerPrefs.GetInt("targetScore")))
        {
            isEndScreen = true;
            endScreen.SetActive(true);
            scoreScreen.SetActive(false);
        }
    }

    private void SetScoreUI()
    {
        ballsText.text = PlayerPrefs.GetInt("balls").ToString();
        ballsLeftText.text = PlayerPrefs.GetInt("ballsLeft").ToString();
        leftRuns.text = (PlayerPrefs.GetInt("targetScore") - PlayerPrefs.GetInt("lastScore")).ToString();
        float noOfOvers = Convert.ToSingle(PlayerPrefs.GetInt("overs"));
        float floatScore = Convert.ToSingle(PlayerPrefs.GetInt("lastScore"));
        float targetscore = Convert.ToSingle(PlayerPrefs.GetInt("targetScore"));
        float currRunRate = floatScore/noOfOvers;
        float fccr = (float)Math.Round(currRunRate * 100f) / 100f;
        currRR.text = fccr.ToString();

        if (20 - PlayerPrefs.GetInt("overs") > 0)
        {
            float reqRunRate = (targetscore - floatScore) / (totalOvers - noOfOvers);
            float fcrr = (float)Math.Round(reqRunRate * 100f) / 100f;
            reqRR.text = (reqRunRate).ToString();
        }
    }

    private void GetPlayerPrefs()
    {
        ballsLeft = PlayerPrefs.GetInt("ballsLeft");
        life = PlayerPrefs.GetInt("life");
        overs = PlayerPrefs.GetInt("overs");
        targetScore = PlayerPrefs.GetInt("targetScore");
    }

    private void SetPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("targetScore") == false)
        {
            PlayerPrefs.SetInt("targetScore", targetScore);
        }
        if (PlayerPrefs.HasKey("lastScore") == false)
        {
            PlayerPrefs.SetInt("lastScore", score);
        }
        if (PlayerPrefs.HasKey("balls") == false)
        {
            PlayerPrefs.SetInt("balls", 0);
        }
        if (PlayerPrefs.HasKey("overs") == false)
        {
            PlayerPrefs.SetInt("overs", overs);
        }
        if (PlayerPrefs.HasKey("ballsLeft") == false)
        {
            PlayerPrefs.SetInt("ballsLeft", ballsLeft);
        }

        if (PlayerPrefs.HasKey("life") == false)
        {
            PlayerPrefs.SetInt("life", life);
        }
    }

    //IEnumerator RestartScene()
    //{
    //    yield return new WaitForSeconds(endSceneTime);
    //    ballCollision = GameObject.FindObjectOfType<BallCollision>();
    //    ballColBat = ballCollision.BatCollisionCount();
    //    if (ballColBat == 0)
    //    {
    //        life--;
    //    }
    //    PlayerPrefs.SetInt("life", life);
    //    SceneManager.LoadScene("MainScreen");
    //}




    public void BallThrow()
    {
        ballsLeft--;
        int balls = PlayerPrefs.GetInt("balls");
        balls++;
        PlayerPrefs.SetInt("balls", balls);
        PlayerPrefs.SetInt("ballsLeft", ballsLeft);
        ball = GameObject.FindGameObjectWithTag("Ball");
        pitchPos = quad.transform.position;
        ball.transform.parent = null;
        /*
        ballDirection = (pitchPos - ball.transform.position).normalized;
        ball.GetComponent<Rigidbody>().AddForce(ballDirection * ballThrowForce);
        */
        ball.GetComponent<Rigidbody>().useGravity = true;

        ball.GetComponent<Rigidbody>().velocity = GetBallVelocity();
        StartCoroutine(CameraSwitch());
        //StartCoroutine(RestartScene());

    }

    private Vector3 GetBallVelocity()
    {
        float displacementY = quad.transform.position.y - ball.transform.position.y;
        Vector3 displacementXZ = new Vector3(quad.transform.position.x - ball.transform.position.x, 0, quad.transform.position.z - ball.transform.position.z);
        float dividingFactor = (Mathf.Sqrt(-2 * displacementY / Physics.gravity.magnitude));
        Vector3 velocityXZ = (displacementXZ) / dividingFactor;

        return velocityXZ * multiplyingFactor;
    }


    public int LifeCount()
    {
        return life;
    }

    public bool IsEndScreen()
    {
        return isEndScreen;
    }

}
