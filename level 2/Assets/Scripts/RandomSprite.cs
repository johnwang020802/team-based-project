using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour
{
    [Header("Put your images here")]
    public Sprite[] randomImages;

    void Start()
    {
        // Check to make sure you actually put images in the list
        if (randomImages.Length > 0)
        {
            // Pick a random number between 0 and the number of images you have
            int randomIndex = Random.Range(0, randomImages.Length);
            
            // Change the object's SpriteRenderer to the chosen image
            GetComponent<SpriteRenderer>().sprite = randomImages[randomIndex];
        }
    }
}