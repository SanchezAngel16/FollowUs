using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeHead : CentipedeBody
{
    public CentipedePoint[] points;
    private CentipedePoint lastTarget;

    private bool fixedHead = false;

    public void createFixedHead(Transform nPos)
    {
        points = currentRoom.centipedePoints;
        this.transform.position = nPos.position;
        CentipedePoint nearestPoint = Util.getNearestTarget(transform, points);
        this.nextTarget = nearestPoint.getRandomPath(nearestPoint);
        this.lastTarget = nextTarget;
        rotate(nextTarget.transform);
        fixedHead = true;
    }


    public override void manageCollision(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("PlayerBullet"))
        {
            if (collision.gameObject.GetComponent<Bullet>().hit) return;
            collision.gameObject.GetComponent<Bullet>().hit = true;
            collision.gameObject.SetActive(false);
            if (removeLifePoints(40) <= 0)
            {
                if (lastBody != null)
                {
                    this.lifePoints = 150;

                    GameObject last = lastBody.gameObject;
                    transform.position = last.transform.position;

                    if(lastBody.lastBody != null)
                    {
                        lastBody.lastBody.nextBody = this;
                        lastBody = lastBody.lastBody;
                    }

                    Destroy(last.gameObject);
                }
                else removeEnemy();
            }
        }
    }

    public override void initCentipedeBody()
    {
        if (!fixedHead)
        {
            points = currentRoom.centipedePoints;
            int randomPosition = Random.Range(0, points.Length);
            transform.position = points[randomPosition].transform.position;
            this.nextTarget = points[randomPosition];
            this.lastTarget = nextTarget;
            rotate(nextTarget.transform);
        }
    }

    public override void manageMovement()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, nextTarget.transform.position, (speed * Util.enemiesSpeed) * Time.fixedDeltaTime));
        if (Vector2.Distance(transform.position, nextTarget.transform.position) < 0.2f || collidingStaticObject)
        {
            CentipedePoint temp = nextTarget;
            nextTarget = nextTarget.getRandomPath(lastTarget);
            lastTarget = temp;
            rotate(nextTarget.transform);
        }
    }

    
}
