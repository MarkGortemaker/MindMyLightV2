using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3 (0, 15, -10);
    public Vector3 referencePosition = Vector3.zero;

    public bool IsMain = true;

    void Start()
    {
        transform.position = target.position + offset;
        if (IsMain)
        {
            transform.LookAt(target.position);
        }
    }

    void FixedUpdate()
    {
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, target.position + offset, ref referencePosition, 0.2f);
        transform.position = smoothPosition;
    }
}
