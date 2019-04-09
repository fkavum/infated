using UnityEngine;
using System.Collections;

// @NOTE the attached sprite's position should be "top left" or the children will not align properly
// Strech out the image as you need in the sprite render, the following script will auto-correct it when rendered in the game
[RequireComponent(typeof(SpriteRenderer))]

// Generates a nice set of repeated sprites inside a streched sprite renderer
// @NOTE Vertical only, you can easily expand this to horizontal with a little tweaking
public class repeatingBG : MonoBehaviour
{
    SpriteRenderer sprite;

    void Awake()
    {
        // Get the current sprite with an unscaled size
        sprite = GetComponent<SpriteRenderer>();
        Debug.Log(sprite.bounds.size.x);
        Debug.Log(transform.localScale.x);
        Debug.Log(sprite.bounds.size.x / transform.localScale.x);
        Vector2 spriteSize = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);

        // Generate a child prefab of the sprite renderer
        GameObject childPrefab = new GameObject();
        SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childPrefab.transform.position = transform.position;
        childSprite.sprite = sprite.sprite;

        // Loop through and spit out repeated tiles
        GameObject child;
        for (int i = 1, w = (int)Mathf.Round(sprite.bounds.size.x); i < w; i++) { 
            for (int j = 1, l = (int)Mathf.Round(sprite.bounds.size.y); j < l; j++)
            {
              
                child = Instantiate(childPrefab) as GameObject;
                child.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                child.transform.position = transform.position + (new Vector3(spriteSize.x * i, spriteSize.y * j, 0));
                child.transform.parent = transform;
            }
        }
        // Set the parent last on the prefab to prevent transform displacement
        childPrefab.transform.parent = transform;

        // Disable the currently existing sprite component since its now a repeated image
        sprite.enabled = false;
    }
}