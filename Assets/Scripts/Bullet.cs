using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colliderTag = collision.gameObject.tag;
        if(bulletType == 0)
        {
            if (!(colliderTag.Equals("Player") || colliderTag.Equals("Room") || colliderTag.Equals("Bullet")))
            {
                if (colliderTag.Equals("Enemy"))
                {
                    collision.gameObject.GetComponent<Enemy>().removeLifePoints(10);
                }
                setActive(false);
            }
        }else if(bulletType == 1)
        {
            if(!(colliderTag.Equals("Enemy") || colliderTag.Equals("Room") || colliderTag.Equals("Bullet")))
            {
                Debug.Log("Hit: " + colliderTag);
                if (colliderTag.Equals("Player"))
                {
                    collision.gameObject.GetComponent<PlayerController>().removeLifePoints(1);
                }
                setActive(false);
            }
        }
    }

    private void setActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
