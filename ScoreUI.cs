using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    int scoreValue;
    Boundaries boundaries;
    BallThrower ballThrower;
    int highscore;
    int life = 3;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI lifeText;


    // Start is called before the first frame update
    void Start()
    {
        ballThrower = GameObject.FindObjectOfType<BallThrower>();
        boundaries = GameObject.FindObjectOfType<Boundaries>();
    }

    // Update is called once per frame
    void Update()
    {
        ballThrower = GameObject.FindObjectOfType<BallThrower>();
        boundaries = GameObject.FindObjectOfType<Boundaries>();
        life = ballThrower.LifeCount();
        lifeText.text = life.ToString();
        scoreText.text = PlayerPrefs.GetInt("lastScore").ToString();
        //highscoreText.text = PlayerPrefs.GetInt("highScore").ToString();

    }
}
