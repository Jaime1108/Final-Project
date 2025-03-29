using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 7f; // Jump force

    private Rigidbody rb;
    private Vector3 movementDirection;
    private bool isGrounded;
    
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
        rb.freezeRotation = true; // Prevent unwanted rotation
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovementInput();
        RotateCharacter();
        CheckGroundStatus();
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // Jump only if grounded
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    void HandleMovementInput()
{
    float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
    float moveZ = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

    movementDirection = new Vector3(moveX, 0f, moveZ).normalized; // Normalize to prevent diagonal speed boost

    bool isMoving = movementDirection.magnitude > 0;
    //Debug.Log($"MoveX: {moveX}, MoveZ: {moveZ}, IsMoving: {isMoving}");

    if (animator != null)
    {
        animator.SetBool("IsMoving", isMoving);
    }
}

    void MoveCharacter()
    {
        Vector3 moveVelocity = movementDirection * speed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z); // Keep Y velocity unchanged
    }

    void RotateCharacter()
    {
        if (movementDirection != Vector3.zero) // Rotate only when moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smooth rotation
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Apply upward force
    }

    void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f); // Simple ground check
    }
}
