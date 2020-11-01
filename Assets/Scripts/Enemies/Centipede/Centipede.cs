using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    public CentipedeBody[] body;
    public Room currentRoom;
    public GameObject centipedeHead;
    public GameObject centipedeBody;
    private CentipedeHead firstHead;
    

    public void initCentipede(Room r)
    {
        currentRoom = r;
        body = new CentipedeBody[12];
        firstHead = null;
        for(int i = 0; i < body.Length; i++)
        {
            if (i == 0)
            {
                GameObject newCentipedeHead = Instantiate(centipedeHead, transform);
                firstHead = newCentipedeHead.transform.GetChild(1).GetComponent<CentipedeHead>();
                body[i] = firstHead;
                body[i].setCentipedeAttributes(i, currentRoom);
                body[i].nextBody = null;
                body[i].lastBody = body[i + 1];
            }
            else
            {
                GameObject newCentipedeTail = Instantiate(centipedeBody, transform);
                body[i] = newCentipedeTail.transform.GetChild(1).GetComponent<CentipedeTail>();
                newCentipedeTail.transform.GetChild(1).GetComponent<CentipedeTail>().currentHead = firstHead;
                body[i].setCentipedeAttributes(i, currentRoom);
                if (i == body.Length - 1) body[i].lastBody = null;
                else body[i].lastBody = body[i + 1];
                body[i].nextBody = body[i - 1];
            }
            Main.Instance.enemies.Add(body[i].transform);
        }
    }
}
