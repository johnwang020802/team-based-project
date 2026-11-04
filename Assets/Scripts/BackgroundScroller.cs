using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // [Header] creates a bold label in the Unity Inspector to keep things organized.
    // [Tooltip] adds a little pop-up explanation when you hover your mouse over the variable in Unity.
    [Header("Scroll Settings")]
    [Tooltip("How fast the background moves when the player is running.")]
    public float playerScrollSpeed = 0.05f; 
    
    [Tooltip("How fast the background drifts on its own when the player is standing still.")]
    public float idleScrollSpeed = 0.01f; 
    
    [Header("References")]
    public Transform cameraTransform; // The camera we want the background to follow
    public Transform playerTransform; // The player character we are tracking

    // Private variables are hidden from the Inspector and used only for internal math
    private Material mat; // The material (image) attached to this object
    private float currentOffset; // How far the image has been shifted
    private float lastPlayerPosX; // The player's exact X position during the previous frame

    void Start()
    {
        // Grab the Material attached to this object's Renderer so we can slide its texture later
        mat = GetComponent<Renderer>().material;

        // Failsafe: If we forgot to assign the camera in the Inspector, find the Main Camera automatically
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Failsafe: Record where the player is standing exactly when the game starts. 
        // This prevents the background from violently jerking on the very first frame.
        if (playerTransform != null)
        {
            lastPlayerPosX = playerTransform.position.x;
        }
    }

    // LateUpdate runs AFTER all other game logic (like player movement) has finished.
    // We use it for cameras and backgrounds to prevent stuttering.
    void LateUpdate()
    {
        // Only run this math if we successfully found both the player and the camera
        if (playerTransform != null && cameraTransform != null)
        {
            // 1. Lock this background object to the camera's X and Y position so it never leaves the screen.
            // We keep its original Z position so it stays in the background.
            transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);

            // 2. Figure out exactly how far (and in what direction) the player moved during this exact frame.
            float playerMovement = playerTransform.position.x - lastPlayerPosX;

            // 3. Time.deltaTime makes the idle scrolling frame-rate independent (smooth on both fast and slow computers).
            // We add the automatic idle drift, and add the player's movement multiplied by our chosen speed.
            currentOffset += (idleScrollSpeed * Time.deltaTime) + (playerMovement * playerScrollSpeed);

            // 4. Slide the actual picture (texture) across the material by our newly calculated amount.
            mat.mainTextureOffset = new Vector2(currentOffset, 0);

            // 5. Save the player's current position. 
            // In the next frame, this becomes the "old" position we compare against.
            lastPlayerPosX = playerTransform.position.x;
        }
    }
}