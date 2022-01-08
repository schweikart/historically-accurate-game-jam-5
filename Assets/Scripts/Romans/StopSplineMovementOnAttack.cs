using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SplineMovement))]
public class StopSplineMovementOnAttack : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<StartAttackManager>().attackStarts += delegate { StopMoving(); };
    }

    private void StopMoving()
    {
        GetComponent<SplineMovement>().Stop();
    }
}
