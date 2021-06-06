using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    Boundaries boundaries;
    public int score;
    int count = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (count == 0)
        {
            boundaries = GameObject.FindObjectOfType<Boundaries>();
            score += boundaries.ShowScore();
            count = 1;
        }


       
    }

    public int ScoreToItsUI()
    {
        return score;
    }




}
