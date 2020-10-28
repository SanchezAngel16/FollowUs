using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : Enemy
{
    public List<List<GameObject>> body;
    private Transform[] pathPositions;

    public override void initEnemy()
    {
        rb = GetComponent<Rigidbody2D>();
        lifePoints = 150;
    }

    public override void move()
    {

    }
}
