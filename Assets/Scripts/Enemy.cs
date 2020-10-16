using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Vector2[] randomSpots;
    private int currentDestination;

    private float waitTime;
    public float startWaitTime;

    public int lifePoints;

    public BulletPool enemyBulletsPool;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lifePoints = 150;
        waitTime = startWaitTime;

        randomSpots = new Vector2[10];
        for (int i = 0; i < randomSpots.Length; i++)
        {
            //randomSpots[i] = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            randomSpots[i] = Util.getRandomPosition(transform.parent);
        }
        transform.position = randomSpots[0];
        currentDestination = Random.Range(0, randomSpots.Length);
    }

    private void Update()
    {
        //transform.position = ;
        rb.MovePosition(Vector2.MoveTowards(transform.position, randomSpots[currentDestination], speed * Time.deltaTime));
        if (Vector2.Distance(transform.position, randomSpots[currentDestination]) < 0.2f)
        {
            if(waitTime <= 0)
            {
                //Shoot
                GameObject[] bullets = new GameObject[4];
                float angle = 0;
                for(int i = 0; i < bullets.Length; i++)
                {
                    bullets[i] = enemyBulletsPool.getBullet();
                    bullets[i].GetComponent<Bullet>().bulletType = 1;
                    bullets[i].transform.position = transform.position;
                    bullets[i].transform.rotation = Quaternion.Euler(0,0,angle);
                    bullets[i].SetActive(true);
                    Rigidbody2D rb = bullets[i].GetComponent<Rigidbody2D>();
                    Debug.Log(bullets[i].transform.up);
                    rb.AddForce(bullets[i].transform.up * 3, ForceMode2D.Impulse);
                    angle += 90;
                }

                //Change destination target
                currentDestination = Random.Range(0, randomSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    public void removeLifePoints(int points)
    {
        this.lifePoints -= points;
        if(this.lifePoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
