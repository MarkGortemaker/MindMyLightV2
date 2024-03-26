using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float borderX = 50; //values for the stage borders
    public static float borderY = 50;
    public static float borderZ = 50;

    public static List<GameObject> balloons = new List<GameObject>();
    public static List<GameObject> birds = new List<GameObject>();

    void Start()
    {
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Balloon")) //set balloon positions and add them to a list
        {
            balloons.Add(i);
            Spawn(i, borderX, -borderX, borderY, -borderY, borderZ, -borderZ);
        }

        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Bird"))
        {
            birds.Add(i);
            if (birds.Count % 2 == 0) //even numbered birds and odd numbered birds go cross 
            {
                Spawn(i, borderX, -borderX, borderY, -borderY, borderZ, borderZ);
            }
            else
            {
                Spawn(i, borderX, borderX, borderY, -borderY, borderZ, -borderZ);
                i.transform.Rotate(new Vector3(0, 0, 90)); //axis must be changed after models are fixed
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < birds.Count; i++) 
        {
            GameObject bird = birds[i];
            if (Mathf.Abs(bird.transform.position.x) > borderX + 10 || Mathf.Abs(bird.transform.position.z) > borderZ + 10) //re"spawn" when hitting a border
            {
                if (i % 2 != 0)  
                {
                    Spawn(bird, borderX, -borderX, borderY, -borderY, borderZ, borderZ);
                }
                else
                {
                    Spawn(bird, borderX, borderX, borderY, -borderY, borderZ, -borderZ);
                }
            }
        }
    }

    private void OnDrawGizmos() //draw cube to visualize stage borders
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(Vector3.zero, 2 * new Vector3(borderX, borderY, borderZ));
    }

    public static void Spawn(GameObject obj, float Xmax, float Xmin, float Ymax, float Ymin, float Zmax, float Zmin)
    {
        obj.transform.position = new Vector3(Random.Range(Xmin, Xmax), Random.Range(Ymin, Ymax), Random.Range(Zmin, Zmax));
        obj.SetActive(true);
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
