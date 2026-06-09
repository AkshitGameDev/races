using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
public class FPSMovement : MonoBehaviour{
   
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

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        ReadInput();
        GroundCheck();

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReadInput()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1f;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1f;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1f;

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(
            groundCheckPoint.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }

    private void Move()
    {
        bool isSprinting = Keyboard.current.leftShiftKey.isPressed;
        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 moveDirection =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        moveDirection.Normalize();

        Vector3 targetVelocity = moveDirection * targetSpeed;

        Vector3 currentVelocity = rb.linearVelocity;

        Vector3 velocityChange = targetVelocity - new Vector3(
            currentVelocity.x,
            0f,
            currentVelocity.z
        );

        rb.AddForce(
            velocityChange * acceleration * Time.fixedDeltaTime,
            ForceMode.VelocityChange
        );
    }

    private void Jump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
