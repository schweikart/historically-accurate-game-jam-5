

using UnityEngine;

public class WaitToAttackState : BaseState
{

    private StartAttackManager manager;

    public WaitToAttackState(StartAttackManager manager)
    {
        Debug.Assert(manager != null);
        this.manager = manager;
    }

    public override void StartState()
    {
        base.StartState();

        manager.attackStarts += delegate { StartAttacking(); };
    }

    private void StartAttacking()
    {
        owner.ChangeState(new MoveToRomanState());
    }
}
