using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilEye : Enemy
{
    private Vector2 targetPosition;

    private float waitShootTime;
    public float startWaitShootTime;

    private float spinSpeed = 360;


    public override void initEnemy()
    {
        GameController.Instance.enemiesCount++;
        lifePoints = 150;
        waitTime = startWaitTime;
        startWaitShootTime = Random.Range(2, 4);
        waitShootTime = startWaitShootTime;

        targetPosition = Util.getRandomPosition(transform.parent, 0);
    }

    private void shoot()
    {
        int bulletsCount = 4;
        GameObject[] bullets = new GameObject[bulletsCount];
        float angle = Random.Range(0f, 360f);
        float startingAngle = angle;
        float incrementalAngles = 25f;
        float bulletSpeed = 3f;
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = bulletsPool.getBullet();
            bullets[i].transform.position = transform.position;
            bullets[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            bullets[i].SetActive(true);
            Rigidbody2D rb = bullets[i].GetComponent<Rigidbody2D>();
            rb.AddForce(bullets[i].transform.up * bulletSpeed, ForceMode2D.Impulse);
            angle += incrementalAngles;
        }

        photonView.RPC("displayBullet", RpcTarget.Others, bulletsCount, startingAngle, incrementalAngles, bulletSpeed, 1);
    }

    public override void move()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, (speed * CurseManager.enemiesSpeed) * Time.deltaTime));
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f || collidingStaticObject)
        {
            targetPosition = Util.getRandomPosition(transform.parent, 0);
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

    #region RPC Calls



    #endregion
}
