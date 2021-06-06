using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotContro : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        DirectionCheckerX();
        DirectionCheckerY();
        
    }

    public float DirectionCheckerX()
    {
        float x;
        if(Input.GetKey(KeyCode.A))
        {
            x = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            x = 1f;
        }
        else
        {
            x = 0f;
        }
        return x;
    }

    public float DirectionCheckerY()
    {
        float y;
        if (Input.GetKey(KeyCode.W))
        {
            y = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            y = -1f;
        }
        else
        {
            y = 0f;
        }
        return y;
    }
}
