using UnityEngine;
// We must include this specific library to unlock scene switching commands
using UnityEngine.SceneManagement; 

public class MenuController : MonoBehaviour
{
    // "public" means our UI Buttons are allowed to see and trigger this specific block of code.
    // "string levelName" means the button will hand this script a word (the name of the level) when clicked.
    public void LoadLevel(string levelName)
    {
        // Tells Unity to freeze the current scene, destroy it, and load the scene matching the exact word provided.
        // NOTE: The scene MUST be dragged into File > Build Settings for this to work!
        SceneManager.LoadScene(levelName);
    }

    // A separate block of code for the Quit button.
    public void QuitGame()
    {
        // Closes the application entirely.
        Application.Quit();
        
        // Application.Quit doesn't visually do anything while you are testing inside the Unity Editor.
        // Debug.Log prints a message to the Console so you know the button actually worked!
        Debug.Log("Game Quit!"); 
    }
}