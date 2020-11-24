using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    
    public static Vector2 rightDirection = new Vector2(1, 0);
    public static Vector2 leftDirection = new Vector2(-1, 0);
    public static Vector2 UpDirection = new Vector2(0, 1);
    public static Vector2 DownDirection = new Vector2(0, -1);

    public const int RIGHT_DIR = 0;
    public const int LEFT_DIR = 1;
    public const int UP_DIR = 2;
    public const int DOWN_DIR = 3;

    public static int mapSize = 5;

    public static int playableArea = 4;

    public enum RoomType
    {
        GOOD = 0, BAD = 1, NORMAL = 2, MAIN = 3
    }

    public static Vector2 getRandomPosition(Transform parent, float substractOffset)
    {
        float minX = parent.position.x - playableArea + substractOffset;
        float maxX = parent.position.x + playableArea - substractOffset;
        float minY = parent.position.y - playableArea + substractOffset;
        float maxY = parent.position.y + playableArea - substractOffset;

        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    public static bool isInsideMinDistance(float minDistance, List<Vector2> positions, Vector2 newPos)
    {
        foreach (Vector2 pos in positions)
        {
            if (Vector2.Distance(pos, newPos) < minDistance) return true;
        }
        return false;
    }

    public static Vector2[] getCorners(Vector2 currentPos)
    {
        Vector2[] corners = new Vector2[4];

        corners[0] = new Vector2(currentPos.x + 4f, currentPos.y - 3.5f);
        corners[1] = new Vector2(currentPos.x + 4f, currentPos.y + 3.5f);
        corners[2] = new Vector2(currentPos.x - 4f, currentPos.y + 3.5f);
        corners[3] = new Vector2(currentPos.x - 4f, currentPos.y - 3.5f);

        return corners;
    }

    public static bool compareTags(List<string> tags, GameObject gameObject)
    {
        foreach(string tag in tags)
        {
            if (gameObject.CompareTag(tag)) return true;
        }
        return false;
    }

    public static CentipedePoint getNearestTarget(Transform currentPosition, CentipedePoint[] elements)
    {
        CentipedePoint nearestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (CentipedePoint potentialTarget in elements)
        {
            if (!potentialTarget.gameObject.activeInHierarchy) break;
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestTarget = potentialTarget;
            }
        }

        return nearestTarget;
    }
}
