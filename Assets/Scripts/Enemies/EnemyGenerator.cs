using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyGenerator : MonoBehaviour
{

    public void generate(int threatType)
    {
        switch (threatType)
        {
            case 1:
                generateEnemies(PrefabManager.Instance.octopus, Random.Range(4, 6));
                break;
            case 2:
                generateEnemies(PrefabManager.Instance.octopus, Random.Range(6, 8));
                generateEnemies(PrefabManager.Instance.golemGenerator, 1);
                break;
            case 3:
                generateEnemies(PrefabManager.Instance.golemGenerator, 1);
                generateEnemies(PrefabManager.Instance.evilEye, Random.Range(1, 2));
                generateEnemies(PrefabManager.Instance.octopus, Random.Range(1, 3));
                break;
            case 4:
                generateEnemies(PrefabManager.Instance.golemGenerator, 1);
                generateEnemies(PrefabManager.Instance.evilEye, Random.Range(3, 4));
                break;
            case 5:
                generateEnemies(PrefabManager.Instance.demon, 1);
                generateEnemies(PrefabManager.Instance.golemGenerator, 1);
                break;
            case 6:
                generateEnemies(PrefabManager.Instance.demon, 1);
                generateEnemies(PrefabManager.Instance.octopus, Random.Range(4, 6));
                generateEnemies(PrefabManager.Instance.evilEye, Random.Range(1, 3));
                break;
            case 7:
                generateEnemies(PrefabManager.Instance.golemGenerator, 1);
                generateEnemies(PrefabManager.Instance.octopus, Random.Range(4, 6));
                //generateEnemies(PrefabManager.Instance.zombieGenerator, 1);
                break;
            case 8:
                //generateEnemies(PrefabManager.Instance.zombieGenerator, 1);
                generateEnemies(PrefabManager.Instance.golemGenerator, 1);
                generateEnemies(PrefabManager.Instance.octopus, Random.Range(1, 4));
                generateEnemies(PrefabManager.Instance.evilEye, Random.Range(1, 2));
                break;
            case 9:
                if(Random.Range(0, 10) > 5)
                {
                    generateEnemies(PrefabManager.Instance.jellyFish, 1);
                }
                else
                {
                    //Centipede
                    generateEnemies(PrefabManager.Instance.centipede, 1);
                }
                break;
        }
    }

    private void generateEnemies(GameObject enemy, int enemiesCount)
    {
        /*GameObject g = PhotonNetwork.Instantiate(PrefabManager.Instance.demon.name, transform.position, Quaternion.identity);

        g.transform.SetParent(transform);
        PhotonNetwork.Instantiate(PrefabManager.Instance.octopus.name, transform.position, Quaternion.identity);
        PhotonNetwork.Instantiate(PrefabManager.Instance.golemGenerator.name, transform.position, Quaternion.identity);
        PhotonNetwork.Instantiate(PrefabManager.Instance.evilEye.name, transform.position, Quaternion.identity);*/
        /*GameObject centipede = PhotonNetwork.Instantiate(PrefabManager.Instance.jellyFish.name, transform.position, Quaternion.identity);
        centipede.transform.SetParent(transform);
        */
        for(int i = 0; i < enemiesCount; i++)
        {
            //Instantiate(enemy, transform).transform.position = transform.position;
            GameObject newEnemy = PhotonNetwork.Instantiate(enemy.name, transform.position, Quaternion.identity);
            newEnemy.transform.SetParent(transform);
        }
    }
}
