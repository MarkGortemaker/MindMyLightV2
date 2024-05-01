using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSpawn : MonoBehaviour
{
    public static Vector3 lastPosition = Vector3.zero;

    /// <summary>
    /// Instantiates a GameObject in a random spot between the given two radiuses, centered on the given Vector3 position.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="minRadius"></param>
    /// <param name="maxRadius"></param>
    /// <param name="center"></param>
    public static GameObject SpawnInCircleArea(GameObject obj, float minRadius, float maxRadius, Vector3 center)
    {
        Vector3 spawnDirection = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));
        Vector3 spawnPosition = center + spawnDirection.normalized * (Random.Range(minRadius, maxRadius));

        if ((spawnPosition - lastPosition).magnitude < 20f)
        {
            Debug.Log("Too close!");
        }

        lastPosition = spawnPosition;

        return Instantiate(obj, spawnPosition, Quaternion.identity);
    }
}
