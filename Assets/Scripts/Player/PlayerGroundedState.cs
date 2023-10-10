using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
        {
            if (player.skill.blackhole.cooldownTimer > 0)
                return;

            stateMachine.ChangeState(player.blackholeState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
            stateMachine.ChangeState(player.aimSwordState);

        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
            stateMachine.ChangeState(player.counterAttackState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool HasNoSword()
    {
        if (!player.sword)
            return true;
        
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();

        return false;
    }
}
