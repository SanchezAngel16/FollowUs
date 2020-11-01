using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CentipedeBody : Enemy
{
    /*public CentipedePoint[] points;
    public CentipedePoint nextTarget;
    private CentipedePoint lastTarget;*/
    public Transform firePointR;
    public Transform firePointL;
    //private float startWaitShootTime;
    private float waitShootTime;

    public CentipedeBody head;
    public CentipedeBody nextBody;
    public CentipedeBody lastBody;

    /*private bool isHead;
    private int bodyPos;*/
    private int bodyPos;
    private float startMoveTime;
    public override void initEnemy()
    {
        lifePoints = 150;
        initCentipedeBody();

        waitShootTime = Random.Range(2f, 10f);
    }

    public override void move()
    {
        if (startMoveTime <= 0)
        {

            manageMovement();
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

    protected void rotate(Transform t)
    {
        //Vector2 target = pathPositions[targetIndex].position;
        Vector2 target = t.position;
        Vector2 lookDir = target - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void setCentipedeAttributes(int bodyPos, Room room)
    {
        this.currentRoom = room;
        this.bodyPos = bodyPos;
        this.startMoveTime = bodyPos * 0.15f;
    }


    public abstract void manageMovement();

    public abstract void initCentipedeBody();

}
