using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectsManager : MonoBehaviour
{
    public GameObject[] staticElements;

    public void generate(int staticObjectType, bool isMainRoom)
    {

        switch (staticObjectType)
        {
            case 0:
                generateLightningsGenerator();
                break;
            case 1:
                generateStaticObjects(isMainRoom);
                break;
        }
    }

    private void generateLightningsGenerator()
    {
        Instantiate(PrefabManager.Instance.lightning, transform).transform.position = transform.position;
        //GameObject lightning = PhotonNetwork.Instantiate(PrefabManager.Instance.lightning.name, transform.position, Quaternion.identity);
        //lightning.transform.SetParent(transform);
    }

    private void generateStaticObjects(bool mainRoom)
    {
        if (mainRoom)
        {
            Instantiate(staticElements[0], transform).transform.position = transform.position;
        }
        else
        {
            int numElements = Random.Range(3, 4);
            List<string> tagsToAvoid = new List<string>();
            tagsToAvoid.Add("StaticObject");
            tagsToAvoid.Add("Enemy");

            for (int i = 0; i < numElements; i++)
            {
                int randType = Random.Range(1, staticElements.Length);
                GameObject g = Instantiate(staticElements[randType], transform);
                Vector2 newPos = Util.getValidRandomPosition(g, transform, tagsToAvoid, 1.2f);
                g.transform.position = newPos;
            }
        }
    }
    
    /*
    #region Pun Calls
       
    [PunRPC]
    public void instantiateObject(float xPos, float yPos, int type)
    {
        GameObject g = Instantiate(staticElements[type], transform);
        g.transform.position = new Vector2(xPos, yPos);
    }

    [PunRPC]
    public void setCurrentStaticObjectsParent()
    {

        ParentsManager.Instance.currentStaticObjectsParent = this.transform;
    }

    #endregion
    */
}
