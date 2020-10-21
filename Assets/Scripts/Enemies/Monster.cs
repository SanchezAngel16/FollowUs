using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Enemy
{
    private int direction;
    private Vector2[] targetsPositions;
    private int nextTargetIndex;
    private Transform parent;
    private bool horizontal;

    public override void initEnemy()
    {
        rb = GetComponent<Rigidbody2D>();
        lifePoints = 150;
        waitTime = startWaitTime;
        parent = transform.parent;
        transform.position = parent.position;

        targetsPositions = new Vector2[16];

        targetsPositions[0] = new Vector2(transform.position.x + Util.playableArea-.5f, transform.position.y);
        targetsPositions[1] = new Vector2(parent.position.x, parent.position.y);
        targetsPositions[2] = new Vector2(transform.position.x + Util.playableArea-.5f, transform.position.y + Util.playableArea-.5f);
        targetsPositions[3] = new Vector2(parent.position.x, parent.position.y);
        targetsPositions[4] = new Vector2(transform.position.x, transform.position.y + Util.playableArea-.5f);
        targetsPositions[5] = new Vector2(parent.position.x, parent.position.y);
        targetsPositions[6] = new Vector2(transform.position.x - Util.playableArea+.5f, transform.position.y + Util.playableArea-.5f);
        targetsPositions[7] = new Vector2(parent.position.x, parent.position.y);
        targetsPositions[8] = new Vector2(transform.position.x - Util.playableArea+.5f, transform.position.y);
        targetsPositions[9] = new Vector2(parent.position.x, parent.position.y);
        targetsPositions[10] = new Vector2(transform.position.x - Util.playableArea+.5f, transform.position.y - Util.playableArea+.5f);
        targetsPositions[11] = new Vector2(parent.position.x, parent.position.y);
        targetsPositions[12] = new Vector2(transform.position.x, transform.position.y - Util.playableArea+.5f);
        targetsPositions[13] = new Vector2(parent.position.x, parent.position.y);
        targetsPositions[14] = new Vector2(transform.position.x + Util.playableArea-.5f, transform.position.y - Util.playableArea+.5f);
        targetsPositions[15] = new Vector2(parent.position.x, parent.position.y);

        nextTargetIndex = 0;
    }

    private void shoot()
    {
        GameObject[] bullets = new GameObject[24];
        float angle = 0;
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = bulletsPool.getBullet();
            bullets[i].GetComponent<Bullet>().bulletType = 1;
            bullets[i].transform.position = transform.position;
            bullets[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            bullets[i].SetActive(true);
            Rigidbody2D rb = bullets[i].GetComponent<Rigidbody2D>();
            rb.AddForce(bullets[i].transform.up * 2f, ForceMode2D.Impulse);
            angle += 15f;
        }
    }

    public override void move()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetsPositions[nextTargetIndex], speed * Time.deltaTime));
        if (Vector2.Distance(transform.position, targetsPositions[nextTargetIndex]) < 0.2f || collidingStaticObject)
        {
            if (waitTime < 0)
            {
                //Change destination target
                if(!(nextTargetIndex % 2 == 0))
                {
                    shoot();
                }
                nextTargetIndex++;
                if (nextTargetIndex >= targetsPositions.Length) nextTargetIndex = 0;
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
