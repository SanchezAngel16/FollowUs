using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool hit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("StaticObject") || collision.gameObject.CompareTag("Door")) activate(false);
        else if ((collision.gameObject.CompareTag("StaticObjectEnemy") ||
            collision.gameObject.CompareTag("LightningLaser")) && this.tag.Equals("PlayerBullet")) activate(false);
    }

    public void activate(bool active)
    {
        gameObject.SetActive(active);
    }

    private void OnEnable()
    {
        hit = false;
    }
}
