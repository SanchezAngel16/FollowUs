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
            GameObject[] staticObjects = new GameObject[numElements];
            float minDistance = 2f;
            Vector2 randomPos;
            List<Vector2> randomPositions = new List<Vector2>();
            randomPositions.Add(Util.getRandomPosition(transform, 1f));

            for (int i = 1; i < numElements; i++)
            {
                do
                {
                    randomPos = Util.getRandomPosition(transform, 1f);
                } while (Util.isInsideMinDistance(minDistance, randomPositions, randomPos));
                randomPositions.Add(randomPos);
            }
            for (int i = 0; i < numElements; i++)
            {
                staticObjects[i] = Instantiate(staticElements[Random.Range(1, staticElements.Length)], transform);
                staticObjects[i].transform.position = randomPositions[i];

            }
        }
    }
}
