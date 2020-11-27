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
                GameObject g = Instantiate(staticElements[Random.Range(1, staticElements.Length)], transform);
                g.transform.position = Util.getValidRandomPosition(g, transform, tagsToAvoid, 1.2f);

            }
        }
    }
}
