using UnityEngine;

// Forces Unity to automatically add a Rigidbody2D to the Rat so gravity works
[RequireComponent(typeof(Rigidbody2D))] 
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // How fast the Rat runs left and right
    public float jumpForce = 12f; // How high the Rat jumps

    [Header("Ground Detection")]
    [Tooltip("An empty child object placed exactly at the Rat's feet.")]
    public Transform groundCheck; 
    public float groundCheckRadius = 0.2f; // How big of a circle to draw at the feet to check for floor
    public LayerMask groundLayer; // Tells the script which layers count as solid ground

    [Header("Damage & Knockback")]
    [Tooltip("How much health is subtracted when touching a Cat or a Bad Pizza.")]
    public int enemyDamage = 1; 
    public float knockbackForceX = 10f; // How hard the Rat is thrown horizontally when hit
    public float knockbackForceY = 5f;  // How hard the Rat bounces up into the air when hit
    [Tooltip("How many seconds the Rat is invincible after getting hurt.")]
    public float hurtDuration = 1f; 

    // Internal physics and component trackers
    private Rigidbody2D rb;
    private PlayerHealth healthScript;
    
    // State trackers
    private float horizontalInput;
    private bool isGrounded;
    private bool isHurt; // True when the Rat is knocked back and loses control
    private float hurtTimer; // Counts down our invincibility frames

    void Awake()
    {
        // Grab the physics body and health script attached to the Rat
        rb = GetComponent<Rigidbody2D>();
        healthScript = GetComponent<PlayerHealth>();
        
        // Prevent the Rat from physically tumbling over on its face
        rb.freezeRotation = true;
    }

    void Update()
    {
        // If the Rat is currently knocked back, tick down the timer and ignore all player button presses
        if (isHurt)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0f)
            {
                isHurt = false; // The Rat regains control!
            }
            return; // Stop reading the rest of the Update loop early
        }

        // 1. Read Keyboard/Controller Input
        // Input.GetAxisRaw returns -1 if pressing Left/A, 1 if pressing Right/D, and 0 if pressing nothing
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Check Jumping
        // GetButtonDown checks if the jump button (usually Spacebar) was pressed this exact frame
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Apply an instant burst of upward force
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 3. Flip the Character Graphic
        // If moving right, face right. If moving left, face left.
        if (horizontalInput > 0) transform.localScale = new Vector3(1f, 1f, 1f);
        else if (horizontalInput < 0) transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    void FixedUpdate()
    {
        // FixedUpdate is used for physics. 
        
        // Draw an invisible circle at the Rat's feet. If it touches anything on the "Ground" layer, isGrounded becomes true.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Apply our horizontal movement, but only if we aren't currently flying backward from taking damage
        if (!isHurt)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    // This triggers automatically when the Rat's physical body overlaps a "Trigger" collider (like the Cat's damage aura)
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore the collision if we are currently invincible from a previous hit
        if (isHurt) return;

        // Check if the object we touched has the tag "Enemy" (Cat) OR "BadPizza" (Projectile)
        if (collision.CompareTag("Enemy") || collision.CompareTag("BadPizza"))
        {
            TakeDamage(collision.transform);

            // If it was a projectile, destroy the projectile so it doesn't pass through us and hit us twice
            if (collision.CompareTag("BadPizza"))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void TakeDamage(Transform enemyTransform)
    {
        // Lock the player's controls and start the invincibility timer
        isHurt = true;
        hurtTimer = hurtDuration;

        // Figure out if the enemy hit us from the left or the right
        // If enemy X is greater than Rat X, they are to our right, so we must bounce to the left (-1)
        float bounceDirection = enemyTransform.position.x > transform.position.x ? -1f : 1f;

        // Erase any current momentum, and instantly throw the Rat backward and upward
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(knockbackForceX * bounceDirection, knockbackForceY);

        // Tell the PlayerHealth script to do the math and update the UI pictures
        if (healthScript != null)
        {
            healthScript.TakeDamage(enemyDamage);
        }
    }
}