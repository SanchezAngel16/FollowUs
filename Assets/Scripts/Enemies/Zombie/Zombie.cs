using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public Transform target;
    public SpriteRenderer sprite;

    public override void initEnemy()
    {
        lifePoints = 150;
        waitTime = startWaitTime;
        speed = Random.Range(1f, 2f);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override void move()
    {
        if(Vector2.Distance(transform.position, target.position) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, (speed * CurseManager.enemiesSpeed) * Time.deltaTime);
            if(transform.position.x < target.position.x)
            {
                sprite.flipX = true;
            } else
            {
                sprite.flipX = false;
            }
        }
    }


}
