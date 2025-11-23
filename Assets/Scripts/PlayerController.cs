using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // [SerializeField] forces fields to appear in the Inspector, so you can tweak them without editing code.
    [SerializeField] private float moveSpeed = 4f;       // How fast the player moves left/right
    [SerializeField] private float jumpForce = 8f;      // How strong the jump is (vertical speed)
    [SerializeField] private Transform groundCheck;      // Empty child object placed at the player's feet
    [SerializeField] private float groundCheckRadius = 0.2f; // Size of the circle used to detect ground
    [SerializeField] private LayerMask groundLayer;      // Which layer counts as "ground" (set in Inspector)

    // Private fields are used internally by the script.
    // new keyword is used to intentionally override Component.rigidbody, which is deprecated
    private new Rigidbody2D rigidbody;            // Reference to the Rigidbody2D component
    private bool isGrounded;           // True if player is standing on ground
    private Animator animator;         //Reference to Player's animator

    void Start()
    {
        // Grab the Rigidbody2D attached to the Player object once at the start.
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();    //Getting the animator component on the player
    }

    void Update()
    {
        // --- Horizontal movement ---
        // Get input from keyboard (A/D or Left/Right arrows).
        float moveInput = Input.GetAxis("Horizontal");
        // Apply horizontal speed while keeping the current vertical velocity.
        rigidbody.linearVelocity = new Vector2(moveInput * moveSpeed, rigidbody.linearVelocity.y);

        // --- Jump ---
        // If player is grounded AND the Jump button (Spacebar by default) is pressed:
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Set vertical velocity to jumpForce (launch upward).
            // Horizontal velocity stays the same.
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, jumpForce);
        }
        SetAnimation(moveInput);    // Call animation logic based on movement and jump state
    }

    // Decide which animation to play based on movement and grounded state
    private void SetAnimation(float moveInput)
    {
        if (isGrounded)                         // On the ground
        {
         if (moveInput == 0)                    // Not Moving
            {
                animator.Play("Player_Idle");   // Play idle animation
            }
         else                                   // Not moving
            {
                animator.Play("Player_Run");    // Play run animation
            }
        }
        else                                    // In the air
        {
            if (rigidbody.linearVelocityY > 0)  // Going upward
            {
                animator.Play("Player_Jump");   // Play jump animation
            }
            else                                // Going downward
            {
                animator.Play("Player_Fall");   // Play fall animation
            }
        }
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    private void FixedUpdate()
    {
        // --- Ground check ---
        // Create an invisible circle at the GroundCheck position.
        // If this circle overlaps any collider on the "Ground" layer, player is grounded.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
