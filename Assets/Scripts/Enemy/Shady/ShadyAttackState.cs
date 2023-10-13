using UnityEngine;

public class ShadyAttackState : EnemyState
{
    protected Enemy_Shady enemy;

    public ShadyAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Shady enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }
}
