using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public float movementSpeed = 30f;

    public float rotationSpeed = 0;

    public float variation = 10f;

    private void Start()
    {
        movementSpeed = Random.Range(movementSpeed - variation, movementSpeed + variation);
    }
    void Update()
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime); //should be Vector3.forward but the model is turned to face up

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
