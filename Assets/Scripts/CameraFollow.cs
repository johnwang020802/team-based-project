using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    
    [Header("Camera Settings")]
    public float cameraHeight = 0f; // Change this to aim the camera higher/lower
    public float smoothSpeed = 5f;  // Higher number = faster horizontal tracking

    // We use LateUpdate for cameras so it moves AFTER the player finishes moving
    void LateUpdate()
    {
        if (player != null)
        {
            // Track the player's X, but lock the Y to your chosen height. Z stays at -10 for 2D.
            Vector3 targetPosition = new Vector3(player.position.x, cameraHeight, -10f);
            
            // Lerp glides the camera smoothly instead of snapping it rigidly
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}