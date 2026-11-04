using UnityEngine;

public class FeetEnemy : MonoBehaviour
{
    [Tooltip("The starting speed of the enemy.")]
    public float enemySpeed;
    
    [Tooltip("How many seconds before the enemy randomly changes its speed.")]
    public float timer = 4f;
    
    void Start()
    {
        // Initialization code would go here, but this enemy is ready to go immediately!
    }

    void Update()
    {
        // Translate forces an object to slide through space without using physical forces like gravity.
        // Vector2.left means it moves on the X axis to the left.
        // Multiplying by Time.deltaTime ensures the speed is smooth regardless of your computer's framerate.
        transform.Translate(Vector2.left * enemySpeed * Time.deltaTime);

        // Check if the timer has counted all the way down to zero
        if(timer <= 0)
        {
            // Pick a random whole number: -1, 0, or 1.
            // Note: Random.Range for integers includes the first number, but excludes the last number.
            int change = Random.Range(-1, 2);
            
            // Add that random number to the enemy's speed (making them slightly faster or slower)
            enemySpeed += change;
            
            // Reset the timer back to 4 seconds so it can start counting down again
            timer = 4f;
        }
        else
        {
            // If the timer isn't zero yet, subtract the time passed since the last frame
            timer -= Time.deltaTime;
        }
    }
}