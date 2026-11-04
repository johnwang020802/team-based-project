using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CatChaser : MonoBehaviour
{
    [Header("Chase Settings")]
    [Tooltip("How fast the cat runs towards the Rat.")]
    public float moveSpeed = 4f;

    private Rigidbody2D rb;
    private Transform playerTransform;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Keep the cat from tipping over

        // Automatically find the Rat in the scene using the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void FixedUpdate()
    {
        // If the Rat died, or we forgot to tag it, just stand still
        if (playerTransform == null)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        // Figure out if the Rat is to the left or the right of the cat
        float direction = 0f;
        
        if (playerTransform.position.x > transform.position.x)
        {
            direction = 1f; // Rat is to the right
        }
        else if (playerTransform.position.x < transform.position.x)
        {
            direction = -1f; // Rat is to the left
        }

        // Move the cat towards the Rat
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // Flip the cat's picture to face the correct direction
        if (direction > 0 && !isFacingRight)
        {
            FlipDirection();
        }
        else if (direction < 0 && isFacingRight)
        {
            FlipDirection();
        }
    }

    // Triggers when the cat's solid body physically bumps into something
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // The Rat's script handles the damage math. We just need to delete the cat!
            Destroy(gameObject);
        }
    }

    // Triggers if the cat's collider is set to "Is Trigger" instead of solid
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void FlipDirection()
    {
        isFacingRight = !isFacingRight;
        
        if (isFacingRight) transform.localScale = new Vector3(1f, 1f, 1f);
        else transform.localScale = new Vector3(-1f, 1f, 1f);
    }
}