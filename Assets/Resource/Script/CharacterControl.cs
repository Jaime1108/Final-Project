using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    public float walkspeed = 3f;
    public float sprintSpeed = 6f;
    public float jumpForce = 7f;

    public float staminaDrainRate = 1f;
    public float staminaRecoveryRate = 1.5f;

    public PlayerStat playerStat;

    private Rigidbody rb;
    private Vector3 movementDirection;
    private bool isGrounded;
    public bool isMoving = false;
    public bool isSprinting = false;

    private Animator animator;
    public AudioSource walking;
    public AudioClip walkingClip;
    public AudioManager audioManager;
    float volumeSFX;

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null){
            volumeSFX = 1f;
        }else{
            volumeSFX= audioManager.masterVolume*audioManager.sfxVolume;
        }
        playerStat = FindFirstObjectByType<PlayerStat>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        walking.volume = volumeSFX*0.5f;
    }

    void Update()
    {
        MovementControl();
        rotateCharacter();
        checkGroundStatus();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jump();
        }
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsSprinting", isSprinting);
        useStamina();
        if (isMoving){
            if (!walking.isPlaying){
                walking.Play();
            }
        }
        else{
            if (walking.isPlaying){
                walking.Stop();
            }
        }
    }

    void FixedUpdate()
    {
        moveCharacter();
    }

    void MovementControl()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector3(moveX, 0f, moveZ).normalized;

        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift);
        isSprinting = wantsToSprint && movementDirection.magnitude > 0 && playerStat.stamina > 0;

        isMoving = movementDirection.magnitude > 0;

        if (animator != null)
        {
            
        }
    }

    void moveCharacter()
    {
        float currentSpeed = 0f;
        if(isSprinting && playerStat.stamina > 0.1f){
            currentSpeed = sprintSpeed;
        }
        else{
            currentSpeed = walkspeed;
        }
        Vector3 moveVelocity = movementDirection * currentSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    void rotateCharacter()
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    void checkGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void useStamina()
    {
        if (isSprinting)
        {
            playerStat.stamina -= staminaDrainRate * Time.deltaTime;
        }
        else if (playerStat.stamina < playerStat.maxStamina)
        {
            playerStat.stamina += staminaRecoveryRate * Time.deltaTime;
        }

        playerStat.stamina = Mathf.Clamp(playerStat.stamina, 0f, playerStat.maxStamina);  


        Debug.Log($"Current Stamina: {playerStat.stamina:F2}");
    }
}
