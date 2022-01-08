using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomanSpawner : MonoBehaviour
{
    public GameObject prefab;

    public int numberOfRows;

    public int romansPerRow;

    public float distanceBetweenRomans;

    public float delayBetweenRomanRows;

    public BezierSpline spline;

    public void Awake()
    {
        for (int i = 0; i < numberOfRows; i++)
        {
            
        }
    }
}
