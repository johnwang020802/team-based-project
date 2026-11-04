using UnityEngine;

// Ensures this object has a SpriteRenderer (the component that displays 2D art).
[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour
{
    [Header("Put your images here")]
    [Tooltip("Add as many different sprites as you want to this list in the Inspector.")]
    public Sprite[] randomImages; // The brackets [] mean this is a list (array) of items, not just one.

    void Start()
    {
        // First, check to make sure we actually dragged images into the Inspector list.
        // If the list length is greater than 0, it means we have at least one picture.
        if (randomImages.Length > 0)
        {
            // Pick a random whole number.
            // 0 is the very first image in the list. 
            // randomImages.Length is the total count. Random.Range ignores the highest number, making the math perfect.
            int randomIndex = Random.Range(0, randomImages.Length);
            
            // 1. Grab the SpriteRenderer component attached to this object.
            // 2. Look at its "sprite" property (the actual picture being displayed).
            // 3. Change it to the picture we randomly selected from our list.
            GetComponent<SpriteRenderer>().sprite = randomImages[randomIndex];
        }
    }
}