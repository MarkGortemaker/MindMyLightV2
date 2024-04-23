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

        Vector3 positionOffset = new Vector3(Random.Range(-50f, 50f), 0, Random.Range(-50f, 50f));

        for (int i = 0; i < lineRenderer.positionCount; i += 2) //randomly reposition the points when line spawns, then set up each stardust patch in the line
        {
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) + positionOffset); //find a way to turn them around randomly too maybe
            GameObject stardustPatch = Instantiate(stardust, lineRenderer.GetPosition(i), Quaternion.identity);
            stardustPatch.GetComponent<ParticleSystem>().trigger.AddCollider(player.transform);
        }
    }
}
