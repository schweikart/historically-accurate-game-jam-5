
public abstract class BaseState
{
    public StateMachine owner;

    public virtual void StartState() { }

    public virtual void UpdateState() { }

    public virtual void FixedUpdateState() { }

    public virtual void LeaveState() { }
}
