using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject projectilePrefab;
    [Tooltip("How many seconds between each shot")]
    public float fireRate = 2f;
    
    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0f)
        {
            ShootFromTop();
            timer = fireRate;
        }
    }

    private void ShootFromTop()
    {
        if (projectilePrefab == null) return;

        // X = Random horizontal position across the width of the screen (10% to 90%)
        // Y = 1.1 means it spawns just slightly above the top edge of the camera so it doesn't "pop" into view
        float randomWidth = Random.Range(0.1f, 0.9f);
        
        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(randomWidth, 1.1f, Camera.main.nearClipPlane));
        spawnPosition.z = 0f; // Keep it perfectly 2D

        Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
    }
}