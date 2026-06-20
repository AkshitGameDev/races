public class PlayerJumpingState : PlayerBaseState
{
    private bool hasJumped;

    public PlayerJumpingState(FPSMovement player) : base(player) { }

    public override void EnterState()
    {
        hasJumped = true;
        player.Jump();
    }

    public override void UpdateState()
    {
        if (hasJumped && player.IsGrounded && player.Rb.linearVelocity.y <= 0.1f)
        {
            if (!player.HasMovementInput())
                player.SwitchState(player.IdleState);
            else if (player.IsSprintPressed())
                player.SwitchState(player.RunningState);
            else
                player.SwitchState(player.WalkingState);
        }
    }

    public override void FixedUpdateState()
    {
        float airSpeed = player.IsSprintPressed()
            ? player.sprintSpeed
            : player.walkSpeed;

        player.Move(airSpeed);
    }

    public override void ExitState() { }
}