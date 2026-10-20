using UnityEngine;

// Moves the player with a Rigidbody: 
// A/D or Left/Right arrows to ROTATE, W/S or Up/Down to move Forward/Backward, Space to jump.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkMoveSpeed = 5f;      // units per second
    [SerializeField] private float runMoveSpeed = 10f;      // units per second
    [SerializeField] private float turnSpeed = 150f;    // degrees per second
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxChargeTime = 3f;
    [SerializeField] private float doubleJumpForce = 4f;
    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; // ray length
    [SerializeField] private LayerMask groundMask;        // ground layers

    private Rigidbody rb;               // the physics body we move
    private Vector3 moveDirection;      // worked out from input in Update()
    private float turnInput;            // horizontal input for rotation
    private bool isGrounded;            // true when standing on the ground
    private bool jumpRequested = false; // set in Update(), used later
    private float jumpChargeTime = 0.0f;
    private bool isChargingJump = false;
    public bool canDoubleJump { get; set; } = false;
    private bool doubleJumpRequested = false; // set in TryDoubleJump()
    private bool hasDoubleJumped = false;     // one per jump
    // Multiplies the normal speed. 1 = normal, 1.5 = 50% faster.
    // Other scripts (like PlayerPowerUps) are allowed to change it.
    public float SpeedMultiplier { get; set; } = 1f;


    public bool IsGrounded => isGrounded;
    public bool CanDoubleJump => canDoubleJump;
    private bool isRunning;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        // 1. Ground Check
        isGrounded = Physics.Raycast(transform.position + transform.up*groundDistance/2, -transform.up, groundDistance, groundMask);

        // Back on the ground: the double jump is available again
        if (isGrounded)
        {
            hasDoubleJumped = false;
        }


        // 2. Read Inputs
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right turn input
        float moveZ = Input.GetAxisRaw("Vertical");  // W/S or Up/Down forward/backward
        isRunning = Input.GetKey(KeyCode.LeftShift);// checks if you are running


        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        // Rotate the player transform in Update for responsive turning visuals
        // Calculate forward movement relative to current facing direction
        moveDirection = cameraForward * moveZ + cameraRight * moveX;

        if (moveDirection.magnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                turnSpeed * Time.deltaTime);
        }

        // 3. Handle Jump Input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isChargingJump = true;
            jumpChargeTime = 0f;
        }
        if (isChargingJump && Input.GetKey(KeyCode.Space))
        {
            jumpChargeTime += Time.deltaTime;// use delta time so the calculation works properly

            if (jumpChargeTime >= maxChargeTime) // dont let it go over the max charge time
            {
                jumpChargeTime = maxChargeTime;
                jumpRequested = true;
                isChargingJump = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)// when you let go request jump
        {
            jumpRequested = true;
            isChargingJump = false;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
 
        if (jumpRequested)
        {
            Jump(jumpForce, jumpChargeTime);
            jumpRequested = false;
            jumpChargeTime = 0.0f;
        }
 
        // NEW: the second jump, in mid-air
        if (doubleJumpRequested && canDoubleJump)
        {
            Jump(doubleJumpForce, 1.0f);
            doubleJumpRequested = false;
        }
    }
 
    private void Jump(float force, float chargeTime)
    {
        if(chargeTime < 1.0f)// make sure if you barely tap jump you still jump a decent amount.
        {
            chargeTime = 1.0f;
        }

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f,
                                        rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force * chargeTime, ForceMode.VelocityChange);
    }


    private void MovePlayer()
    {
        // Calculate velocity based on current forward vector
        if(isRunning)
        {
            // Apply movement while preserving vertical velocity (gravity/jumping)

            Vector3 targetVelocity = moveDirection * runMoveSpeed * SpeedMultiplier;
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            // Apply movement while preserving vertical velocity (gravity/jumping)

            Vector3 targetVelocity = moveDirection * walkMoveSpeed * SpeedMultiplier;
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        }
    }

     // Tries to jump a second time in mid-air.
    // Returns true if the double jump really happened.
    public bool TryDoubleJump()
    {
        // Not allowed on the ground, or if it was already used
        if (isGrounded || hasDoubleJumped || !canDoubleJump) return false;
 
        doubleJumpRequested = true;
        hasDoubleJumped = true;
        return true;
    }

}