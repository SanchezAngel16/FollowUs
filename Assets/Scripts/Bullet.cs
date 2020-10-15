using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float TimeToLive = 5f;
    private void Start()
    {
        //Destroy(gameObject, TimeToLive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("Room")))
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
