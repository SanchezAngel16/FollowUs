using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Main : MonoBehaviour
{
    private const int RIGHT_DIR = 0;
    private const int LEFT_DIR = 1;
    private const int UP_DIR = 2;
    private const int DOWN_DIR = 3;

    public GameObject gameObjectMap;
    Map mapController;
    public GameObject roomPrefab;

    Vector2 roomSize;

    public Button right;
    public Button left;
    public Button up;
    public Button down;

    public TextMeshProUGUI gameOverText;

    public Vector2Int currentActiveRoom = new Vector2Int(0, 0);

    public static List<Transform> enemies = new List<Transform>();
    public static bool runningOnPC;

    public TextMeshProUGUI timerText;

    //public PlayerController playerController;

    private void Awake()
    {
        checkRunningPlatform();
    }

    void Start()
    {
        enemies.Clear();
        roomSize = new Vector2(roomPrefab.GetComponent<Renderer>().bounds.size.x, roomPrefab.GetComponent<Renderer>().bounds.size.y);
        
        mapController = gameObjectMap.GetComponent<Map>();
        mapController.initMapArray();
        createMap(roomSize.x, roomSize.y);
        mapController.map[0, 0].GetComponent<Room>().isRoomActive = true;
        mapController.map[0, 0].SetActive(true);
        mapController.updatePosibleDirections();

        initUI();
    }


    private void checkRunningPlatform()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
            runningOnPC = true;
        #elif UNITY_IOS || UNITY_ANDROID
            runningOnPC = false;
        #endif
    }

    private void initUI()
    {
        gameOverText.gameObject.SetActive(false);
        initializeArrows();
    }

    private void setGoodAndBadRooms()
    {
        mapController.goodRoom = new Vector2Int(Random.Range(1, mapController.cols), Random.Range(1, mapController.rows));
        do
        {
            mapController.badRoom = new Vector2Int(Random.Range(1, mapController.cols), Random.Range(1, mapController.rows));
        } while (Vector2Int.Equals(mapController.goodRoom, mapController.badRoom));
    }

    private void createMap(float wRoom, float hRoom)
    {
        Color rColor;

        setGoodAndBadRooms();

        int enemyType = 1;

        for (int row = 0; row < mapController.rows; row++)
        {
            for (int col = 0; col < mapController.cols; col++)
            {
                GameObject newRoom = (GameObject)Instantiate(roomPrefab);
                Room roomScript = newRoom.GetComponent<Room>();
                newRoom.transform.parent = gameObjectMap.transform;

                float posX = col * wRoom;
                float posY = row * -hRoom;

                newRoom.transform.position = new Vector2(posX, posY);

                
                if (col == 0 && row == 0)
                {
                    //roomScript.enemiesCount = 0;
                    roomScript.generateStaticElements(1);
                }
                else
                {
                    //roomScript.threatType = Random.Range(1,8);
                    roomScript.threatType = enemyType++;
                    if (enemyType == 8) enemyType = 1;
                    //roomScript.generateStaticElements(Random.Range(2, 3));
                }

                if (col == mapController.goodRoom.x && row == mapController.goodRoom.y)
                {
                    rColor = Color.blue;
                    roomScript.threatType = 0;
                }
                else if (col == mapController.badRoom.x && row == mapController.badRoom.y) rColor = Color.red;
                else rColor = Color.white;

                roomScript.mapLocation = new Vector2Int(col, row);
                newRoom.GetComponent<SpriteRenderer>().color = rColor;
                
                newRoom.SetActive(false);

                roomScript.isRoomActive = false;
                roomScript.setDoorSprites(mapController.cols, mapController.rows);
                mapController.map[col, row] = newRoom;
            }
        }
    }

    private void initializeArrows()
    {
        right.onClick.AddListener(() => desactivateDoor(1, 0, RIGHT_DIR));
        left.onClick.AddListener(() => desactivateDoor(-1, 0, LEFT_DIR));
        down.onClick.AddListener(() => desactivateDoor(0, 1, DOWN_DIR));
        up.onClick.AddListener(() => desactivateDoor(0, -1, UP_DIR));
        updateUIArrows();
    }

    private void desactivateDoor(int x, int y, int direction)
    {
        int doorToDeactivate = 0;
        switch (direction)
        {
            case RIGHT_DIR:
                mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>().right.gameObject.SetActive(false);
                doorToDeactivate = LEFT_DIR;
                break;
            case LEFT_DIR:
                mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>().left.gameObject.SetActive(false);
                doorToDeactivate = RIGHT_DIR;
                break;
            case UP_DIR:
                mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>().up.gameObject.SetActive(false);
                doorToDeactivate = DOWN_DIR;
                break;
            case DOWN_DIR:
                mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>().down.gameObject.SetActive(false);
                doorToDeactivate = UP_DIR;
                break;
        }
        
        currentActiveRoom.x += x;
        currentActiveRoom.y += y;
        mapController.map[currentActiveRoom.x, currentActiveRoom.y].SetActive(true);
        mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>().isRoomActive = true;

        switch (doorToDeactivate)
        {
            case RIGHT_DIR:
                mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>().right.gameObject.SetActive(false);
                break;
            case LEFT_DIR:
                mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>().left.gameObject.SetActive(false);
                break;
            case UP_DIR:
                mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>().up.gameObject.SetActive(false);
                break;
            case DOWN_DIR:
                mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>().down.gameObject.SetActive(false);
                break;
        }

        if (Vector2Int.Equals(currentActiveRoom, mapController.goodRoom))
        {
            setGameOverText(true, "You win!");
        }else if (Vector2Int.Equals(currentActiveRoom, mapController.badRoom))
        {
            setGameOverText(true, "You lose!");
        }
        else
        {
            setGameOverText(false, "");
        }

        mapController.updatePosibleDirections();
        setActiveAllUIArrows(false);
    }

    private void setGameOverText(bool active, string text)
    {
        gameOverText.gameObject.SetActive(active);
        gameOverText.text = text;
    }

    public void updateUIArrows()
    {
        setActiveAllUIArrows(true);
        Room activeRoom = gameObjectMap.GetComponent<Map>().map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>();
        if (activeRoom.posibleDirections[0] == 0) right.gameObject.SetActive(false);
        if (activeRoom.posibleDirections[1] == 0) left.gameObject.SetActive(false);
        if (activeRoom.posibleDirections[2] == 0) up.gameObject.SetActive(false);
        if (activeRoom.posibleDirections[3] == 0) down.gameObject.SetActive(false);
    }

    public void setActiveAllUIArrows(bool active)
    {
        right.gameObject.SetActive(active);
        left.gameObject.SetActive(active);
        up.gameObject.SetActive(active);
        down.gameObject.SetActive(active);
    }

}
