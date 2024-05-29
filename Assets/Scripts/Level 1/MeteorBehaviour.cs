using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour
{
    public List<Sprite> MeteorSprites;
    public Transform playerTransform; // The object towards which gravity is applied
    public static float gravityStrength = 200f; // Strength of the gravitational force
    public float attractionRange = 15f; // Range within which objects are affected by gravity

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Check if the list is not empty
        if (MeteorSprites != null && MeteorSprites.Count > 0)
        {
            // Generate a random index
            int randomIndex = Random.Range(0, MeteorSprites.Count);

            // Get the SpriteRenderer component
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            // Assign the random sprite to the SpriteRenderer
            spriteRenderer.sprite = MeteorSprites[randomIndex];

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    void FixedUpdate()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer <= attractionRange)
        {
            Vector3 direction = (transform.position - playerTransform.position).normalized;
            playerTransform.GetComponent<Rigidbody>().AddForce(direction * gravityStrength * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }

}
