using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target; //target's 3D transform

    private Vector3 velocity = new Vector3(); //reference variable for smoothdamp of camera position
    public float smoothTime = 0.3f; //speed for linear movement
    public Vector3 offset; //offset for camera

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset; //set up initial position with offset
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime); //move from initial position to target's position with linear speed
        Vector3 verticalPosition = Vector3.Lerp(transform.position, targetPosition, 10f);

        transform.position = new Vector3(smoothPosition.x, verticalPosition.y, smoothPosition.z); //update position
    }

}
