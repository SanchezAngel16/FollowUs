using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFish : Enemy
{
    private float waitShootTime;
    public float startWaitShootTime;

    public GameObject rotatingObjects;
    public int shieldsCount;
    public float rotationSpeed;

    public override void initEnemy()
    {
        Main.Instance.enemiesCount++;
        lifePoints = 150;
        rotationSpeed = 45;
        startWaitShootTime = Random.Range(2, 4);
        waitShootTime = startWaitShootTime;
        this.transform.position = this.transform.parent.position;
        shieldsCount = 4;
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
            angle += 30f;
        }
    }

    public override void move()
    {
        rotatingObjects.transform.Rotate(0, 0, (rotationSpeed * Util.rotationSpeed) * Time.deltaTime);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("PlayerBullet") && shieldsCount <= 0)
        {
            if (removeLifePoints(40) <= 0)
            {
                this.destroy = true;
                removeEnemy();
            }
            collision.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (destroy)
        {
            rotatingObjects.SetActive(false);

            if (Random.Range(0, 100) >= 10 && lootMaker)
            {
                GameObject collectable = Instantiate(collectables[Random.Range(0, collectables.Length)]);
                collectable.transform.position = transform.position;
            }
            Main.Instance.enemiesCount--;
            if (Main.Instance.enemiesCount <= 0)
            {
                Main.Instance.updateUIArrows();
            }
        }
    }
}
