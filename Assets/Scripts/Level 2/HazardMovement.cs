using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMovement : MonoBehaviour
{
    public float speed = 30f;
    public float variation = 10f;

    private void Start()
    {
        speed = Random.Range(speed - variation, speed + variation);
    }
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime); //should be Vector3.forward but the model is turned to face up
    }
}
