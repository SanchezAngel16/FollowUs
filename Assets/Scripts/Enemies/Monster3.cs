using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster3 : Enemy
{
    private float waitShootTime;
    public float startWaitShootTime;

    public Transform[] blocks;
    public Transform[] blocks_2;


    public override void initEnemy()
    {
        lifePoints = 150;
        startWaitShootTime = Random.Range(2, 4);
        waitShootTime = startWaitShootTime;
        this.transform.position = this.transform.parent.position;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < blocks_2.Length; i++)
        {
            blocks_2[i].gameObject.SetActive(false);
        }
    }

    private void shoot()
    {
        GameObject[] bullets = new GameObject[12];
        float angle = Random.Range(0f, 360f);
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = bulletsPool.getBullet();
            bullets[i].transform.position = transform.position;
            bullets[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            bullets[i].SetActive(true);
            Rigidbody2D rb = bullets[i].GetComponent<Rigidbody2D>();
            rb.AddForce(bullets[i].transform.up * 3, ForceMode2D.Impulse);
            angle += 30;
        }
    }

    private void rotateBlocks(Transform[] arrBlocks, float speed)
    {
        for(int i = 0; i < arrBlocks.Length; i++)
        {
            arrBlocks[i].RotateAround(this.transform.position, new Vector3(0, 0, 1), speed * Time.fixedDeltaTime);
        }
    }

    public override void move()
    {
        rotateBlocks(blocks, 300);
        rotateBlocks(blocks_2, 180);
        /*transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime));
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
        }*/

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
