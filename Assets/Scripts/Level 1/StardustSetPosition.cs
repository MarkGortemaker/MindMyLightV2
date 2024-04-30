using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardustSetPosition: MonoBehaviour
{
    public GameObject stardust;
    public GameObject player;
    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < lineRenderer.positionCount; i++) //set all y axes in the positions to 0
        {
            Vector3 flatPosition = new Vector3(lineRenderer.GetPosition(i).x, 0f, lineRenderer.GetPosition(i).z);
            lineRenderer.SetPosition(i, flatPosition); //find a way to turn them around randomly too maybe
        }

        for (int i = 0; i < lineRenderer.positionCount; i += 2) //reposition the points according to randomized transform position 
        { 
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) + transform.position);

            if (lineRenderer.GetPosition(i).magnitude <= Level1Controller.borderDistance - 5f)
            {
                GameObject stardustPatch = Instantiate(stardust, lineRenderer.GetPosition(i), Quaternion.identity);
                stardustPatch.GetComponent<ParticleSystem>().trigger.AddCollider(player.transform);
            }
        }
    }
}
