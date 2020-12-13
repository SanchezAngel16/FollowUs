using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(ParentsManager.Instance.currentEnemyParent);
    }
}
