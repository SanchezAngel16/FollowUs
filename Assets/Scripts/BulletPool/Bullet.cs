using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("StaticObject")) activate(false);
        else if (tag.Equals("StaticObjectEnemy") && this.tag.Equals("PlayerBullet")) activate(false);
    }

    public void activate(bool active)
    {
        gameObject.SetActive(active);
    }
}
