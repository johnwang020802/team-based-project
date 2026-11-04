using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingTerrain : MonoBehaviour
{
    [Header("Timer Settings")]
    public float dropDelay = 3f; // How many seconds the rock hovers before falling

    [Header("Damage Setup")]
    [Tooltip("Drag the child DamageZone object here so it gets deleted when landing")]
    public GameObject damageHitbox; // The trigger collider that hurts the player

    [Header("Environment Movement Sync")]
    public float scrollSpeed = -3f; // Forces the rock to move left at the same speed as the rest of the level

    private Rigidbody2D rb;
    private bool hasDropped = false; // Tracks if the rock has started falling
    private bool hasLanded = false; // Tracks if the rock has smashed into the floor
    
    private float timer; // Internal countdown clock

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Prevents the block from spinning weirdly in the air
    }

    void Start()
    {
        // Kinematic means physics won't affect it yet. Gravity is turned off.
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        // Start the countdown clock
        timer = dropDelay;
    }

    void Update()
    {
        // If it hasn't fallen yet, keep ticking down the timer
        if (!hasDropped)
        {
            // Time.deltaTime equals the time passed since the last frame (usually 0.016 seconds)
            timer -= Time.deltaTime; 
            
            // Once the timer hits 0, trigger the drop
            if (timer <= 0f) TriggerDrop();
        }
    }

    void FixedUpdate()
    {
        // Move the rock horizontally so it matches the speed of your scrolling level
        if (!hasLanded)
        {
            if (!hasDropped) 
                rb.linearVelocity = new Vector2(scrollSpeed, 0f); // Hovering (no vertical movement)
            else 
                rb.linearVelocity = new Vector2(scrollSpeed, rb.linearVelocity.y); // Falling (keep existing vertical movement)
        }
        else
        {
            // It hit the ground, so it only moves left now
            rb.linearVelocity = new Vector2(scrollSpeed, 0f);
        }
    }

    private void TriggerDrop()
    {
        hasDropped = true;
        // Changing it to Dynamic turns gravity on, causing it to plummet down
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // This detects when the solid physics body of the rock slams into something solid (like the floor)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ensure it only activates once it has actually been dropped and hasn't already landed
        if (hasDropped && !hasLanded)
        {
            hasLanded = true;
            
            // Turn off gravity again so it stops trying to push through the floor
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = new Vector2(scrollSpeed, 0f);
            
            // Change the object's layer to "Ground". 
            // This is essential so the player's jump script recognizes it as safe to jump off of.
            gameObject.layer = LayerMask.NameToLayer("Ground"); 

            // Destroy the attached damage sensor box. 
            // This makes the rock permanently safe for the player to stand on without getting hurt!
            if (damageHitbox != null) Destroy(damageHitbox);
        }
    }
}