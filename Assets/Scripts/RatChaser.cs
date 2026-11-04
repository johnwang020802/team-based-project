using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyRat : MonoBehaviour
{
    [Header("Chase Settings")]
    public float moveSpeed = 4f;
    [Tooltip("How fast the rival rat sprints offscreen to the right after attacking")]
    public float runAwaySpeed = 8f; 

    private Rigidbody2D rb;
    private Transform playerTransform;
    private bool isFacingRight = true;
    
    private bool hasHitPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; 

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        // If it already attacked, ALWAYS sprint to the right
        if (hasHitPlayer)
        {
            // runAwaySpeed is always positive, forcing it to move right
            rb.linearVelocity = new Vector2(runAwaySpeed, rb.linearVelocity.y);
            return; 
        }

        // Normal tracking logic (before attacking)
        float direction = 0f;
        if (playerTransform.position.x > transform.position.x) direction = 1f;
        else if (playerTransform.position.x < transform.position.x) direction = -1f;

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (direction > 0 && !isFacingRight) FlipDirection();
        else if (direction < 0 && isFacingRight) FlipDirection();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            hasHitPlayer = true;
            
            gameObject.tag = "Untagged";
            
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());

            // Force the enemy rat to instantly face right if it was looking left when it hit you
            if (!isFacingRight)
            {
                FlipDirection();
            }
        }
    }

    private void FlipDirection()
    {
        isFacingRight = !isFacingRight;
        if (isFacingRight) transform.localScale = new Vector3(1f, 1f, 1f);
        else transform.localScale = new Vector3(-1f, 1f, 1f);
    }
}