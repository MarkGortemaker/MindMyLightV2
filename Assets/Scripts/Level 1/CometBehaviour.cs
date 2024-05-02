using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometBehaviour : MonoBehaviour
{
    Transform playerTransform;

    float speed = 0.1f;
    float bumpSpeed = 2.5f;

    static bool IsRunningAway = false;
    
    Rigidbody rb;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>(); 
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Obstacle")
        {
            col.attachedRigidbody.AddForce((col.transform.position - transform.position) * bumpSpeed / 2, ForceMode.Impulse);
            rb.AddForce((transform.position - col.transform.position) * bumpSpeed / 2, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        if (IsRunningAway)
        {
            transform.LookAt(playerTransform.position);
            transform.Rotate(0f, Mathf.Lerp(0, -180f, 0.5f), 0f);
            transform.Translate(Vector3.forward * speed, Space.Self);
        }

        else
        {
            transform.LookAt(playerTransform.position);
            transform.Translate(Vector3.forward * speed, Space.Self);
        }

        IsRunningAway = Level1Controller.DespawnComet(gameObject);
    }
    
}
