using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemGenerator : MonoBehaviour
{
    private void Start()
    {
        int golemCount = Random.Range(1, 3);
        for(int i = 0; i < golemCount; i++)
        {
            GameObject newGolem = Instantiate(EnemyPrefabManager.Instance.golem, transform);
            Golem g = newGolem.transform.GetChild(1).GetComponent<Golem>();
            g.setGolemAttributes(i, Util.getCorners(transform.position));
            Main.Instance.enemiesCount++;
        }
    }
}
