using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeHead : CentipedeBody
{
    public CentipedePoint[] points;
    public CentipedePoint nextTarget;
    private CentipedePoint lastTarget;

    private int maxPaths = 20;
    private int ins = 0;
    public CentipedePoint[] pathTraveled;

    private void addPath(CentipedePoint p)
    {
        if (ins >= maxPaths)
        {
            ins = 0;
            pathTraveled[ins] = p;
            ins++;
        }
        else
        {
            pathTraveled[ins] = p;
            ins++;
        }
    }

    public void setHeadAttributes(Transform nPos, CentipedePoint nextTarget)
    {
        this.transform.position = nPos.position;
        this.nextTarget = nextTarget;
        this.lastTarget = nextTarget;
        this.ins = 0;
        addPath(this.nextTarget);
    }


    public override void initCentipedeBody()
    {
        points = currentRoom.centipedePoints;
        int targetIndex = Random.Range(0, points.Length);
        CentipedePoint currentPoint = points[targetIndex];
        transform.position = currentPoint.transform.position;
        lastTarget = currentPoint;
        nextTarget = currentPoint.getRandomPath(lastTarget);
        rotate(nextTarget.transform);
        pathTraveled = new CentipedePoint[maxPaths];
        addPath(nextTarget);
    }

    public override void manageMovement()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, nextTarget.transform.position, speed * Time.fixedDeltaTime));
        if (Vector2.Distance(transform.position, nextTarget.transform.position) < 0.2f || collidingStaticObject)
        {
            if (waitTime < 0)
            {
                CentipedePoint temp = nextTarget;
                nextTarget = nextTarget.getRandomPath(lastTarget);
                lastTarget = temp;
                rotate(nextTarget.transform);
                addPath(nextTarget);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
