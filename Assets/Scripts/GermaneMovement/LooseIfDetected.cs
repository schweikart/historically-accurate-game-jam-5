using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseIfDetected : MonoBehaviour, IDetected
{
    public void Awake()
    {
        FindObjectOfType<GameLogic.GameController>().onStartRomanMove += delegate { active = true; };
    }

    private bool active = false;

    public void Detected()
    {
        if (active == true)
        {
            FindObjectOfType<GameLogic.GameController>().GermanDetected();
        }
    }
}
