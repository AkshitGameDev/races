using UnityEngine;



    public class PlayerPickUPState : PlayerBaseState
    {
        public PlayerPickUPState(FPSMovement player) : base(player) { }
        public override void EnterState()
        {
            Debug.Log("Entered PlayerPickUPState");
            
        }

        public override void UpdateState()
        {
            
        }

        public override void FixedUpdateState()
        {
            
        }
        public override void ExitState()
        {
            Debug.Log("Exited PlayerPickUPState");
           
        }
    }

