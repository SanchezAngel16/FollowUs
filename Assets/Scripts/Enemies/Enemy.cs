
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Photon.Pun;

public abstract class Enemy : MonoBehaviourPunCallbacks
{
    public float speed;
    protected Vector2[] spots;
    protected int currentDestination;

    protected float waitTime;
    public float startWaitTime;

    public int lifePoints;


    public BulletPool bulletsPool;
    public BulletPool outsideBulletPool;
    public Rigidbody2D rb;

    protected bool collidingStaticObject;

    public GameObject[] collectables;
    protected bool lootMaker = true;

    //protected Room currentRoom;

    public bool destroy = false;

    public Animator anim;
    private void Start()
    {
        initEnemy();
        
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            move();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("StaticObject") || tag.Equals("Door")) collidingStaticObject = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("StaticObject") || collision.gameObject.CompareTag("Door")) collidingStaticObject = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
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
            Instantiate(PrefabManager.Instance.explosionEffect).transform.position = transform.position;
            if (Random.Range(0, 100) >= 10 && lootMaker)
            {
                GameObject collectable = Instantiate(collectables[Random.Range(0, collectables.Length)]);
                collectable.transform.position = transform.position;
            }
            GameController.Instance.enemiesCount--;
            if (GameController.Instance.enemiesCount <= 0)
            {
                //GameController.Instance.updateUIArrows();
                VotingSystem.Instance.updateUIDirectionsButtons();
            }
        }
    }

    protected void removeEnemy()
    {
        if (photonView.IsMine)
        {
            //PhotonNetwork.Destroy(photonView);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    protected int removeLifePoints(int points)
    {
        this.lifePoints -= points;
        return this.lifePoints;
    }

    public abstract void initEnemy();
    public abstract void move();

    #region RPC Calls



    [PunRPC]
    public void displayBullet(int bulletsCount, float startingAngle, float incrementalAngles, float bulletSpeed, int direction)
    {
        if (bulletsCount == 1)
        {
            GameObject bullet = outsideBulletPool.getBullet();
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed * direction, ForceMode2D.Impulse);
        }
        else
        {
            GameObject[] bullets = new GameObject[bulletsCount];
            float angle = startingAngle;
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = outsideBulletPool.getBullet();
                bullets[i].transform.position = transform.position;
                bullets[i].transform.rotation = Quaternion.Euler(0, 0, angle);
                bullets[i].SetActive(true);
                Rigidbody2D rb = bullets[i].GetComponent<Rigidbody2D>();
                rb.AddForce(bullets[i].transform.up * bulletSpeed * direction, ForceMode2D.Impulse);
                angle += incrementalAngles;
            }
        }
    }

    #endregion
}
