using UnityEngine;
 
// Sends the player's movement to the Animator every frame
// so the right animation plays at the right time.
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;       // plays the animations
    private PlayerMovement movement; // our movement script
    private Rigidbody rb;            // to read the player's speed
    [SerializeField] private AudioClip doubleJumpSound;
    private AudioSource audioSource;
 
    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
 
    private void Update()
    {
        // Send the current speed to the blend tree
        Vector3 characterSpeed = rb.linearVelocity;
        characterSpeed.y = 0;
        animator.SetFloat("CharacterSpeed", characterSpeed.magnitude);
 
        // On the ground? (controls Falling and ends the flip)
        bool grounded = movement.IsGrounded;
        bool doubleJump = movement.CanDoubleJump;
        animator.SetBool("IsGrounded", grounded);
        animator.SetBool("CanDoubleJump", doubleJump);
 
        if (grounded)
        {
            // NEW: throw away a double jump request that was never used
            animator.ResetTrigger("doDoubleJump");
        }
        else
        {
            // NEW: Double jump. Space again in mid-air.
            // The flip only plays if PlayerMovement really jumped.
            if (Input.GetButtonDown("Jump") && movement.TryDoubleJump())
            {
                animator.SetTrigger("doDoubleJump");
                audioSource.PlayOneShot(doubleJumpSound);
            }
        }
    }
}
