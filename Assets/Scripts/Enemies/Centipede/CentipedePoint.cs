using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedePoint : MonoBehaviour
{
    public CentipedePoint[] possibleWays;

    public CentipedePoint getRandomPath(CentipedePoint t)
    {
        CentipedePoint randomPoint;
        do
        {
            randomPoint = possibleWays[Random.Range(0, possibleWays.Length)];
        } while (randomPoint.Equals(t));
        return randomPoint;
    }
}
