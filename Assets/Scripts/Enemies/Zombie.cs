using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public Transform target;

    public override void initEnemy()
    {
        rb = GetComponent<Rigidbody2D>();
        lifePoints = 150;
        waitTime = startWaitTime;
        speed = Random.Range(1f, 2f);
        //transform.position = Util.getRandomPosition(transform.parent, 0);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override void move()
    {
        if(Vector2.Distance(transform.position, target.position) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
