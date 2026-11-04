using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The target the camera should follow
    
    [Header("Camera Settings")]
    public float cameraHeight = 0f; // Allows you to aim the camera slightly above or below the player
    public float smoothSpeed = 5f;  // A higher number means the camera catches up to the player faster

    // LateUpdate ensures the camera moves only AFTER the player has finished taking their steps for this frame
    void LateUpdate()
    {
        // Make sure the player still exists (hasn't been destroyed/killed) before trying to follow them
        if (player != null)
        {
            // Create a target destination. 
            // We copy the player's horizontal (X) position, use our custom height (Y), 
            // and lock the depth (Z) to -10, which is standard for 2D cameras to see the world.
            Vector3 targetPosition = new Vector3(player.position.x, cameraHeight, -10f);
            
            // Vector3.Lerp smoothly glides an object from Point A to Point B over time.
            // It prevents the camera from feeling robotic and rigid.
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}