using UnityEngine;

public class SlimeIdleState : EnemyState
{
    protected Enemy_Slime enemy;

    public SlimeIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Slime enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
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