using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FPSMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    public Rigidbody Rb { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public bool IsGrounded { get; private set; }

    private PlayerBaseState currentState;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkingState WalkingState { get; private set; }
    public PlayerRunningState RunningState { get; private set; }
    public PlayerJumpingState JumpingState { get; private set; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Rb.freezeRotation = true;

        IdleState = new PlayerIdleState(this);
        WalkingState = new PlayerWalkingState(this);
        RunningState = new PlayerRunningState(this);
        JumpingState = new PlayerJumpingState(this);
    }

    private void Start()
    {
        SwitchState(IdleState);
    }

    private void Update()
    {
        ReadInput();
        GroundCheck();

        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    private void ReadInput()
    {
        MoveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) MoveInput += Vector2.up;
        if (Keyboard.current.sKey.isPressed) MoveInput += Vector2.down;
        if (Keyboard.current.dKey.isPressed) MoveInput += Vector2.right;
        if (Keyboard.current.aKey.isPressed) MoveInput += Vector2.left;

        MoveInput = Vector2.ClampMagnitude(MoveInput, 1f);
    }

    private void GroundCheck()
    {
        IsGrounded = Physics.Raycast(
            groundCheckPoint.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }

    public bool HasMovementInput()
    {
        return MoveInput.sqrMagnitude > 0.01f;
    }

    public bool IsSprintPressed()
    {
        return Keyboard.current.leftShiftKey.isPressed;
    }

    public bool JumpPressed()
    {
        return Keyboard.current.spaceKey.wasPressedThisFrame;
    }

    public void Move(float speed)
    {
        Vector3 moveDirection =
            transform.forward * MoveInput.y +
            transform.right * MoveInput.x;

        moveDirection.Normalize();

        Vector3 targetVelocity = moveDirection * speed;

        Vector3 currentVelocity = Rb.linearVelocity;

        Vector3 velocityChange = targetVelocity - new Vector3(
            currentVelocity.x,
            0f,
            currentVelocity.z
        );

        Rb.AddForce(
            velocityChange * acceleration * Time.fixedDeltaTime,
            ForceMode.VelocityChange
        );
    }

    public void Jump()
    {
        Vector3 velocity = Rb.linearVelocity;
        velocity.y = 0f;
        Rb.linearVelocity = velocity;

        Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(
            groundCheckPoint.position,
            Vector3.down * groundCheckDistance
        );
    }
}