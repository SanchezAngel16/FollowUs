using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static int playableArea = 4;

    public static Vector2 getRandomPosition(Transform parent)
    {
        float minX = parent.position.x - playableArea;
        float maxX = parent.position.x + playableArea;
        float minY = parent.position.y - playableArea;
        float maxY = parent.position.y + playableArea;

        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }


}
