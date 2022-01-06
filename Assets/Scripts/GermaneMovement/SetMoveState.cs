using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMoveState : MonoBehaviour
{
    void Start()
    {
        GetComponent<StateMachine>().ChangeState(new MoveToRomanState());
        Destroy(this);
    }
}
