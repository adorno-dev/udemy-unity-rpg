using UnityEngine;

public class SlimeGroundedState : EnemyState
{
    protected Transform player;
    protected Enemy_Slime enemy;

    public SlimeGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Slime enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.agroDistance)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}