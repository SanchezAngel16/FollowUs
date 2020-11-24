using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int mapLocation;
    public bool isRoomActive;
    public bool isDestroyed;
    private bool isMainRoom;
    public int[] posibleDirections = new int[4];

    public int threatType;
    public int roomType;
    private int staticElementsType;

    public Transform enemiesGenerationPoint;
    public GameObject[] enemiesPrefab;

    public Door rightDoor;
    public Door leftDoor;
    public Door downDoor;
    public Door upDoor;

    public Sprite[] backgroundSprites;
    public Sprite[] doorSprites;


    public float timer;
    public float maxTimer;

    public SpriteRenderer crackEffect;
    public Sprite[] crackSprites;
    private float crackTime;

    private bool firstTime = true;


    public GameObject emptyRoom;

    private bool firstTimeDestroying;

    private int currentCrackSprite = -1;

    [SerializeField]
    private EnemyGenerator enemyGenerator = null;
    [SerializeField]
    private StaticObjectsManager staticObjectsGenerator = null;

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
            generateStaticElements(Random.Range(0, 2));
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
        staticObjectsGenerator.gameObject.SetActive(false);
    }

    private void setCrackSprite(int index)
    {
        if(currentCrackSprite != index)
        {
            currentCrackSprite = index;
            crackEffect.sprite = crackSprites[index];
        }
    }

    public void setRoomType(int threatType, int roomType)
    {
        this.threatType = threatType;
        this.roomType = roomType;
        if (roomType == (int)Util.RoomType.MAIN) isMainRoom = true;
    }

    private void generateEnemies()
    {
        enemyGenerator.generate(threatType);
    }

    private void generateStaticElements(int type)
    {
        if (isMainRoom)
        {
            staticObjectsGenerator.generate(1, isMainRoom);
        }
        else
        {
            if(threatType != 9 && threatType != 0)
            {
                staticObjectsGenerator.generate(type, isMainRoom);
            }
        }
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

    
}
