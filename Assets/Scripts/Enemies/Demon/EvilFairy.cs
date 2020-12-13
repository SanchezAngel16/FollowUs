using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilFairy : Enemy
{
    private Vector2 targetPosition;
    private Transform parent;
    public override void initEnemy()
    {
        GameController.Instance.enemiesCount++;
        lifePoints = 150;
        parent = transform.parent;
        lootMaker = false;

        targetPosition = Util.getRandomPosition(transform.parent, 0);
        transform.SetParent(ParentsManager.Instance.currentEnemyParent);
    }

    public override void move()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, (speed * CurseManager.enemiesSpeed) * Time.deltaTime));
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f || collidingStaticObject)
        {
            targetPosition = Util.getRandomPosition(transform.parent, 0);
        }
    }
}
