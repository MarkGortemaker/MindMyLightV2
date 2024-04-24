using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSpawn : MonoBehaviour
{
    public static Vector3 lastPosition = Vector3.zero;
    public static void SpawnInCircleArea(GameObject obj, float minRadius, float maxRadius, Vector3 center)
    {
        Vector3 spawnDirection = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));
        Vector3 spawnPosition = center + spawnDirection.normalized * (Random.Range(minRadius, maxRadius));

        if ((spawnPosition - lastPosition).magnitude < 20f)
        {
            Debug.Log("Too close!");
        }

        Instantiate(obj, spawnPosition, Quaternion.identity);

        lastPosition = spawnPosition;
    }
}
