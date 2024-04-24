using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarIndicatorArrow : MonoBehaviour
{
    private Vector3 targetPosition;
    private Transform playerTransform;
    void Awake()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Star").transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        Vector3 direction = (targetPosition - playerTransform.position); //take the difference between player position and target position for the direction

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg; //get angle between direction's x and z axes
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
    }
}
