using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBoundaries : MonoBehaviour
{
    public GameObject boundaryUI6;
    public GameObject boundaryUI4;

    public void ShowSix()
    {
        boundaryUI6.SetActive(true);
    }

    public void ShowFour()
    {
        boundaryUI4.SetActive(true);
    }
}
