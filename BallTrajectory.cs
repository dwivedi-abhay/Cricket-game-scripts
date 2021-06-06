using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
    Vector3 velocitVectorAtCollision;
    Vector3 velocityVectorAlongXZ;
    BallCollision ballCollision;
    bool isHitWithBat = false;
    int count = 0;


    private void FixedUpdate()
    {

        ballCollision = GameObject.FindObjectOfType<BallCollision>();
        isHitWithBat = ballCollision.HitBat();
        if (isHitWithBat == true && count == 0)
        {
            Invoke("ShowTrajectoryInXZPlane", 0.04f);
            count = 1;
        }
    }
    private void ShowTrajectoryInXZPlane()
    {
        //show trajectory on xz plane on collision with bat
        velocitVectorAtCollision = GetComponent<Rigidbody>().velocity;
        velocityVectorAlongXZ = new Vector3(velocitVectorAtCollision.x, 0, velocitVectorAtCollision.z);
        //velocityVectorAlongXZ = Vector3.ProjectOnPlane(velocityVectorAlongXZ, Vector3.up);
        Debug.DrawRay(transform.position, velocityVectorAlongXZ * (8f - Time.deltaTime), Color.black, 8f);


    }
}
