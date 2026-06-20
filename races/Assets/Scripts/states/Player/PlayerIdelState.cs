public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(FPSMovement player) : base(player) { }

    public override void EnterState() { }

    public override void UpdateState()
    {
        if (player.JumpPressed() && player.IsGrounded)
        {
            player.SwitchState(player.JumpingState);
            return;
        }

        if (player.HasMovementInput())
        {
            if (player.IsSprintPressed())
                player.SwitchState(player.RunningState);
            else
                player.SwitchState(player.WalkingState);
        }
    }

    public override void FixedUpdateState()
    {
        player.Move(0f);
    }

    public override void ExitState() { }
}