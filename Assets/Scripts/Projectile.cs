using UnityEngine;

// Forces Unity to add physics to this object if it doesn't have it yet.
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Flight Settings")]
    [Tooltip("How fast the projectile flies towards the player.")]
    public float speed = 7f;
    
    [Tooltip("How many seconds until the projectile magically deletes itself to save memory.")]
    public float lifetime = 4f;

    // Internal physics component
    private Rigidbody2D rb;

    // Awake runs the exact millisecond the object is created, even before Start().
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Turn off gravity so the projectile doesn't drop like a rock. 
        // We want it to fly strictly based on our math.
        rb.gravityScale = 0f; 
        rb.isKinematic = true; 
    }

    void Start()
    {
        // 1. Search the entire game for the script called "PlayerController" to find our Rat.
        PlayerController rat = FindAnyObjectByType<PlayerController>();
        
        // If the Rat exists (and hasn't been destroyed)
        if (rat != null)
        {
            // 2. Calculate the exact direction to fly.
            // We subtract our position from the Rat's position to get the angle.
            // .normalized takes that angle and makes it exactly a length of 1, so the math is clean.
            Vector2 aimDirection = (rat.transform.position - transform.position).normalized;
            
            // 3. Push the projectile in that direction multiplied by our chosen speed.
            // linearVelocity controls the actual physical movement.
            rb.linearVelocity = aimDirection * speed;
        }
        else
        {
            // Failsafe: If the Rat is dead/missing, just fall straight down so the script doesn't break.
            rb.linearVelocity = new Vector2(0f, -speed);
        }
        
        // Tell Unity to automatically delete this specific projectile after a few seconds.
        // This prevents the game from lagging if thousands of projectiles miss and fly off the screen.
        Destroy(gameObject, lifetime);
    }

    // This triggers when the projectile's Trigger Collider touches another collider.
    // (Note: Damage to the player is handled by the Player's script detecting the "Enemy" tag, not here).
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the thing we hit is on the "Ground" layer.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // If it hits the floor, destroy the projectile so it doesn't dig through the dirt.
            Destroy(gameObject);
        }
    }
}