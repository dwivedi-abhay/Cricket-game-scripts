using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenReadyCameraAnimation : MonoBehaviour
{
    int count = 0;
    BallThrower ballThrower;
    bool isEndScreen;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ballThrower = GameObject.FindObjectOfType<BallThrower>();
        isEndScreen = ballThrower.IsEndScreen();
        if (count == 0 && isEndScreen == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                GetComponent<Animator>().SetTrigger("WhenReady");
                count = 1;
            }
        }
    }
}
