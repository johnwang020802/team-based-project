using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    [Tooltip("How fast the background moves when the player is running.")]
    public float playerScrollSpeed = 0.05f; 
    [Tooltip("How fast the background drifts on its own when the player is standing still.")]
    public float idleScrollSpeed = 0.01f; 
    
    [Header("References")]
    public Transform cameraTransform;
    public Transform playerTransform; 

    private Material mat;
    private float currentOffset;
    private float lastPlayerPosX;

    void Start()
    {
        mat = GetComponent<Renderer>().material;

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Record the player's starting position so the math doesn't jump on frame 1
        if (playerTransform != null)
        {
            lastPlayerPosX = playerTransform.position.x;
        }
    }

    void LateUpdate()
    {
        if (playerTransform != null && cameraTransform != null)
        {
            // 1. Lock the physical Quad to the camera so it stays on screen
            transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);

            // 2. Calculate exactly how far the player moved this specific frame
            float playerMovement = playerTransform.position.x - lastPlayerPosX;

            // 3. Add the automatic idle speed PLUS the player's movement speed
            currentOffset += (idleScrollSpeed * Time.deltaTime) + (playerMovement * playerScrollSpeed);

            // 4. Apply the final combined offset to the material
            mat.mainTextureOffset = new Vector2(currentOffset, 0);

            // 5. Save the new position to compare against in the next frame
            lastPlayerPosX = playerTransform.position.x;
        }
    }
}