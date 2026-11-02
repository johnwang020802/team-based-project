using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CatWander : MonoBehaviour
{
    [Header("Chase Settings")]
    public Transform ratTarget;
    public float chaseRange = 5f;    
    public float chaseSpeed = 4f;    
    
    [Header("Sprint Settings")]
    public float sprintRange = 2f;   
    public float sprintSpeed = 7f;   

    [Header("Wander Settings")]
    public float walkSpeed = 2f;
    public float minWalkTime = 1f;
    public float maxWalkTime = 3f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 3f;

    private Rigidbody2D rb;
    public Animator animator;
    
    private int moveDirection = 0;
    private bool isWalking = false;
    private string currentState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>(); 
        rb.freezeRotation = true;

    }

    void Start()
    {
        if (ratTarget == null)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null) ratTarget = player.transform;
        }

        StartCoroutine(WanderRoutine());
    }

    void FixedUpdate()
    {
        float currentSpeed = walkSpeed;

        if (ratTarget != null)
        {
            float distanceToRat = Vector2.Distance(transform.position, ratTarget.position);

            if (distanceToRat <= sprintRange)
            {
                currentSpeed = sprintSpeed;
                moveDirection = ratTarget.position.x > transform.position.x ? 1 : -1;
            }
            else if (distanceToRat <= chaseRange)
            {
                currentSpeed = chaseSpeed;
                moveDirection = ratTarget.position.x > transform.position.x ? 1 : -1;
            }
        }

        rb.linearVelocity = new Vector2(moveDirection * currentSpeed, rb.linearVelocity.y);

        if (moveDirection > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); 
            ChangeAnimation("walk");
        }
        else if (moveDirection < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); 
            ChangeAnimation("walk");
        }
        else
        {
            ChangeAnimation("idle");
        }
    }

    IEnumerator WanderRoutine()
    {
        while (true) 
        {
            if (ratTarget == null || Vector2.Distance(transform.position, ratTarget.position) > chaseRange)
            {
                isWalking = !isWalking;

                if (isWalking)
                {
                    // --- NEW CAMERA CONSTRAINT LOGIC ---
                    // Convert the cat's position to a screen percentage (0.0 to 1.0)
                    Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
                    
                    if (screenPos.x < 0.1f) 
                    {
                        // If cat is too close to the LEFT edge of the screen, force walk RIGHT
                        moveDirection = 1; 
                    }
                    else if (screenPos.x > 0.9f) 
                    {
                        // If cat is too close to the RIGHT edge of the screen, force walk LEFT
                        moveDirection = -1; 
                    }
                    else
                    {
                        // Cat is safely in the middle of the screen, pick a random direction
                        moveDirection = Random.Range(0, 2) == 0 ? -1 : 1;
                    }
                    // -----------------------------------

                    yield return new WaitForSeconds(Random.Range(minWalkTime, maxWalkTime));
                }
                else
                {
                    moveDirection = 0;
                    yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
                }
            }
            else
            {
                // Pause wander decisions while chasing
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void ChangeAnimation(string newState)
    {
        if (animator == null || currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}