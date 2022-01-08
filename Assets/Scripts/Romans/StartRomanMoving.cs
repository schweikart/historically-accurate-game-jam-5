using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRomanMoving : MonoBehaviour
{
    public void Awake()
    {
        FindObjectOfType<GameLogic.GameController>().onStartRomanMove += delegate { GetComponent<SplineMovement>().Resume();};
    }
}
