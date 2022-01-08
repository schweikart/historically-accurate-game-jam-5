using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseIfDetected : MonoBehaviour, IDetected
{
    public void Awake()
    {
        FindObjectOfType<GameController>().onStartRomanMove += delegate { active = true; };
    }

    private bool active = false;

    public void Detected()
    {
        if (active == true)
        {
            FindObjectOfType<GameController>().GermanDetected();
        }
    }
}
