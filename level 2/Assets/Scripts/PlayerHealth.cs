using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Stats")]
    public float currentHealth = 3f;
    public float maxHealth = 3f;

    [Header("Health Bar UI")]
    public Image healthUIImage;
    public Sprite[] healthStateSprites;

    void Start()
    {
        // Update the screen immediately when the game starts
        UpdateHealthUI(); 
    }

    // Called by pizzas or enemies when they hurt you
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Called by the Good Pizza
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    // Handles swapping the PNG pictures
    private void UpdateHealthUI()
{
    if (healthUIImage != null && healthStateSprites.Length > 0)
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        float healthPercent = currentHealth / maxHealth;
        
        int spriteIndex = Mathf.RoundToInt(healthPercent * (healthStateSprites.Length - 1));
        spriteIndex = Mathf.Clamp(spriteIndex, 0, healthStateSprites.Length - 1);
        
        healthUIImage.sprite = healthStateSprites[spriteIndex];

        // ADD THIS LINE:
        Debug.Log("Current Health: " + currentHealth + " | Chosen Image Index: " + spriteIndex);
    }
}
    private void Die()
    {
        Debug.Log("Health reached zero. Game Over!");
        Application.Quit();
    }
}