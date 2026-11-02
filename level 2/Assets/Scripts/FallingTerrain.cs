using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingTerrain : MonoBehaviour
{
    [Header("Timer Settings")]
    public float dropDelay = 3f;

    [Header("Damage Setup")]
    [Tooltip("Drag the child DamageZone object here so it gets deleted when landing")]
    public GameObject damageHitbox;

    [Header("Environment Movement Sync")]
    public float scrollSpeed = -3f;

    private Rigidbody2D rb;
    private bool hasDropped = false;
    private bool hasLanded = false;
    
    private float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        timer = dropDelay;
    }

    void Update()
    {
        if (!hasDropped)
        {
            timer -= Time.deltaTime; 
            if (timer <= 0f) TriggerDrop();
        }
    }

    void FixedUpdate()
    {
        if (!hasLanded)
        {
            if (!hasDropped) rb.linearVelocity = new Vector2(scrollSpeed, 0f);
            else rb.linearVelocity = new Vector2(scrollSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(scrollSpeed, 0f);
        }
    }

    private void TriggerDrop()
    {
        hasDropped = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasDropped && !hasLanded)
        {
            hasLanded = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = new Vector2(scrollSpeed, 0f);
            
            // Becomes jumpable ground
            gameObject.layer = LayerMask.NameToLayer("Ground"); 

            // Deletes the damage sensor so it is safe to stand on!
            if (damageHitbox != null) Destroy(damageHitbox);
        }
    }
}