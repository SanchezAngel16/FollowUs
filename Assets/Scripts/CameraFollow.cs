using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 0.125f;

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    private void Start()
    {
        desiredPosition = new Vector3(0, 0, transform.position.z);
        smoothedPosition = new Vector3(0,0,0);
    }

    void FixedUpdate()
    {
        desiredPosition.x = target.position.x;
        desiredPosition.y = target.position.y;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
