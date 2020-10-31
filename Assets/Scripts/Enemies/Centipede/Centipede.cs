using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : Enemy
{
    public List<List<GameObject>> body;
    public Transform[] pathPositions;
    public Transform firePointR;
    public Transform firePointL;
    private float startWaitShootTime;
    private float waitShootTime;

    private int targetIndex = 1;

    private bool head;
    private int bodyPos;
    private float startMoveTime;

    public Sprite[] bodySprites;
    public SpriteRenderer spriteRenderer;


    public override void initEnemy()
    {
        lifePoints = 150;
        transform.position = new Vector2(transform.position.x + Util.playableArea-.5f, transform.position.y - Util.playableArea+.5f);
        pathPositions = currentRoom.centipedePoints;
        waitShootTime = Random.Range(2f, 10f);
    }

    public override void move()
    {
        if(startMoveTime <= 0)
        {
            
            rb.MovePosition(Vector2.MoveTowards(transform.position, pathPositions[targetIndex].position, speed * Time.fixedDeltaTime));
            if (Vector2.Distance(transform.position, pathPositions[targetIndex].position) < 0.2f || collidingStaticObject)
            {
                if (waitTime < 0)
                {
                    targetIndex++;
                    if (targetIndex == pathPositions.Length) targetIndex = 0;
                    rotate();
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
        else
        {
            startMoveTime -= Time.deltaTime;
        }

        if (waitShootTime < 0)
        {
            shoot();
            waitShootTime = Random.Range(2f, 10f);
        }
        else
        {
            waitShootTime -= Time.deltaTime;
        }

    }

    private void shoot()
    {
        GameObject lBullet = bulletsPool.getBullet();
        lBullet.SetActive(true);
        lBullet.transform.position = transform.position;
        lBullet.transform.rotation = firePointL.rotation;
        lBullet.GetComponent<Rigidbody2D>().AddForce(firePointL.transform.up * 7f, ForceMode2D.Impulse);

        GameObject rBullet = bulletsPool.getBullet();
        rBullet.SetActive(true);
        rBullet.transform.position = transform.position;
        rBullet.transform.rotation = firePointR.rotation;
        rBullet.GetComponent<Rigidbody2D>().AddForce(firePointR.transform.up * 7f, ForceMode2D.Impulse);

    }

    private void rotate()
    {
        Vector2 target = pathPositions[targetIndex].position;
        Vector2 lookDir = target - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void setCentipedeAttributes(bool h, int bP, Room r)
    {
        this.currentRoom = r;
        this.head = h;
        this.bodyPos = bP;
        this.startMoveTime = bodyPos * 0.15f;
        setSprite();
    }

    private void setSprite()
    {
        if (this.head) spriteRenderer.sprite = bodySprites[0];
        else
        {
            lootMaker = false;
            spriteRenderer.sprite = bodySprites[1];
        }
    }
}
