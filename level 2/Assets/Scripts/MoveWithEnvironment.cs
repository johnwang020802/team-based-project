using UnityEngine;

public class MoveWithEnvironment : MonoBehaviour
{
    public float scrollSpeed = 5f; // Set this to match your Quad's scrolling speed

    void Update()
    {
        // Pushes the pizza constantly to the left
        transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
    }
}