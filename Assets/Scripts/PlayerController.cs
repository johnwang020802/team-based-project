using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; 
    public float jumpForce = 12f; 
    
    [Tooltip("If you let go of Space early, multiply the upward speed by this decimal to cut the jump short.")]
    public float jumpCutMultiplier = 0.5f; // --- NEW: Variable Jump Math ---

    [Header("Ground Detection")]
    public Transform groundCheck; 
    public float groundCheckRadius = 0.2f; 
    public LayerMask groundLayer; 

    [Header("Damage & Knockback")]
    public int enemyDamage = 1; 
    public float knockbackForceX = 10f; 
    public float knockbackForceY = 5f;  
    public float hurtDuration = 1f; 

    // Internal physics and animation components
    private Rigidbody2D rb;
    private PlayerHealth healthScript;
    private Animator animator; 
    
    // State trackers
    private float horizontalInput;
    private bool isGrounded;
    private bool isHurt; 
    private float hurtTimer; 
    private string currentState; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthScript = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>(); 
        
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isHurt)
        {
            ChangeAnimation("hurt"); 
            
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0f) isHurt = false; 
            return; 
        }

        // 1. Read Horizontal Input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Check for Starting a Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 3. --- NEW: Check for Letting Go of Jump Early ---
        // If the player releases the jump button AND the Rat is currently moving upwards...
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            // Instantly slash the upward speed in half. 
            // This stops the Rat from floating all the way to the top of the jump arc.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        // 4. Flip Graphic
        if (horizontalInput > 0) transform.localScale = new Vector3(1f, 1f, 1f);
        else if (horizontalInput < 0) transform.localScale = new Vector3(-1f, 1f, 1f);

        // 5. Update Animations
        if (!isGrounded)
        {
            ChangeAnimation("jump");
        }
        else if (horizontalInput != 0)
        {
            ChangeAnimation("run");
        }
        else
        {
            ChangeAnimation("idle");
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isHurt)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHurt) return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("BadPizza"))
        {
            TakeDamage(collision.transform);

            if (collision.CompareTag("BadPizza")) Destroy(collision.gameObject);
        }
    }

    private void TakeDamage(Transform enemyTransform)
    {
        isHurt = true;
        hurtTimer = hurtDuration;

        float bounceDirection = enemyTransform.position.x > transform.position.x ? -1f : 1f;

        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(knockbackForceX * bounceDirection, knockbackForceY);

        if (healthScript != null) healthScript.TakeDamage(enemyDamage);
    }

    private void ChangeAnimation(string newState)
    {
        if (animator == null || currentState == newState) return;
        
        animator.Play(newState); 
        currentState = newState; 
    }
}