using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPrediction : MonoBehaviour
{

    public void Awake()
    {
        FindObjectOfType<GameLogic.GameController>().onStartRomanMove += delegate { this.enabled = false; };
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
