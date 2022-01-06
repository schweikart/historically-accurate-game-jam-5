using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackRomanState : BaseState
{

    private Roman roman;
    public AttackRomanState(Roman roman)
    {
        this.roman = roman;
    }

    public override void StartState()
    {
        GameObject.DestroyImmediate(roman.gameObject);
        owner.ChangeState(new MoveToRomanState());
    }
}
