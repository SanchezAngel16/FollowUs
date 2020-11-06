using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private float waitTime;
    public float startWaitTime;
    private int lifePoints;
    public Transform firePoint;
    public LineRenderer laser;
    public Monster3 monster;

    private float waitLaserTime;
    private float startLaserTime;

    private void Start()
    {
        waitTime = startWaitTime;
        lifePoints = 100;

        startLaserTime = 4f;
        waitLaserTime = startLaserTime;
        laser.sortingLayerName = "Elements";
    }

    private void Update()
    {
        if(waitTime < 0)
        {
            if(startLaserTime >= 0)
            {
                laser.enabled = true;
                shootLaser();
                startLaserTime -= Time.deltaTime;
            }
            else
            {
                laser.enabled = false;
                startLaserTime = startWaitTime;
                waitTime = startWaitTime;
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    private void shootLaser()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        if (hitInfo)
        {
            string tag = hitInfo.collider.tag;
            if (tag.Equals("StaticObject"))
            {
                laser.SetPosition(0, firePoint.localPosition);
                laser.SetPosition(1, new Vector2(hitInfo.distance, 0));
            }else if (tag.Equals("PlayerHitBox"))
            {
                PlayerCollider p = hitInfo.collider.gameObject.GetComponent<PlayerCollider>();
                p.takeDamage(25);
                laser.SetPosition(0, firePoint.localPosition);
                laser.SetPosition(1, new Vector2(hitInfo.distance, 0));
            }
        }
        else
        {
            Debug.Log("No hit");
            laser.SetPosition(0, firePoint.localPosition);
            laser.SetPosition(1, new Vector2(hitInfo.distance + 100, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        if (tag.Equals("PlayerBullet"))
        {
            collision.gameObject.SetActive(false);
            this.lifePoints -= 10;
            if(this.lifePoints <= 0)
            {
                gameObject.SetActive(false);
                monster.shieldsCount--;
            }
        }
    }
}
