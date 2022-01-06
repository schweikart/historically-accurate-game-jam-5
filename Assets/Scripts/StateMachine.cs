using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    private BaseState state;

    public void Start()
    {
        if (state != null)
        {
            state.owner = this;
            state.StartState();
        }
    }

    private void Update()
    {
        state.UpdateState();
    }

    private void FixedUpdate()
    {
        state.FixedUpdateState();
    }

    public void ChangeState(BaseState newState)
    {
        if (state != null)
        {
            state.LeaveState();
        }

        state = newState;
        state.owner = this;
        newState.StartState();
    }
}
