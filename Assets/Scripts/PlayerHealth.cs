using UnityEngine;
using UnityEngine.UI; // We must include this library to talk to Canvas UI elements like Images

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Math")]
    [Tooltip("The total amount of health the Rat starts with.")]
    public int maxHealth = 3;
    
    [Tooltip("The Rat's current health. The Inspector will show this drop as you take damage.")]
    public int currentHealth;

    [Header("UI Connections")]
    [Tooltip("Drag the Canvas Image that displays the health bar into this slot.")]
    public Image healthUIImage;
    
    [Tooltip("Put your health bar pictures here. Element 0 MUST be the empty/dead bar, and the last Element MUST be the full bar.")]
    public Sprite[] healthStateSprites;

    void Start()
    {
        // When the game starts, heal the Rat to completely full health
        currentHealth = maxHealth;
        
        // Update the screen so the picture matches the full health numbers
        UpdateHealthBar();
    }

    // "public" means our PlayerController script is allowed to trigger this function when it touches an enemy
    public void TakeDamage(int damageAmount)
    {
        // Subtract the damage from our current health pool
        currentHealth -= damageAmount;

        // Failsafe: Don't let health drop below exactly 0 (no negative numbers)
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Now that the math is done, update the visual picture on the screen
        UpdateHealthBar();

        // Check if the Rat is dead
        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        // Failsafe: If we forgot to assign the UI or the pictures in the Inspector, stop running so the game doesn't crash
        if (healthUIImage == null || healthStateSprites.Length == 0) return;

        // Ensure the current health doesn't ask for a picture that doesn't exist in our list
        // (e.g., if currentHealth is 3, it pulls Element 3 from our Sprite array)
        int safeIndex = Mathf.Clamp(currentHealth, 0, healthStateSprites.Length - 1);
        
        // Swap the picture on the Canvas to the correct one
        healthUIImage.sprite = healthStateSprites[safeIndex];
    }

    private void Die()
    {
        // This is where you would put your game over logic!
        // For now, it just prints a message to your Console and destroys the Rat.
        Debug.Log("The Rat has died!");
        Destroy(gameObject);
    }
}