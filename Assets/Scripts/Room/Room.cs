using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int mapLocation;
    public bool isRoomActive;
    public bool isDestroyed;
    public int[] posibleDirections = new int[4];
    public int threatType;

    public Transform enemiesGenerationPoint;
    public GameObject[] enemiesPrefab;

    public Door rightDoor;
    public Door leftDoor;
    public Door downDoor;
    public Door upDoor;

    public Sprite[] backgroundSprites;
    public Sprite[] doorSprites;

    public GameObject staticElementsGroup;
    public GameObject[] staticElements;

    public Transform[] corners;
    public CentipedePoint[] centipedePoints;

    public float timer;
    public float maxTimer;

    public SpriteRenderer crackEffect;
    public Sprite[] crackSprites;
    private float crackTime;

    private bool firstTime = true;

    public Sprite zombieHole;
    private int[] cornersIndex = new int[2];

    public GameObject emptyRoom;

    private bool firstTimeDestroying;

    private int currentCrackSprite = -1;



    void Awake()
    {
        posibleDirections[0] = 1;
        posibleDirections[1] = 1;
        posibleDirections[2] = 1;
        posibleDirections[3] = 1;
        timer = maxTimer;
        isDestroyed = false;
        firstTimeDestroying = true;
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = backgroundSprites[Random.Range(1, backgroundSprites.Length)];
        crackTime = maxTimer / 5;
    }

    private void OnEnable()
    {
        if (!firstTime)
        {
            generateEnemies();
            staticElementsGroup.SetActive(true);
        }
        else firstTime = false;
    }

    public void setCurseType(int curseType)
    {
        Util.setCurseType(curseType);
        if (curseType == 2)
        {
            maxTimer /= 2f;
            timer = maxTimer;
            crackTime = maxTimer / 5;
        }else if(curseType == 6)
        {
            threatType += 2;
            if (threatType >= 9) threatType = 9;
        }
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer > crackTime * 4) crackEffect.sprite = null;
            else if (timer > crackTime * 3) setCrackSprite(0);
            else if (timer > crackTime * 2) setCrackSprite(1);
            else if (timer > crackTime * 1) setCrackSprite(2);
            else if (timer > 0) setCrackSprite(3);
        }
        else
        {
            if (firstTimeDestroying)
            {
                DestroyRoom();
                firstTimeDestroying = false;
            }
        }
    }

    private void DestroyRoom()
    {
        this.isDestroyed = true;
        setCrackSprite(4);
        enemiesGenerationPoint.gameObject.SetActive(false);
        staticElementsGroup.gameObject.SetActive(false);
    }

    private void setCrackSprite(int index)
    {
        if(currentCrackSprite != index)
        {
            currentCrackSprite = index;
            crackEffect.sprite = crackSprites[index];
        }
    }

    public void generateEnemies()
    {
        int enemiesCount = 0;
        int enemyType = -1;
        switch (threatType)
        {
            case 1:
                enemyType = 0;
                enemiesCount = Random.Range(1, 3);
                break;
            case 2:
                enemyType = 0;
                enemiesCount = Random.Range(3, 5);
                break;
            case 3:
                enemyType = 0;
                enemiesCount = Random.Range(5, 7);
                break;
            case 4:
                enemyType = 1;
                enemiesCount = 1;
                break;
            case 5:
                enemyType = 2;
                enemiesCount = Random.Range(1,4);
                break;
            case 6:
                enemyType = 3;
                enemiesCount = 1;
                break;
            case 7:
                enemyType = 4;
                enemiesCount = 6;
                break;
            case 8:
                enemyType = 5;
                break;
            case 9:
                enemyType = 6;
                enemiesCount = 1;
                break;
        }

        if(enemyType == 4)
        {
            int randPos = Random.Range(0, corners.Length);
            cornersIndex[0] = randPos;
            corners[randPos].GetComponent<SpriteRenderer>().sprite = zombieHole;
            randPos++;
            if (randPos == corners.Length) randPos = 0;
            cornersIndex[1] = randPos;
            corners[randPos].GetComponent<SpriteRenderer>().sprite = zombieHole;
            int randomZombiesCount = Random.Range(12, 30);
            Main.Instance.enemiesCount += randomZombiesCount;
            for (int i = 0; i < randomZombiesCount; i++)
            {
                Invoke("generateZombies", i * 0.7f);
            }
        }else if (enemyType == 5)
        {
            GameObject newCentipede = Instantiate(enemiesPrefab[enemyType], enemiesGenerationPoint);

            Main.Instance.enemies.Add(newCentipede.transform);
        }else if(enemyType == 2)
        {
            for (int i = 0; i < enemiesCount; i++)
            {
                GameObject newEnemy = Instantiate(enemiesPrefab[enemyType], enemiesGenerationPoint);
                Golem g = newEnemy.transform.GetChild(1).GetComponent<Golem>();
                Main.Instance.enemies.Add(newEnemy.transform);
                Main.Instance.enemiesCount++;
            }
        }
        else
        {
            for (int i = 0; i < enemiesCount; i++)
            {
                GameObject newEnemy = Instantiate(enemiesPrefab[enemyType], enemiesGenerationPoint);
                Main.Instance.enemies.Add(newEnemy.transform);
                Main.Instance.enemiesCount++;
            }
        }
        
    }

    private void generateZombies()
    {
        GameObject newZombie = Instantiate(enemiesPrefab[4], enemiesGenerationPoint );
        newZombie.transform.position = corners[cornersIndex[Random.Range(0, cornersIndex.Length)]].transform.position;
        Main.Instance.enemies.Add(newZombie.transform);
    }

    public void generateLightnings()
    {
        GameObject newLightning = Instantiate(EnemyPrefabManager.Instance.lightning, staticElementsGroup.transform);
        Lightning l = newLightning.GetComponent<Lightning>();
        l.transform.position = staticElementsGroup.transform.position;
        //l.setLightningPoints(lightningPoints);
    }

    public void setDoorSprites(int cols, int rows)
    {
        Sprite notPassRight = doorSprites[0];
        Sprite notPassLeft = doorSprites[1];
        Sprite notPassUp = doorSprites[2];
        Sprite notPassDown = doorSprites[3];

        if (mapLocation.x == 0) leftDoor.GetComponent<SpriteRenderer>().sprite = notPassLeft;
        else if (mapLocation.x == cols - 1) rightDoor.GetComponent<SpriteRenderer>().sprite = notPassRight;

        if (mapLocation.y == 0) upDoor.GetComponent<SpriteRenderer>().sprite = notPassUp;
        else if (mapLocation.y == rows - 1) downDoor.GetComponent<SpriteRenderer>().sprite = notPassDown;
    }

    public void setPosibleDirections(int right, int left, int up, int down)
    {
        posibleDirections[0] = right;
        posibleDirections[1] = left;
        posibleDirections[2] = up;
        posibleDirections[3] = down;
    }


    public void generateStaticElements(int numElements)
    {
        if (numElements == 1)
        {
            Instantiate(staticElements[0], staticElementsGroup.transform).transform.position = staticElementsGroup.transform.position;
        }
        else
        {
            GameObject[] staticObjects = new GameObject[numElements];
            float minDistance = 2f;
            Vector2 randomPos;
            List<Vector2> randomPositions = new List<Vector2>();
            randomPositions.Add(Util.getRandomPosition(staticElementsGroup.transform, 1f));

            for(int i = 1; i < numElements; i++)
            {
                do
                {
                    randomPos = Util.getRandomPosition(staticElementsGroup.transform, 1f);
                } while (Util.isInsideMinDistance(minDistance, randomPositions, randomPos));
                randomPositions.Add(randomPos);
            }
            for(int i = 0; i < numElements; i++)
            {
                staticObjects[i] = Instantiate(staticElements[Random.Range(1, staticElements.Length)], staticElementsGroup.transform);
                staticObjects[i].transform.position = randomPositions[i];

            }
        }
    }

    
}
