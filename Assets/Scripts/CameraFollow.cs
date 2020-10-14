using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public Vector3 positionToMove;
    private bool moving;

    void Start()
    {
        positionToMove = transform.position;
        moving = false;
    }

    void Update()
    {
        if (moving)
        {
            
            transform.position = Vector3.Lerp(transform.position, positionToMove, Time.deltaTime * moveSpeed);
            float distance = Vector3.Distance(transform.position, positionToMove);

            if (distance < 0.1) moving = false;
        }
        
    }

    public void moveCamera(Vector3 pos)
    {
        positionToMove = pos;
        positionToMove.z = transform.position.z;
        moving = true;
    }
}
