using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // [SerializeField] forces fields to appear in the Inspector, so you can tweak them without editing code.
    [SerializeField] private float moveSpeed = 4f;              // How fast the player moves left/right
    [SerializeField] private float jumpForce = 8f;              // How strong the jump is (vertical speed)
    [SerializeField] private int airJumps = 1;                  // How many air jumps the player can perform
    [SerializeField] private Transform groundCheck;             // Empty child object placed at the player's feet
    [SerializeField] private float groundCheckRadius = 0.2f;    // Size of the circle used to detect ground
    [SerializeField] private LayerMask groundLayer;             // Which layer counts as "ground" (set in Inspector)

    // Private fields are used internally by the script.
    // Components
    // new keyword is used to intentionally override Component.rigidbody, which is deprecated
    private new Rigidbody2D rigidbody;      // Reference to the Rigidbody2D component
    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component
    private Animator animator;              // Reference to Player's animator

    private bool isGrounded;                // True if player is standing on ground
    private int airJumpCounter = 0;         // Counter for how many times the player has performed an air jump

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();    // Grab the Rigidbody2D attached to the Player object once at the start.
        spriteRenderer = GetComponent<SpriteRenderer>(); // Grab the SpriteRenderer attached to the Player GameObject
        animator = GetComponent<Animator>();        // Grab the animator component on the player
    }

    private void Update()
    {
        // --- Horizontal movement ---
        // Get input from keyboard (A/D or Left/Right arrows).
        float moveInput = Input.GetAxis("Horizontal");
        // Apply horizontal speed while keeping the current vertical velocity.
        rigidbody.linearVelocity = new Vector2(moveInput * moveSpeed, rigidbody.linearVelocity.y);

        if (moveInput != 0f && isGrounded)
            spriteRenderer.flipX = moveInput < 0f; // makes the player face the way their walking

        // --- Jump ---
        if (airJumpCounter > 0 && isGrounded)
            airJumpCounter = 0;

        // If player is grounded AND the Jump button (Spacebar by default) is pressed:
        if (Input.GetKeyDown(KeyCode.Space) && airJumpCounter < airJumps)
        {
            // Set vertical velocity to jumpForce (launch upward).
            // Horizontal velocity stays the same.
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, jumpForce);
            spriteRenderer.flipX = moveInput < 0f; // makes the player face the way they are jumping

            airJumpCounter++;
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

    private void FixedUpdate()
    {
        // --- Ground check ---
        // Create an invisible circle at the GroundCheck position.
        // If this circle overlaps any collider on the "Ground" layer, player is grounded.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
