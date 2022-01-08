using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAttackManager : MonoBehaviour
{

    public event Action attackStarts;

    [SerializeField]
    private Roman roman;

    public void Awake()
    {
        roman.GetComponent<SplineMovement>().onFinished += delegate { attackStarts(); };
    }

    public void StartAttack()
    {
        attackStarts();
    }

}
