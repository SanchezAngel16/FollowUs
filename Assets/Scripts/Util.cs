using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static int playableArea = 4;

    public static Vector2 getRandomPosition(Transform parent, float substractOffset)
    {
        float minX = parent.position.x - playableArea + substractOffset;
        float maxX = parent.position.x + playableArea - substractOffset;
        float minY = parent.position.y - playableArea + substractOffset;
        float maxY = parent.position.y + playableArea - substractOffset;

        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    public static Vector3 getOneRandomSidePosition(Transform parent)
    {
        Vector2 parentPosition = parent.transform.position;
        Vector3[] posiblePositions =
        {
            new Vector3(parentPosition.x, parentPosition.y + playableArea, 0),
            new Vector3(parentPosition.x, parentPosition.y - playableArea, 1),
            new Vector3(parentPosition.x + playableArea, parentPosition.y, 2),
            new Vector3(parentPosition.x - playableArea, parentPosition.y, 3),
        };
        return posiblePositions[Random.Range(0, posiblePositions.Length)];
    }
}
