using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // This method will be triggered by your UI buttons
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    // Optional: A button to quit the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!"); // Only shows in editor, but confirms it works
    }
}