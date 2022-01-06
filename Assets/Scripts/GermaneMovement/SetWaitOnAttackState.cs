using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWaitOnAttackState : MonoBehaviour
{
    void Start()
    {
        GetComponent<StateMachine>().ChangeState(new WaitToAttackState(FindObjectOfType<StartAttackManager>()));
        Destroy(this);
    }
}
