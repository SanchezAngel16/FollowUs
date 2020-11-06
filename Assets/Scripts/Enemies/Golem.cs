using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    public Vector2[] targetsPositions;
    private int currentTargetPositionIndex;
    public int direction;

    private float waitShootTime;
    public float startWaitShootTime;

    public override void initEnemy()
    {
        lifePoints = 150;
        waitShootTime = startWaitShootTime;
        if (Random.Range(0, 10) >= 10) direction = 1;
        else direction = -1;
        targetsPositions = getCornersPositions();
        currentTargetPositionIndex = Random.Range(0, targetsPositions.Length);
        transform.position = targetsPositions[currentTargetPositionIndex];
        rotate();
    }

    public override void move()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetsPositions[currentTargetPositionIndex], (speed * Util.enemiesSpeed) * Time.fixedDeltaTime));
        if (Vector2.Distance(transform.position, targetsPositions[currentTargetPositionIndex]) < 0.2f || collidingStaticObject)
        {
            if (waitTime < 0)
            {
                if(direction == 1)
                {
                    currentTargetPositionIndex++;
                    if (currentTargetPositionIndex >= targetsPositions.Length) currentTargetPositionIndex = 0;
                }
                else
                {
                    currentTargetPositionIndex--;
                    if (currentTargetPositionIndex < 0) currentTargetPositionIndex = targetsPositions.Length-1;
                }
                rotate();
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (waitShootTime < 0)
        {
            InvokeRepeating("shoot", 0, 0.1f);
            Invoke("stopShooting", .3f);
            waitShootTime = Random.Range(3f, 5f);
        }
        else
        {
            waitShootTime -= Time.deltaTime;
        }
    }

    private void stopShooting()
    {
        CancelInvoke("shoot");
    }

    private void shoot()
    {
        GameObject bullet = bulletsPool.getBullet();
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(-transform.up * 8f, ForceMode2D.Impulse);
    }

    private Vector2[] getCornersPositions()
    {
        Vector2[] targets = new Vector2[4];
        Room currentRoom = this.transform.parent.parent.parent.GetComponent<Room>();
        for(int i = 0; i < 4; i++)
        {
            targets[i] = currentRoom.corners[i].position;
        }
        return targets;
    }

    private void rotate()
    {
        switch (currentTargetPositionIndex)
        {
            case 0:
                if(direction == 1) transform.rotation = Quaternion.Euler(0, 0, 180);
                else transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case 1:
                if (direction == 1) transform.rotation = Quaternion.Euler(0, 0, -90);
                else transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 2:
                if (direction == 1) transform.rotation = Quaternion.Euler(0, 0, 0);
                else transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 3:
                if (direction == 1) transform.rotation = Quaternion.Euler(0, 0, 90);
                else transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
        }
    }
}
