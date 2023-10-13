using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    private Enemy_DeathBringer enemy;

    public DeathBringerAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_DeathBringer enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.chanceToTeleport += 5;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            if (enemy.CanTeleport())
                stateMachine.ChangeState(enemy.teleportState);
            else
                stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }
}
