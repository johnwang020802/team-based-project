using System.Collections;
using UnityEngine;

// This forces Unity to add a Rigidbody2D to the object if it doesn't already have one, preventing errors.
[RequireComponent(typeof(Rigidbody2D))]
public class CatWander : MonoBehaviour
{
    [Header("Chase Settings")]
    public Transform ratTarget; // The player character to chase
    public float chaseRange = 5f; // How close the player must be to trigger a chase
    public float chaseSpeed = 4f; // How fast the cat runs during a normal chase
    
    [Header("Sprint Settings")]
    public float sprintRange = 2f; // How close the player must be to trigger a fast sprint
    public float sprintSpeed = 7f; // The cat's top speed when the player is very close

    [Header("Wander Settings")]
    public float walkSpeed = 2f; // Normal casual walking speed
    public float minWalkTime = 1f; // Minimum time the cat will walk in one direction
    public float maxWalkTime = 3f; // Maximum time the cat will walk in one direction
    public float minIdleTime = 1f; // Minimum time the cat will stand completely still
    public float maxIdleTime = 3f; // Maximum time the cat will stand completely still

    // Internal components used for physics and animation
    private Rigidbody2D rb;
    public Animator animator;
    
    // State trackers
    private int moveDirection = 0; // -1 is left, 1 is right, 0 is standing still
    private bool isWalking = false; // Is the cat currently in a walking phase?
    private string currentState; // Remembers the current animation so we don't restart it constantly

    void Awake()
    {
        // Grab the necessary physics and animation components attached to this object
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>(); 
        
        // Prevent the cat from physically tumbling over and rolling like a ball
        rb.freezeRotation = true;
    }

    void Start()
    {
        // If we forgot to assign the target in the Inspector, automatically search the game for the player
        if (ratTarget == null)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null) ratTarget = player.transform;
        }

        // Start the background timer loop that handles random wandering
        StartCoroutine(WanderRoutine());
    }

    // FixedUpdate handles all physics and movement
    void FixedUpdate()
    {
        float currentSpeed = walkSpeed; // Default to casual walking speed

        // Check if we have a valid target to chase
        if (ratTarget != null)
        {
            // Measure the exact distance between the Cat and the Rat
            float distanceToRat = Vector2.Distance(transform.position, ratTarget.position);

            // If the Rat is extremely close, activate Sprint mode
            if (distanceToRat <= sprintRange)
            {
                currentSpeed = sprintSpeed;
                // Move right (1) if target is to our right, otherwise move left (-1)
                moveDirection = ratTarget.position.x > transform.position.x ? 1 : -1;
            }
            // Otherwise, if the Rat is somewhat close, activate normal Chase mode
            else if (distanceToRat <= chaseRange)
            {
                currentSpeed = chaseSpeed;
                moveDirection = ratTarget.position.x > transform.position.x ? 1 : -1;
            }
        }

        // Apply movement physics. We multiply direction by speed, and leave the vertical (Y) speed alone so gravity still works.
        rb.linearVelocity = new Vector2(moveDirection * currentSpeed, rb.linearVelocity.y);

        // Handle Animations and Sprite Flipping
        if (moveDirection > 0) // Moving Right
        {
            // Flip the sprite graphic to face right
            transform.localScale = new Vector3(-1f, 1f, 1f); 
            ChangeAnimation("walk");
        }
        else if (moveDirection < 0) // Moving Left
        {
            // Keep the sprite graphic facing its normal left direction
            transform.localScale = new Vector3(1f, 1f, 1f); 
            ChangeAnimation("walk");
        }
        else // Standing still (moveDirection is 0)
        {
            ChangeAnimation("idle");
        }
    }

    // A Coroutine is a function that can pause itself and wait over time (like a background timer)
    IEnumerator WanderRoutine()
    {
        // This loop runs endlessly for the entire game
        while (true) 
        {
            // Only wander if there is no target, OR if the target is too far away to chase
            if (ratTarget == null || Vector2.Distance(transform.position, ratTarget.position) > chaseRange)
            {
                // Toggle between walking and standing still
                isWalking = !isWalking;

                if (isWalking)
                {
                    // --- CAMERA CONSTRAINT LOGIC ---
                    // This finds where the cat is on the player's screen. 
                    // 0.0 is the far left edge of the screen, 1.0 is the far right edge.
                    Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
                    
                    if (screenPos.x < 0.1f) 
                    {
                        // Cat is too close to the left edge; force it to walk Right to stay on screen
                        moveDirection = 1; 
                    }
                    else if (screenPos.x > 0.9f) 
                    {
                        // Cat is too close to the right edge; force it to walk Left
                        moveDirection = -1; 
                    }
                    else
                    {
                        // Cat is safely in the middle; pick a random direction (either -1 or 1)
                        moveDirection = Random.Range(0, 2) == 0 ? -1 : 1;
                    }

                    // Pause this background loop for a random amount of time while the cat walks
                    yield return new WaitForSeconds(Random.Range(minWalkTime, maxWalkTime));
                }
                else
                {
                    // Stop moving entirely and wait in the idle state
                    moveDirection = 0;
                    yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
                }
            }
            else
            {
                // If the cat is currently chasing the player, pause this wander timer for a split second and check again
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    // A helper function to safely change animations without restarting the clip every frame
    private void ChangeAnimation(string newState)
    {
        // If there's no animator, or if we are already playing this exact animation, do nothing
        if (animator == null || currentState == newState) return;
        
        animator.Play(newState); // Play the new animation
        currentState = newState; // Remember what we are currently playing
    }
}