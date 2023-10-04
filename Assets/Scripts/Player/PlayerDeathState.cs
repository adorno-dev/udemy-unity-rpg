public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
