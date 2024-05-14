using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour
{
    public List<Sprite> MeteorSprits;
    public Transform player; // The object towards which gravity is applied
    public float gravityStrength = 300f; // Strength of the gravitational force
    public float attractionRange = 15f; // Range within which objects are affected by gravity

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

    void FixedUpdate()
    {
        ApplyGravity();
    }

    /*void ApplyGravity()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractionRange);

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null && col.transform != targetObject)
            {
                float distanceToTarget = Vector3.Distance(col.transform.position, targetObject.position);
                if (distanceToTarget <= attractionRange)
                {
                    Vector3 direction = (targetObject.position - col.transform.position).normalized;
                    rb.AddForce(direction * gravityStrength * Time.fixedDeltaTime);
                }
            }
        }
    }*/

    void ApplyGravity()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attractionRange)
        {
            Vector3 direction = (transform.position - player.position).normalized;
            player.GetComponent<Rigidbody>().AddForce(direction * gravityStrength * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }

}
