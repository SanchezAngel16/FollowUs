using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private float waitTime;
    public float startWaitTime;
    private int lifePoints;
    public Transform firePoint;
    public JellyFish monster;
    public Transform testLaser;

    private float waitLaserTime;
    private float startLaserTime;

    private bool destroy = false;

    private void Start()
    {
        waitTime = startWaitTime;
        lifePoints = 100;

        startLaserTime = 4f;
        waitLaserTime = startLaserTime;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (waitTime < 0)
            {
                if (startLaserTime >= 0)
                {
                    testLaser.gameObject.SetActive(true);
                    shootLaser();
                    startLaserTime -= Time.deltaTime;
                }
                else
                {
                    testLaser.gameObject.SetActive(false);
                    startLaserTime = startWaitTime;
                    waitTime = startWaitTime;
                }
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
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
                testLaser.localScale = new Vector2(2, hitInfo.distance);
                
                
            }else if (tag.Equals("PlayerHitBox"))
            {
                PlayerCollider p = hitInfo.collider.gameObject.GetComponent<PlayerCollider>();
                p.takeDamage(25);
                testLaser.localScale = new Vector2(2, hitInfo.distance);
            }
        }
        else
        {
            Debug.Log("No hit");
            testLaser.localScale = new Vector2(2, hitInfo.distance);
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
                destroy = true;
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        if (destroy)
        {
            monster.shieldsCount--;
            monster.rotationSpeed += 15;
        }
    }
}
