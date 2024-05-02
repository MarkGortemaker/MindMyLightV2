using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardustSetPosition : MonoBehaviour
{
    public GameObject stardust;

    public Transform playerTransform;
    public Transform starTransform;

    LineRenderer lineRenderer;

    //NOTE: If there is any spare time, use pooling for the particle gameObjects to ease up on CPU usage when mass spawning.
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        starTransform = GameObject.FindGameObjectWithTag("Star").transform;

        for (int i = 0; i < lineRenderer.positionCount; i++) //set all y axes in the positions to 0
        {
            Vector3 flatPosition = new Vector3(lineRenderer.GetPosition(i).x, 0f, lineRenderer.GetPosition(i).z);
            lineRenderer.SetPosition(i, flatPosition); 
        }

        for (int i = 0; i < lineRenderer.positionCount; i += 2) //reposition the points according to randomized transform position 
        {
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) + transform.position);

            if (lineRenderer.GetPosition(i).magnitude <= Level1Controller.borderDistance - 5f)
            {
                GameObject stardustPatch = Instantiate(stardust, transform, false);
                stardustPatch.transform.position = lineRenderer.GetPosition(i);
                stardustPatch.GetComponent<ParticleSystem>().trigger.AddCollider(playerTransform);
            }
        }

        transform.LookAt(starTransform); //rotate all stardust patches in the line towards the star

        for (int i = 0; i < gameObject.GetComponentsInChildren<Transform>().Length; i++)
        {
            if (gameObject.GetComponentsInChildren<Transform>()[i].position.magnitude > Level1Controller.borderDistance - 5f)
            {
                Destroy(gameObject.GetComponentsInChildren<Transform>()[i].gameObject);
            }
        }
    }
}
