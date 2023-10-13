using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private Enemy_DeathBringer enemy;
    private Transform player;

    public DeathBringerIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_DeathBringer enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(player.transform.position, enemy.transform.position) < 7)
            enemy.bossFightBegun = true;

        if (Input.GetKeyDown(KeyCode.V))
            stateMachine.ChangeState(enemy.teleportState);

        if (stateTimer < 0 && enemy.bossFightBegun)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
