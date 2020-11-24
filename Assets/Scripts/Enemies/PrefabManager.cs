using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    // Assign the prefab in the inspector
    public GameObject centipede;
    public GameObject centipedeHead;
    public GameObject centipedeTail;
    public GameObject golemGenerator;
    public GameObject golem;
    public GameObject demon;
    public GameObject evilEye;
    public GameObject jellyFish;
    public GameObject octopus;
    public GameObject zombieGenerator;
    public GameObject zombie;
    public GameObject evilFairy;
    public GameObject lightning;
    public GameObject lightningGenerator;
    public GameObject explosionEffect;
    //Singleton
    private static PrefabManager m_Instance = null;
    public static PrefabManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (PrefabManager)FindObjectOfType(typeof(PrefabManager));
            }
            return m_Instance;
        }
    }
}
