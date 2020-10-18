using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float speed;
    protected Vector2[] spots;
    protected int currentDestination;

    protected float waitTime;
    public float startWaitTime;

    public int lifePoints;


    public BulletPool bulletsPool;
    protected Rigidbody2D rb;

    protected bool collidingStaticObject;

    private void Start()
    {
        initEnemy();
    }

    private void Update()
    {
        move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("StaticObject")) collidingStaticObject = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("StaticObject")) collidingStaticObject = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("PlayerBullet"))
        {
            removeLifePoints(15);
            collision.gameObject.SetActive(false);
        }
    }

    public void removeLifePoints(int points)
    {
        this.lifePoints -= points;
        if (this.lifePoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public abstract void initEnemy();
    public abstract void move();
}
