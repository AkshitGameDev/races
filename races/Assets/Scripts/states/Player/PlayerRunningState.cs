public class PlayerRunningState : PlayerBaseState
{
    public PlayerRunningState(FPSMovement player) : base(player) { }

    public override void EnterState() { }

    public override void UpdateState()
    {
        if (player.JumpPressed() && player.IsGrounded)
        {
            player.SwitchState(player.JumpingState);
            return;
        }

        if (!player.HasMovementInput())
        {
            player.SwitchState(player.IdleState);
            return;
        }

        if (!player.IsSprintPressed())
        {
            player.SwitchState(player.WalkingState);
        }
    }

    public override void FixedUpdateState()
    {
        player.Move(player.sprintSpeed);
    }

    public override void ExitState() { }
}