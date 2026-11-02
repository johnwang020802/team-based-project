using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Flight Settings")]
    public float speed = 7f;
    public float lifetime = 4f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; 
        rb.isKinematic = true; 
    }

    void Start()
    {
        // 1. Find the Rat in the scene
        PlayerController rat = FindAnyObjectByType<PlayerController>();
        
        if (rat != null)
        {
            // 2. Calculate the exact direction from the spawn point to the Rat
            Vector2 aimDirection = (rat.transform.position - transform.position).normalized;
            
            // 3. Fire in that direction
            rb.linearVelocity = aimDirection * speed;
        }
        else
        {
            // Fallback just in case the Rat is destroyed
            rb.linearVelocity = new Vector2(speed, 0f);
        }
        
        // Clean up memory after a few seconds
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy if it hits the ground/walls
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}