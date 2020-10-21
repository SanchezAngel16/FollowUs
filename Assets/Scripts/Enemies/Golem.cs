using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    public Vector2[] targetsPositions;
    private int nextTargetIndex;
    private Transform parent;

    private float waitShootTime;
    public float startWaitShootTime;

    public override void initEnemy()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 10;
        lifePoints = 150;
        nextTargetIndex = Random.Range(0, targetsPositions.Length);
        transform.position = targetsPositions[nextTargetIndex];
    }

    public override void move()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetsPositions[nextTargetIndex], speed * Time.deltaTime));
        if (Vector2.Distance(transform.position, targetsPositions[nextTargetIndex]) < 0.2f || collidingStaticObject)
        {
            if (waitTime < 0)
            {
                nextTargetIndex++;
                if (nextTargetIndex >= targetsPositions.Length) nextTargetIndex = 0;
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (waitShootTime < 0)
        {
            shoot();
            waitShootTime = startWaitShootTime;
        }
        else
        {
            waitShootTime -= Time.deltaTime;
        }
    }

    private void shoot()
    {

    }
}
