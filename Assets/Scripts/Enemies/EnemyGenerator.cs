using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{

    public void generate(int threatType)
    {
        switch (threatType)
        {
            case 1:
                generateEnemies(EnemyPrefabManager.Instance.octopus, Random.Range(4, 6));
                break;
            case 2:
                generateEnemies(EnemyPrefabManager.Instance.octopus, Random.Range(6, 8));
                generateEnemies(EnemyPrefabManager.Instance.golemGenerator, 1);
                break;
            case 3:
                generateEnemies(EnemyPrefabManager.Instance.golemGenerator, 1);
                generateEnemies(EnemyPrefabManager.Instance.evilEye, Random.Range(1, 2));
                generateEnemies(EnemyPrefabManager.Instance.octopus, Random.Range(1, 3));
                break;
            case 4:
                generateEnemies(EnemyPrefabManager.Instance.golemGenerator, 1);
                generateEnemies(EnemyPrefabManager.Instance.evilEye, Random.Range(3, 4));
                break;
            case 5:
                generateEnemies(EnemyPrefabManager.Instance.demon, 1);
                generateEnemies(EnemyPrefabManager.Instance.golemGenerator, 1);
                break;
            case 6:
                generateEnemies(EnemyPrefabManager.Instance.demon, 1);
                generateEnemies(EnemyPrefabManager.Instance.octopus, Random.Range(4, 6));
                generateEnemies(EnemyPrefabManager.Instance.evilEye, Random.Range(1, 3));
                break;
            case 7:
                //ZOMBIES, GOLEM, OCTOPUS
                generateEnemies(EnemyPrefabManager.Instance.golemGenerator, 1);
                generateEnemies(EnemyPrefabManager.Instance.octopus, Random.Range(4, 6));
                generateEnemies(EnemyPrefabManager.Instance.zombieGenerator, 1);
                break;
            case 8:
                //ZOMBIES, GOLEM, EVILEYE
                generateEnemies(EnemyPrefabManager.Instance.zombieGenerator, 1);
                generateEnemies(EnemyPrefabManager.Instance.golemGenerator, 1);
                generateEnemies(EnemyPrefabManager.Instance.octopus, Random.Range(1, 4));
                generateEnemies(EnemyPrefabManager.Instance.evilEye, Random.Range(1, 2));
                break;
            case 9:
                //CENTIPEDE OR JELLYFISH
                if(Random.Range(0, 10) > 5)
                {
                    generateEnemies(EnemyPrefabManager.Instance.jellyFish, 1);
                }
                else
                {
                    //Centipede
                    generateEnemies(EnemyPrefabManager.Instance.centipede, 1);
                }
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
