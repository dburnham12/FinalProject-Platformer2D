using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // [SerializeField] forces fields to appear in the Inspector, so you can tweak them without editing code.
    [SerializeField] private float moveSpeed = 4f;              // How fast the player moves left/right
    [SerializeField] private float jumpForce = 8f;              // How strong the jump is (vertical speed)
    [SerializeField] private float jumpBufferTime = 0.2f;       // how long to give forgiveness for jumping too early
    [SerializeField] private float jumpContinuesForce = 0.2f;   // determines the impulse strength for variableJump
    [SerializeField] private float maxJumpSpeed = 10f;          // clamp for variableJump
    [SerializeField] private int airJumps = 1;                  // How many air jumps the player can perform
    [SerializeField] private float jumpBoostDuration = 10f;     // duration of how long triple jump boost lasts for
    [SerializeField] private float speedBoostDuration = 10f;
    [SerializeField] private Transform groundCheck;             // Empty child object placed at the player's feet
    [SerializeField] private float groundCheckRadius = 0.2f;    // Size of the circle used to detect ground
    [SerializeField] private LayerMask groundLayer;             // Which layer counts as "ground" (set in Inspector)

    

    // Private fields are used internally by the script.
    // Components
    // new keyword is used to intentionally override Component.rigidbody, which is deprecated
    private new Rigidbody2D rigidbody;      // Reference to the Rigidbody2D component
    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component
    private Animator animator;              // Reference to Player's animator
    private PlayerHealth playerHealth;      // Reference to Player's health

    private float moveInput;
    private bool isGrounded;                // True if player is standing on ground
    private bool wasGrounded;               // added to fix triple jump bug
    private int airJumpCounter = 0;         // Counter for how many times the player has performed an air jump
    private int coins = 0;

    //Jump additions
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private Coroutine jumpBoostRoutine;

    //Speed additions
    private Coroutine speedBoostRoutine;

    // Public properties expose fields, and allow logic to be done on getters and setters
    public int Coins
    {
        get { return coins; }
        set
        {
            if (value > coins)
                coins = value;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        rigidbody = GetComponent<Rigidbody2D>();    // Grab the Rigidbody2D attached to the Player object once at the start.
        spriteRenderer = GetComponent<SpriteRenderer>(); // Grab the SpriteRenderer attached to the Player GameObject
        animator = GetComponent<Animator>();        // Grab the animator component on the player
        playerHealth = GetComponent<PlayerHealth>();    //Grab the health component
    }

    private void Update()
    {
        // --- Ground check ---
        // Create an invisible circle at the GroundCheck position.
        // If this circle overlaps any collider on the "Ground" layer, player is grounded.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //Landing detection
        if (!wasGrounded && isGrounded) //wasGrounded makes sure of landing condition
        {
            coyoteTimeCounter = coyoteTime; //reset coyote timer while grounded 
            airJumpCounter = 0; //reset airJump counter 
        }
        
        //Coyote Timer (airborne)
        if (!isGrounded)
        {
            coyoteTimeCounter -= Time.deltaTime;
            if (coyoteTimeCounter < 0f)
                coyoteTimeCounter = 0f;
        }


        //Jump buffer
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime;
        else
        {
            jumpBufferCounter -= Time.deltaTime;
            if (jumpBufferCounter < 0f) jumpBufferCounter = 0f;
        }

        //dynamic gravity scaling
        if (rigidbody.linearVelocityY < 0)
            rigidbody.gravityScale = 3f;
        else 
            rigidbody.gravityScale = 2f;
        
        // Get input from keyboard (A/D or Left/Right arrows).
        moveInput = Input.GetAxis("Horizontal");

        HorizontalMovement();
        Jump();
        SetAnimation();    // Call animation logic based on movement and jump state
        
        wasGrounded = isGrounded;   //set last frame to grounded
    }

    private void HorizontalMovement()
    {
        // Apply horizontal speed while keeping the current vertical velocity.
        rigidbody.linearVelocity = new Vector2(moveInput * moveSpeed, rigidbody.linearVelocity.y);

        if (moveInput != 0f && isGrounded)
            spriteRenderer.flipX = moveInput < 0f; // makes the player face the way their walking
    }

    private IEnumerator SpeedBoost(float duration)
    {
        moveSpeed = 8f;
        yield return new WaitForSeconds(duration);
        moveSpeed = 4f;
    }

    private void Jump()
    {
        bool canCoyoteJump = coyoteTimeCounter > 0f;
        bool canAirJump = airJumpCounter < airJumps;

        // If player is grounded AND the Jump button (Spacebar by default) is pressed:
        if (jumpBufferCounter > 0f && (canAirJump || canCoyoteJump))
        {
            // Set vertical velocity to jumpForce (launch upward).
            // Horizontal velocity stays the same.
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, jumpForce);
            spriteRenderer.flipX = moveInput < 0f; // makes the player face the way they are jumping

            SoundManager.Instance.PlaySFX("JUMP", 1f);

            if(canCoyoteJump)
                coyoteTimeCounter = 0f;
            else
                airJumpCounter++;

            jumpBufferCounter = 0f;
        }
    }

    private IEnumerator TripleJump(float duration)
    {
        airJumps = 2;
        yield return new WaitForSeconds(duration);
        airJumps = 1;
    }

    // Decide which animation to play based on movement and grounded state
    private void SetAnimation()
    {
        if (isGrounded) // On the ground
            animator.Play(moveInput == 0 ? "Player_Idle" : "Player_Run"); // Not moving => Play idle animation | Is moving => Play run animation
        else // In the air
            animator.Play(rigidbody.linearVelocityY > 0 ? "Player_Jump" : "Player_Fall"); // Going upward => Play jump animation | Going downward => Play fall animation
    }

    private void FixedUpdate()
    {
        //Variable Jump
        if (Input.GetKey(KeyCode.Space) && rigidbody.linearVelocity.y > 0)
        {
            // Apply upward impulse 
            rigidbody.AddForce(Vector2.up * jumpContinuesForce, ForceMode2D.Impulse);

            //clamp jump
            if (rigidbody.linearVelocity.y > maxJumpSpeed)
            {
                rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, maxJumpSpeed);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "BouncePad")
        {
            SoundManager.Instance.PlaySFX("BOING", 1f);
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, jumpForce * 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Trigger for healthUp
        if(collision.CompareTag("healthUp"))
        {   
            bool healed = playerHealth.healPlayer();
            if (healed)
            {
                SoundManager.Instance.PlaySFX("EAT", 1f);
                Destroy(collision.gameObject);
            }
        }

        //Trigger for jumpUp
        if(collision.CompareTag("jumpUp"))
        {
            SoundManager.Instance.PlaySFX("EAT", 1f);
            Destroy(collision.gameObject);

            if (jumpBoostRoutine != null)
                StopCoroutine(jumpBoostRoutine);
            jumpBoostRoutine = StartCoroutine(TripleJump(jumpBoostDuration));
        }

        //Trigger for speedUp
        if(collision.CompareTag("speedUp"))
        {
            SoundManager.Instance.PlaySFX("EAT", 1f);
            Destroy(collision.gameObject);

            if (speedBoostRoutine != null)
                StopCoroutine(speedBoostRoutine);
            speedBoostRoutine = StartCoroutine(SpeedBoost(speedBoostDuration));
        }
    }
}
