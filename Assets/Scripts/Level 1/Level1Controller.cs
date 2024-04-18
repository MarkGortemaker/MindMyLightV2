using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Controller : MonoBehaviour
{
    public static float borderDistance = 10f;

    public static float stardustMeter;

    public int spawnCount = 5;
    public GameObject stardustLine;

    public static List<GameObject> meteors = new List<GameObject>();
    public static List<GameObject> comets = new List<GameObject>();
    void Start()
    {
        for (int i  = 0; i < spawnCount; i++) { 
            Instantiate(stardustLine, Vector3.zero, Quaternion.identity);
        }

        stardustMeter = 0f;
    }

    void Update()
    {
        
    }
}
