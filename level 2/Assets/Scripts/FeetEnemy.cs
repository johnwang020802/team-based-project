using UnityEngine;

public class FeetEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float enemySpeed;
    public float timer = 4f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * enemySpeed * Time.deltaTime);
        //makes the enemy move left at a constant speed

        if(timer <= 0)
        {
            int change = Random.Range(-1, 2);
            //picks a random number between -1 and 1 to change the enemy speed
            enemySpeed += change;
            //applies speed change to the enemy speed
            timer = 4f;
            //resets enemy speed change timer
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
