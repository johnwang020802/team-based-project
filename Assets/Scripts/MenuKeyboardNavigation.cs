using UnityEngine;
using UnityEngine.UI; // Required to talk to Canvas Buttons
using UnityEngine.EventSystems; // Required to visually highlight UI elements

public class MenuKeyboardNavigation : MonoBehaviour
{
    [Header("Menu Setup")]
    [Tooltip("Drag your Level buttons here in the exact order you want to navigate them (Left to Right)")]
    public Button[] levelButtons; 
    
    private int currentIndex = 0; // Tracks which button we are currently hovering over

    void Start()
    {
        // When the menu first loads, automatically highlight the very first button
        if (levelButtons.Length > 0)
        {
            SelectCurrentButton();
        }
    }

    void Update()
    {
        // Check for right movement (D key or Right Arrow)
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex++; // Move one slot forward in the list
            
            // If we go past the last button, wrap around back to the first one
            if (currentIndex >= levelButtons.Length) currentIndex = 0; 
            
            SelectCurrentButton();
        }

        // Check for left movement (A key or Left Arrow)
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex--; // Move one slot backward in the list
            
            // If we go before the first button, wrap around to the very last one
            if (currentIndex < 0) currentIndex = levelButtons.Length - 1; 
            
            SelectCurrentButton();
        }

        // Check for selection (Enter key)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (levelButtons.Length > 0)
            {
                // This command tells the button to fire exactly as if we clicked it with a mouse!
                levelButtons[currentIndex].onClick.Invoke(); 
            }
        }
    }

    private void SelectCurrentButton()
    {
        // This tells Unity's internal UI system to visually highlight the button on screen
        EventSystem.current.SetSelectedGameObject(null); // Clear old selection
        EventSystem.current.SetSelectedGameObject(levelButtons[currentIndex].gameObject); // Set new selection
    }
}