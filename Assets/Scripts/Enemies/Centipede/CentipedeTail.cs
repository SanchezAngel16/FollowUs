using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeTail : CentipedeBody
{
    public CentipedeHead currentHead;
    public int currentTargetIndex;

    public override void manageCollision(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("PlayerBullet"))
        {
            if (collision.gameObject.GetComponent<Bullet>().hit) return;
            collision.gameObject.GetComponent<Bullet>().hit = true;
            collision.gameObject.SetActive(false);
            if (removeLifePoints(300) <= 0)
            {
                if (lastBody != null)
                {
                    GameObject newHead = Instantiate(EnemyPrefabManager.Instance.centipedeHead, transform.parent.parent);
                    CentipedeHead newCentipedeHead = newHead.transform.GetChild(1).GetComponent<CentipedeHead>();
                    newCentipedeHead.setCentipedeAttributes(0, currentRoom);
                    newCentipedeHead.createFixedHead(lastBody.transform);
                    newCentipedeHead.lastBody = lastBody.lastBody;

                    GameObject last = lastBody.gameObject;
                    CentipedeBody temp = lastBody;

                    if(temp.lastBody != null)
                    {
                        temp.lastBody.nextBody = newCentipedeHead;
                        temp = temp.lastBody;
                        while (temp.lastBody != null)
                        {
                            temp.lastBody.nextBody = temp;
                            temp = temp.lastBody;
                        }
                    }

                    Destroy(last.gameObject);
                }
                removeEnemy();
            }
        }
    }

    public override void initCentipedeBody()
    {
        transform.position = currentHead.transform.position;
    }

    public override void manageMovement()
    {
        if (startMoveTime < 0)
        {
            if (nextBody != null)
            {
                rb.MovePosition(Vector2.MoveTowards(transform.position, nextBody.tail.transform.position, speed * Time.fixedDeltaTime));
                rotate(nextBody.tail.transform);
            }
            /*if (Vector2.Distance(transform.position, nextTarget.transform.position) < 0.2f || collidingStaticObject)
            {
                currentPoint = nextTarget;
                currentTargetIndex++;
                if (currentTargetIndex >= maxSizePaths) currentTargetIndex = 0;
                nextTarget = currentHead.pointsToTravel[currentTargetIndex];
                rotate(nextTarget.transform);
            }*/
        }
        else
        {
            startMoveTime -= Time.fixedDeltaTime;
        }
    }
}
