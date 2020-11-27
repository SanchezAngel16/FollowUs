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
            if (removeLifePoints(40) <= 0)
            {
                if (lastBody != null)
                {
                    GameObject newHead = Instantiate(PrefabManager.Instance.centipedeHead, transform.parent.parent);
                    GameController.Instance.enemiesCount++;
                    CentipedeHead newCentipedeHead = newHead.transform.GetChild(1).GetComponent<CentipedeHead>();
                    newCentipedeHead.setCentipedeAttributes(0, points);
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
                    last.transform.GetComponent<CentipedeTail>().destroy = true;
                    Destroy(last.gameObject);
                }
                this.destroy = true;
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
                rb.MovePosition(Vector2.MoveTowards(transform.position, nextBody.tail.transform.position, (speed * CurseManager.enemiesSpeed) * Time.deltaTime));
                rotate(nextBody.tail.transform);
            }
        }
        else
        {
            startMoveTime -= Time.deltaTime;
        }
    }
}
