using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour
{
    System.Random rand = new System.Random();
    Quad[] quad;
    Quad selectedQuad;
    int index;
    Vector3 selectedQuadPos;



    // Start is called before the first frame update
    void Start()
    {
        quad = FindObjectsOfType<Quad>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
       
        
    }

    public Quad QuadSelection()
    {
        quad = FindObjectsOfType<Quad>();
        index = rand.Next(quad.Length);
        selectedQuad = quad[index];
        selectedQuad.gameObject.tag = "Clicked";

        return selectedQuad;

    }
}
