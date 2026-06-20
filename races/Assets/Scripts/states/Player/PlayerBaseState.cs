public abstract class PlayerBaseState
{
    protected FPSMovement player;

    public PlayerBaseState(FPSMovement player)
    {
        this.player = player;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
}