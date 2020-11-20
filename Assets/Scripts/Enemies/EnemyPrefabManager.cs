using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefabManager : MonoBehaviour
{
    // Assign the prefab in the inspector
    public GameObject centipedeHead;
    public GameObject evilFairy;
    //Singleton
    private static EnemyPrefabManager m_Instance = null;
    public static EnemyPrefabManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (EnemyPrefabManager)FindObjectOfType(typeof(EnemyPrefabManager));
            }
            return m_Instance;
        }
    }
}
