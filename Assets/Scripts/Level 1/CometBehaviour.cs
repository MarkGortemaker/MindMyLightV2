using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometBehaviour : MonoBehaviour
{
    Transform playerTransform;
    Transform starTransform;

    float speed = 0.08f;
    float bumpSpeed = 2.5f;

    static bool IsRunningAway = false;
    
    Rigidbody rb;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        starTransform = GameObject.FindGameObjectWithTag("Star").transform;
        rb = GetComponent<Rigidbody>(); 
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Obstacle")
        {
            col.attachedRigidbody.AddForce((col.transform.position - transform.position) * bumpSpeed / 3, ForceMode.Impulse);
            rb.AddForce((transform.position - col.transform.position) * bumpSpeed / 3, ForceMode.Impulse);
        }

        else if (col.tag == "Safe Zone")
        {
            rb.AddForce((transform.position - col.transform.position) * bumpSpeed / 4, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        //code for the ugly placeholder model that decided "forward" is up, actually
        if (IsRunningAway)
        {
            transform.LookAt(starTransform.position);
            transform.Rotate(0f, Mathf.Lerp(0, -230f, 0.5f), 0f);
            transform.Rotate(0f, 90f, 90f);
            transform.Translate(Vector3.up * speed, Space.Self);
        }

        else
        {
            transform.LookAt(playerTransform.position);
            transform.Rotate(0f, 90f, 90f);
            transform.Translate(Vector3.up * speed, Space.Self);
        }

        IsRunningAway = Level1Controller.DespawnComet(gameObject);

        /* Normal, well structured code for normal, well structured models
        if (IsRunningAway)
        {
            transform.LookAt(starTransform.position);
            transform.Rotate(0f, Mathf.Lerp(0, -230f, 0.5f), 0f);
            transform.Translate(Vector3.forward * speed, Space.Self);
        }

        else
        {
            transform.LookAt(playerTransform.position);
            transform.Translate(Vector3.forward * speed, Space.Self);
        }

        IsRunningAway = Level1Controller.DespawnComet(gameObject);
        */
    }
    
}
