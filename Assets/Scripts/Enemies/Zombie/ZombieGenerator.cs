using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    private Vector2[] corners = new Vector2[4];
    public GameObject zombieHole;

    // Start is called before the first frame update
    void Start()
    {
        corners = Util.getCorners(transform.position);

        createZombieHoles();

        int randomZombiesCount = Random.Range(12, 30);
        GameController.Instance.enemiesCount += randomZombiesCount;
        for (int i = 0; i < randomZombiesCount; i++)
        {
            Invoke("generateZombies", i * 0.7f);
        }
    }

    private void generateZombies()
    {
        GameObject newZombie = Instantiate(PrefabManager.Instance.zombie, transform);
        newZombie.transform.position = corners[Random.Range(0,corners.Length)];
    }

    private void createZombieHoles()
    {
        for(int i = 0; i < corners.Length; i++)
        {
            ((GameObject)Instantiate(zombieHole, transform)).transform.position = corners[i];
        }
    }
}
