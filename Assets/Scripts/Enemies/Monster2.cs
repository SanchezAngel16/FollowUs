using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2 : Enemy
{
    private Vector2 targetPosition;

    private float waitShootTime;
    public float startWaitShootTime;

    private float spinSpeed = 360;


    public override void initEnemy()
    {
        rb = GetComponent<Rigidbody2D>();
        lifePoints = 150;
        waitTime = startWaitTime;
        startWaitShootTime = Random.Range(2, 4);
        waitShootTime = startWaitShootTime;

        targetPosition = Util.getRandomPosition(transform.parent, 0);
    }

    private void shoot()
    {
        GameObject[] bullets = new GameObject[4];
        float angle = Random.Range(0f, 360f);
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = bulletsPool.getBullet();
            bullets[i].transform.position = transform.position;
            bullets[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            bullets[i].SetActive(true);
            Rigidbody2D rb = bullets[i].GetComponent<Rigidbody2D>();
            rb.AddForce(bullets[i].transform.up * 3, ForceMode2D.Impulse);
            angle += 25;
        }
    }

    public override void move()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f || collidingStaticObject)
        {
            if (waitTime < 0)
            {
                //Change destination target
                targetPosition = Util.getRandomPosition(transform.parent, 0);
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
            waitShootTime = Random.Range(3f, 6f);
        }
        else
        {
            waitShootTime -= Time.deltaTime;
        }
    }
}
