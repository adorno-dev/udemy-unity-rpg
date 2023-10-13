public class ShadyIdleState : ShadyGroundedState
{
    public ShadyIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Shady enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
