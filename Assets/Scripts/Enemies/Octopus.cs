using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : Enemy
{
    private int direction;
    private Vector2 targetPosition;
    private Transform parent;
    private bool horizontal;

    private float waitShootTime;
    public float startWaitShootTime;

    public override void initEnemy()
    {
        lifePoints = 150;
        waitTime = startWaitTime;
        startWaitShootTime = Random.Range(2, 4);
        waitShootTime = startWaitShootTime;

        parent = transform.parent;
        transform.position = Util.getRandomPosition(parent, 0);

        direction = 1;

        int randomDirection = Random.Range(0, 10);
        if (randomDirection >= 5) horizontal = true;
        else horizontal = false;

        if (horizontal) targetPosition = new Vector2(parent.position.x + Util.playableArea, transform.position.y);
        else targetPosition = new Vector2(transform.position.x, parent.position.y + Util.playableArea);
    }

    private void shoot()
    {
        GameObject[] bullets = new GameObject[4];
        float angle = 0;
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = bulletsPool.getBullet();
            bullets[i].transform.position = transform.position;
            bullets[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            bullets[i].SetActive(true);
            Rigidbody2D rb = bullets[i].GetComponent<Rigidbody2D>();
            rb.AddForce(bullets[i].transform.up * 3, ForceMode2D.Impulse);
            angle += 90;
        }
    }

    public override void move()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, (speed * Util.enemiesSpeed) * Time.fixedDeltaTime));
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f || collidingStaticObject)
        {
            if (waitTime < 0)
            {
                //Change destination target
                direction *= -1;

                if (horizontal)
                {
                    targetPosition.x = parent.position.x + (Util.playableArea * direction);
                }
                else
                {
                    targetPosition.y = parent.position.y + (Util.playableArea * direction);
                }


                if (currentDestination == 1) currentDestination = 0;
                else currentDestination = 1;
                waitTime = Random.Range(1f, 3f);
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
}
