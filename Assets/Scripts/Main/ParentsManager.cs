using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentsManager : MonoBehaviour
{
    public Transform currentEnemyParent = null;
    public Transform currentStaticObjectsParent = null;

    private static ParentsManager instance = null;

    public static ParentsManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }
}
