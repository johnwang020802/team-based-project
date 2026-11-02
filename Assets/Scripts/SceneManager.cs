using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class GameManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Type the exact name of the scene you want to load (e.g., Level_02)")]
    public string nextSceneName = "Level_02";

    void Update()
    {
        // Checks if a keyboard is connected and if the '1' key was pressed this frame
        if (Keyboard.current != null && Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}