using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour
{
    public List<Sprite> MeteorSprits;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the list is not empty
        if (MeteorSprits != null && MeteorSprits.Count > 0)
        {
            // Generate a random index
            int randomIndex = Random.Range(0, MeteorSprits.Count);

            // Get the SpriteRenderer component
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            // Assign the random sprite to the SpriteRenderer
            spriteRenderer.sprite = MeteorSprits[randomIndex];
        }
        
    }


    // Update is called once per frame
    void Update()

    {
        
    }

}
