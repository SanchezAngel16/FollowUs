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

    private int golemPosition;
    private bool hasChangeDirection = false;
    public void setGolemAttributes(int golemType, Vector2[] corners)
    {
        this.golemPosition = golemType;
        targetsPositions = getTargetsPositions(corners);
    }

    public override void initEnemy()
    {
        lifePoints = 150;
        waitShootTime = startWaitShootTime;
        
        if (Random.Range(0, 10) >= 5) direction = 1;
        else direction = -1;

        currentTargetPositionIndex = Random.Range(0, targetsPositions.Length);
        transform.position = targetsPositions[currentTargetPositionIndex];
        rotate();
    }

    public override void move()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetsPositions[currentTargetPositionIndex], (speed * CurseManager.enemiesSpeed) * Time.deltaTime));
        if (Vector2.Distance(transform.position, targetsPositions[currentTargetPositionIndex]) < 0.2f || collidingStaticObject)
        {
            if (waitTime < 0)
            {
                if (hasChangeDirection)
                {
                    direction *= -1;
                    hasChangeDirection = false;
                }
                else
                {
                    if (direction == 1)
                    {
                        currentTargetPositionIndex++;
                        if (currentTargetPositionIndex >= targetsPositions.Length) currentTargetPositionIndex = 0;
                    }
                    else
                    {
                        currentTargetPositionIndex--;
                        if (currentTargetPositionIndex < 0) currentTargetPositionIndex = targetsPositions.Length - 1;
                    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("StaticObject"))
        {
            hasChangeDirection = true;
            if (direction == 1)
            {
                currentTargetPositionIndex--;
                if (currentTargetPositionIndex < 0) currentTargetPositionIndex = targetsPositions.Length - 1;
            }
            else
            {
                currentTargetPositionIndex++;
                if (currentTargetPositionIndex >= targetsPositions.Length) currentTargetPositionIndex = 0;
            }
            
        }
    }

    private Vector2[] getTargetsPositions(Vector2[] corners)
    {
        
        if(golemPosition == 0)
        {
            return corners;
        }
        else
        {
            Vector2[] targets = new Vector2[4];
            targets[0].x = corners[0].x - (golemPosition - 0.5f);
            targets[0].y = corners[0].y + (golemPosition + 0.5f);

            targets[1].x = corners[1].x - (golemPosition - 0.5f);
            targets[1].y = corners[1].y - (golemPosition - 0.5f);

            targets[2].x = corners[2].x + (golemPosition + 0.5f);
            targets[2].y = corners[2].y - (golemPosition - 0.5f);

            targets[3].x = corners[3].x + (golemPosition + 0.5f);
            targets[3].y = corners[3].y + (golemPosition + 0.5f);
            return targets;
        }

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
