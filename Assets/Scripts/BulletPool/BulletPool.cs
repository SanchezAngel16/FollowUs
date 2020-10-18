using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{

    public GameObject pooledBullet;
    private bool notEnoughBulletsInPool = true;

    private List<GameObject> bullets;

    private void Start()
    {
        bullets = new List<GameObject>();
    }

    public GameObject getBullet()
    {
        if(bullets.Count > 0)
        {
            for(int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    return bullets[i];
                }
            }
        }

        if (notEnoughBulletsInPool)
        {
            GameObject bul = Instantiate(pooledBullet);
            bul.transform.SetParent(gameObject.transform);
            bul.SetActive(false);
            bullets.Add(bul);
            return bul;
        }

        return null;
    }

}
