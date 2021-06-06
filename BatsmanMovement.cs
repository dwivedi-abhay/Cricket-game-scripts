using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatsmanMovement : MonoBehaviour
{
    public float batsmanSpeed = 2f;
    int count;
    public bool movementOn = true;
    public float batsMovement = 4f;

    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(MovementOff());
        }

        if (movementOn == true)
        {
            BatsMovement();
        }

    }

    IEnumerator MovementOff()
    {
        yield return new WaitForSeconds(batsMovement);
        movementOn = false;
    }


    private void BatsMovement()
    {
        float xThrow = Input.GetAxis("horizontal");
        float xOffset = xThrow * batsmanSpeed * Time.unscaledDeltaTime;
        float rawXNew = transform.position.x + xOffset;
        float clampedXNew = Mathf.Clamp(rawXNew, -0.91f, 0.91f);
        transform.position = new Vector3(clampedXNew, transform.position.y, transform.position.z);
    }
}
