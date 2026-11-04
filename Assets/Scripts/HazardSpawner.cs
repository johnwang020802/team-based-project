using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [Tooltip("Drag the Projectile Prefab here so the script knows what to create.")]
    public GameObject projectilePrefab;
    
    [Tooltip("How many seconds the spawner waits before dropping the next item.")]
    public float fireRate = 2f;
    
    // An internal countdown clock used to track the time between shots.
    private float timer;

    void Update()
    {
        // Time.deltaTime is the time passed since the last frame. 
        // We subtract it from our timer to count down exactly in real-world seconds.
        timer -= Time.deltaTime;
        
        // Once the timer hits 0 (or drops slightly below), it's time to fire!
        if (timer <= 0f)
        {
            ShootFromTop();
            
            // Reset the countdown clock back to our chosen fireRate (e.g., 2 seconds)
            timer = fireRate;
        }
    }

    private void ShootFromTop()
    {
        // Failsafe: If we forgot to assign a prefab in the Inspector, stop running this code so Unity doesn't crash.
        if (projectilePrefab == null) return;

        // 1. Pick a random horizontal position. 
        // 0.0 is the far left of the camera screen, 1.0 is the far right. 
        // We pick between 0.1 and 0.9 so it doesn't spawn exactly on the invisible edge.
        float randomWidth = Random.Range(0.1f, 0.9f);
        
        // 2. Convert that screen percentage into a real 3D world position.
        // Y = 1.1 means we spawn it slightly ABOVE the camera's view so the player doesn't see it magically pop into existence.
        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(randomWidth, 1.1f, Camera.main.nearClipPlane));
        
        // Lock the Z axis to 0 so it stays perfectly flat in our 2D game.
        spawnPosition.z = 0f; 

        // 3. Instantiate is Unity's command to create a brand new object. 
        // Quaternion.identity just means "use the default rotation without spinning it."
        Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
    }
}