using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeTail : CentipedeBody
{
    public CentipedeHead currentHead;
    public CentipedePoint nextTarget;

    private int maxSizePaths;
    private int currentTargetIndex;

    public GameObject centipedeHead;

    public override void initCentipedeBody()
    {
        currentTargetIndex = 0;
        nextTarget = currentHead.pathTraveled[currentTargetIndex];
        maxSizePaths = currentHead.pathTraveled.Length;
        transform.position = currentHead.transform.position;
    }

    public override void manageMovement()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, nextTarget.transform.position, speed * Time.fixedDeltaTime));
        if (Vector2.Distance(transform.position, nextTarget.transform.position) < 0.2f || collidingStaticObject)
        {
            if (waitTime < 0)
            {
                currentTargetIndex++;
                if (currentTargetIndex >= maxSizePaths) currentTargetIndex = 0;
                nextTarget = currentHead.pathTraveled[currentTargetIndex];
                rotate(nextTarget.transform);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("PlayerBullet"))
        {
            if (removeLifePoints(40) <= 0)
            {
                if (lastBody != null)
                {
                    GameObject newHead = Instantiate(centipedeHead, transform.parent);
                    CentipedeHead newCentipedeHead = newHead.GetComponent<CentipedeHead>();
                    newCentipedeHead.setHeadAttributes(lastBody.transform,((CentipedeTail)lastBody).nextTarget);
                    while(lastBody.lastBody != null)
                    {
                        ((CentipedeTail)lastBody.lastBody).currentHead = newCentipedeHead;
                        ((CentipedeTail)lastBody.lastBody).currentTargetIndex = 0;
                        lastBody = lastBody.lastBody;
                    }
                    Destroy(lastBody);
                }
            }
            collision.gameObject.SetActive(false);
        }
    }
}
