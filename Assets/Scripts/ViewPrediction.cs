using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPrediction : MonoBehaviour
{

    public void Awake()
    {
        FindObjectOfType<GameController>().onStartRomanMove += delegate { this.enabled = false; };
    }

    public float prediction;

    public void Update()
    {
        //Get slider 
        MoveRomans(prediction);
    }

    public void MoveRomans(float amount)
    {
        List<Roman> romans = GlobalRomanManager.Instance.Romans;

        foreach (Roman roman in romans)
        {
            SplineMovement movement = roman.GetComponent<SplineMovement>();
            float max = movement.stopAt;

            movement.MoveTo(max * amount - movement.speed * movement.delay / movement.SplineLength);
        }
    }
}
