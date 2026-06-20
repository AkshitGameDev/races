using UnityEngine;

public abstract class PlayerBaseState 
{
    public abstract void EnterState(FPSMovement player);
    public abstract void UpdateState(FPSMovement player);
}
