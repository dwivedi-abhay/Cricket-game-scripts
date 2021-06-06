using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    Animator animator;
    int count = 0;
    public Camera cam1;
    public ShotContro shotContro;
    BallThrower ballThrower;
    bool isEndScreen;


    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        animator = GetComponent<Animator>();
        cam1.enabled = true ;
        shotContro = GameObject.FindObjectOfType<ShotContro>();
    }

    // Update is called once per frame
    void Update()
    {
        ballThrower = GameObject.FindObjectOfType<BallThrower>();
        isEndScreen = ballThrower.IsEndScreen();
        if(animator == null)
        {
            return;
        }
        if (isEndScreen == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                GetComponent<Animator>().SetTrigger("Idle to Ready");
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (count == 0)
            {
                var x = shotContro.DirectionCheckerX();
                var y = shotContro.DirectionCheckerY();
                Shots(x, y);
                GetComponent<Animator>().SetTrigger("PlayAnimation");
                cam1.enabled = true;
                count = 1;
               
            }

        }
    }


    private void Shots(float x, float y)
    {
        animator.SetFloat("x", x);
        animator.SetFloat("y", y);
    }
}
