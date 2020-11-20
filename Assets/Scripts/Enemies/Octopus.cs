using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : Enemy
{
    private Transform parent;
    private bool horizontal;

    private float waitShootTime;
    public float startWaitShootTime;

    private float waitDirectionChange;
    private Vector2[] directions =
    {
        new Vector2(1,0),
        new Vector2(-1,0),
        new Vector2(0,1),
        new Vector2(0,-1)
    };
    private int currentDirection;

    public override void initEnemy()
    {
        lifePoints = 150;
        waitTime = startWaitTime;
        startWaitShootTime = Random.Range(2, 4);
        waitShootTime = startWaitShootTime;

        parent = transform.parent;
        transform.position = Util.getRandomPosition(parent, 0);

        waitDirectionChange = Random.Range(3f, 7f);


        currentDirection = Random.Range(0, 4);
        if(currentDirection < 2)
        {
            horizontal = true;
        }
        else
        {
            horizontal = false;
        }

    }

    private void invertDirection()
    {
        if (horizontal)
        {
            if (currentDirection == 0) currentDirection = 1;
            else currentDirection = 0;
        }
        else
        {
            if (currentDirection == 2) currentDirection = 3;
            else currentDirection = 2;
        }
    }

    private void changeDirection()
    {
        if (horizontal)
        {
            horizontal = false;
            currentDirection = Random.Range(2,4);
        }
        else
        {
            horizontal = true;
            currentDirection = Random.Range(0, 2);
        }
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
        rb.MovePosition(rb.position + directions[currentDirection] * speed * Time.deltaTime);
        if (collidingStaticObject)
        {
            invertDirection();
            collidingStaticObject = false;
        }

        if(waitDirectionChange < 0)
        {
            changeDirection();
            waitDirectionChange = Random.Range(5f, 7f);
        }
        else
        {
            waitDirectionChange -= Time.deltaTime;
        }

        if (waitShootTime < 0)
        {
            shoot();
            waitShootTime = Random.Range(3f, 10f);
        }
        else
        {
            waitShootTime -= Time.deltaTime;
        }
    }
}
