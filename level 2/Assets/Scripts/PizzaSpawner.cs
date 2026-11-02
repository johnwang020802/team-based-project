using UnityEngine;

public class PizzaSpawner : MonoBehaviour
{
    [Header("Pizza Prefabs")]
    public GameObject goodPizzaPrefab;
    public GameObject badPizzaPrefab;

    [Header("Pre-Spawning")]
    [Tooltip("How many pizzas to spawn immediately when the level starts")]
    public int initialSpawnAmount = 5;

    [Header("Spawn Timing")]
    public float timeBetweenSpawns = 2f;
    private float spawnTimer;

    [Header("Spawn Probabilities")]
    [Range(0f, 1f)]
    public float badPizzaChance = 0.3f; 

    [Header("Spawn Area Boundaries")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Player Reference (For First 5 Seconds)")]
    public Transform player; 
    public float minDistanceAhead = 2f;
    public float maxDistanceAhead = 8f;

    [Header("Overlap Prevention")]
    public float minPizzaDistance = 2f; 
    public int maxSpawnAttempts = 10;   

    [Header("Spawn Tracking")]
    public int totalPizzasSpawned = 0;

    private void Start()
    {
        // Instantly generates the starting batch of pizzas as soon as the level loads
        for (int i = 0; i < initialSpawnAmount; i++)
        {
            PreSpawnPizza();
        }
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnPizza();
            spawnTimer = timeBetweenSpawns;
        }
    }

    private void PreSpawnPizza()
    {
        Vector2 spawnPosition = Vector2.zero;
        bool validPositionFound = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            // Forces pre-spawned pizzas to use the entire level boundaries, ignoring the player's position
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            spawnPosition = new Vector2(randomX, randomY);

            if (IsPositionValid(spawnPosition))
            {
                validPositionFound = true;
                break;
            }
        }

        if (validPositionFound)
        {
            GameObject pizzaToSpawn = Random.value <= badPizzaChance ? badPizzaPrefab : goodPizzaPrefab;
            Instantiate(pizzaToSpawn, spawnPosition, Quaternion.identity);
            totalPizzasSpawned++;
        }
    }

    private void SpawnPizza()
    {
        Vector2 spawnPosition = Vector2.zero;
        bool validPositionFound = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float randomX;
            float randomY = Random.Range(minY, maxY);

            if ( player != null)
            {
                float facingDirection = player.localScale.x < 0 ? 1f : -1f; 
                float distance = Random.Range(minDistanceAhead, maxDistanceAhead);
                randomX = player.position.x + (distance * facingDirection);
            }
            else
            {
                randomX = Random.Range(minX, maxX);
            }

            spawnPosition = new Vector2(randomX, randomY);

            if (IsPositionValid(spawnPosition))
            {
                validPositionFound = true;
                break; 
            }
        }

        if (validPositionFound)
        {
            GameObject pizzaToSpawn = Random.value <= badPizzaChance ? badPizzaPrefab : goodPizzaPrefab;
            Instantiate(pizzaToSpawn, spawnPosition, Quaternion.identity);
            totalPizzasSpawned++;
        }
    }

    private bool IsPositionValid(Vector2 testPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(testPosition, minPizzaDistance);
        
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("GoodPizza") || col.CompareTag("BadPizza"))
            {
                return false; 
            }
        }
        
        return true; 
    }
}