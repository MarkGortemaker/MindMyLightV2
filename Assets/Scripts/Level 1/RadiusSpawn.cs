using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSpawn : MonoBehaviour
{
    public static Vector3 lastPosition = Vector3.zero;

    /// <summary>
    /// Instantiates a GameObject in a random spot between the given two radiuses, centered on the given Vector3 position. collisionCheckMargin is used to check if any colliders exist in the space prior to spawning, in which case it will recalculate the position.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="minRadius"></param>
    /// <param name="maxRadius"></param>
    /// <param name="isolation"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    public static GameObject SpawnInCircleArea(GameObject obj, float minRadius, float maxRadius, float collisionCheckMargin, Vector3 center)
    {
        Vector3 spawnDirection = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));
        Vector3 spawnPosition = center + spawnDirection.normalized * (Random.Range(minRadius, maxRadius));

        for (int i = 0; i < 1000; i++)
        {
            if (Physics.CheckSphere(spawnPosition, collisionCheckMargin))
            {
                spawnDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                spawnPosition = center + spawnDirection.normalized * (Random.Range(minRadius, maxRadius));
            }

            else
            {
                break;
            }
        }

        return Instantiate(obj, spawnPosition, Quaternion.identity);
    }
}
