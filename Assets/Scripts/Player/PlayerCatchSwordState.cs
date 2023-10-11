using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName) {}

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        player.fx.PlayDustFX();
        player.fx.ScreenShakeFX(player.fx.shakeSwordImpact);

        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
            player.Flip();
        
        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
    }
}
