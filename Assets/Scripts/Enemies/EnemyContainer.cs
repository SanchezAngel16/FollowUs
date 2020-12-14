using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        transform.SetParent(ParentsManager.Instance.currentEnemyParent);
    }
}
