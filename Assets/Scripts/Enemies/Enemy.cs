
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour
{
    public float speed;
    protected Vector2[] spots;
    protected int currentDestination;

    protected float waitTime;
    public float startWaitTime;

    public int lifePoints;


    public BulletPool bulletsPool;
    public Rigidbody2D rb;

    protected bool collidingStaticObject;

    public GameObject[] collectables;
    protected bool lootMaker = true;

    protected Room currentRoom;

    public bool destroy = false;
    private void Start()
    {
        initEnemy();
    }

    private void FixedUpdate()
    {
        move();
    }

    private void OnCollisionStay2D(Collision2D collision)
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
            if (collision.gameObject.GetComponent<Bullet>().hit) return;
            collision.gameObject.GetComponent<Bullet>().hit = true;
            collision.gameObject.SetActive(false);
            if (removeLifePoints(40) <= 0)
            {
                destroy = true;
                removeEnemy();
            }
        }
    }

    private void OnDestroy()
    {
        if (destroy)
        {
            if (Random.Range(0, 100) >= 10 && lootMaker)
            {
                GameObject collectable = Instantiate(collectables[Random.Range(0, collectables.Length)]);
                collectable.transform.position = transform.position;
            }
            Main.Instance.enemies.Remove(this.transform);
            Main.Instance.enemiesCount--;
            if (Main.Instance.enemiesCount <= 0)
            {
                Main.Instance.updateUIArrows();
            }
        }
    }

    protected void removeEnemy()
    {
        Destroy(gameObject);
    }

    protected int removeLifePoints(int points)
    {
        this.lifePoints -= points;
        return this.lifePoints;
    }

    public abstract void initEnemy();
    public abstract void move();

    public void setCurrentRoom(Room r)
    {
        currentRoom = r;
    }
}
