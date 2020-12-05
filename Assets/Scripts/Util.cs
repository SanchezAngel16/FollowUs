using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    
    public static Vector2Int rightDirection = new Vector2Int(1, 0);
    public static Vector2Int leftDirection = new Vector2Int(-1, 0);
    public static Vector2Int UpDirection = new Vector2Int(0, 1);
    public static Vector2Int DownDirection = new Vector2Int(0, -1);

    public const int RIGHT_DIR = 0;
    public const int LEFT_DIR = 1;
    public const int UP_DIR = 2;
    public const int DOWN_DIR = 3;

    public static int mapSize = 5;

    public static int playableArea = 4;

    public static int maxRoomTime = 180;

    public const int GoodRoom = 0;
    public const int BadRoom = 1;
    public const int NormalRoom = 2;
    public const int MainRoom = 3;

    private static string[] enemiesTags = new string[]
    {
        "Enemy",
        "EnemyBullet",
        "Laser",
        "EnemyShield",
        "LightningLaser"
    };

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


    public static Vector2 getValidRandomPosition(GameObject g, Transform parent, List<string> tags, float offset)
    {
        Collider2D objectCollider = g.GetComponent<Collider2D>();

        Vector3 newPosition = Vector3.zero;
        bool validPosition = false;

        do
        {
            newPosition = getRandomPosition(parent, offset);

            Vector2 min = newPosition - objectCollider.bounds.extents;
            Vector2 max = newPosition + objectCollider.bounds.extents;


            Collider2D[] overlapObjects = Physics2D.OverlapAreaAll(min, max);

            bool collidingStaticObject = false;
            foreach (Collider2D col in overlapObjects)
            {
                if (compareTags(col.gameObject)) collidingStaticObject = true;
            }

            if (!collidingStaticObject) validPosition = true;
            
        } while (!validPosition);

        return newPosition;
    }

    public static Vector2[] getCorners(Vector2 currentPos)
    {
        Vector2[] corners = new Vector2[4];

        corners[0] = new Vector2(currentPos.x + 4f, currentPos.y - 3.3f);
        corners[1] = new Vector2(currentPos.x + 4f, currentPos.y + 3.3f);
        corners[2] = new Vector2(currentPos.x - 4f, currentPos.y + 3.3f);
        corners[3] = new Vector2(currentPos.x - 4f, currentPos.y - 3.3f);

        return corners;
    }

    public static bool compareTags(GameObject gameObject)
    {
        for(int i = 0; i < enemiesTags.Length; i++)
        {
            if (gameObject.CompareTag(enemiesTags[i])) return true;
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

    public static object[] serializeRoomObject(Room r)
    {
        object[] room = new object[4];
        room[0] = r.roomType;
        room[1] = r.threatType;
        room[2] = r.mapLocation.x;
        room[3] = r.mapLocation.y;

        return room;
    }
}
