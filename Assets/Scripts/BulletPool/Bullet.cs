using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("StaticObject")) setActive(false);
    }

    private void setActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
