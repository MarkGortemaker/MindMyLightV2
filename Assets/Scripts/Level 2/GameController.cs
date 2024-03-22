using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float borderX = 50; //values for the stage borders
    public static float borderY = 50;
    public static float borderZ = 50;

    public static List<GameObject> balloons = new List<GameObject>();

    void Start()
    {
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Balloon")) //set balloon positions and add them to a list
        {
            balloons.Add(i);
            i.transform.position = new Vector3(Random.Range(-borderX, borderX), Random.Range(-borderY, borderY), Random.Range(-borderZ, borderZ));
            i.SetActive(true);
        }
    }

    private void OnDrawGizmos() //draw cube to visualize stage borders
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(Vector3.zero, 2 * new Vector3(borderX, borderY, borderZ));
    }

    public static void EnforceBorder(Transform t) //limit the transform passed in to stay in the stage borders
    {
        if (Mathf.Abs(t.position.x) > borderX)
        {
            if (t.position.x > 0)
            {
                t.position = new Vector3(borderX, t.position.y, t.position.z);
            }
            else 
            {
                t.position = new Vector3(-borderX, t.position.y, t.position.z);
            }
        }

        if (Mathf.Abs(t.position.y) > borderY)
        {
            if (t.position.y > 0)
            {
                t.position = new Vector3(t.position.x, borderY, t.position.z);
            }
            else 
            {
                t.position = new Vector3(t.position.x, -borderY, t.position.z);
            }
        }

        if (Mathf.Abs(t.position.z) > borderZ)
        {
            if (t.position.z > 0)
            {
                t.position = new Vector3(t.position.x, t.position.y, borderZ);
            }
            else 
            {
                t.position = new Vector3(t.position.x, t.position.y, -borderZ);
            }
        }
    }
}
