using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{

    public void generate(Room currentRoom, int threatType)
    {
        switch (threatType)
        {
            case 1:
                // OCTOPUS 
                generateEnemies(EnemyPrefabManager.Instance.octopus, Random.Range(4, 6));
                break;
            case 2:
                generateEnemies(EnemyPrefabManager.Instance.octopus, Random.Range(6, 8));
                
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
        }
    }

    private void generateEnemies(GameObject enemy, int enemiesCount)
    {
        for(int i = 0; i < enemiesCount; i++)
        {
            Instantiate(enemy, transform);
        }
    }
}
