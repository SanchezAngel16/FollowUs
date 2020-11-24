using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private const int RIGHT_DIR = 0;
    private const int LEFT_DIR = 1;
    private const int UP_DIR = 2;
    private const int DOWN_DIR = 3;

    public GameObject gameObjectMap;
    public Map mapController;
    public GameObject roomPrefab;

    Vector2 roomSize;

    public Button right;
    public Button left;
    public Button up;
    public Button down;

    public TextMeshProUGUI gameOverText;

    public Vector2Int currentActiveRoom = new Vector2Int(0, 0);

    public int enemiesCount = 0;
    public bool runningOnPC;

    public TextMeshProUGUI timerText;

    public PlayerController playerController;

    public int currentCurseType;
    public Image curseTypeImage;
    public Sprite[] curseTypes;

    public GameObject lightsOut;

    private static Main instance = null;

    public static Main Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {

        if (instance == null)
        {

            instance = this;
            //DontDestroyOnLoad(this.gameObject);

            checkRunningPlatform();
            Application.targetFrameRate = 60;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        initGame();
    }

    private void initGame()
    {
        roomSize = new Vector2(roomPrefab.GetComponent<Renderer>().bounds.size.x, roomPrefab.GetComponent<Renderer>().bounds.size.y);

        mapController = gameObjectMap.GetComponent<Map>();
        mapController.initMapArray();
        createMap(roomSize.x, roomSize.y);
        GameObject startRoom = mapController.map[mapController.startRoom.x, mapController.startRoom.y];
        startRoom.GetComponent<Room>().isRoomActive = true;
        startRoom.SetActive(true);
        Vector2 randStartPos = startRoom.transform.position;
        randStartPos.x = (int)Random.Range(randStartPos.x - 1, randStartPos.x + 1);
        randStartPos.y = (int)Random.Range(randStartPos.y - 1, randStartPos.y - 1);
        playerController.transform.position = randStartPos;
        mapController.updatePosibleDirections();
        enemiesCount = 0;
        currentCurseType = 0;
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

    private void setRoomsType()
    {
        mapController.goodRoom = new Vector2Int(Random.Range(0, mapController.cols), Random.Range(0, mapController.rows));
        do
        {
            mapController.badRoom = new Vector2Int(Random.Range(0, mapController.cols), Random.Range(0, mapController.rows));
        } while (Vector2Int.Equals(mapController.goodRoom, mapController.badRoom));

        do
        {
            mapController.startRoom = new Vector2Int(Random.Range(0, mapController.cols), Random.Range(0, mapController.rows));
        } while (Vector2Int.Equals(mapController.startRoom, mapController.goodRoom) 
        || Vector2Int.Equals(mapController.startRoom, mapController.badRoom));

        currentActiveRoom = mapController.startRoom;
    }

    private void createMap(float wRoom, float hRoom)
    {
        Color rColor;
        setRoomsType();
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

                
                if (col == mapController.startRoom.x && row == mapController.startRoom.y)
                {
                    roomScript.setRoomType(0, (int)Util.RoomType.MAIN);
                    newRoom.SetActive(true);
                }
                else
                {
                    roomScript.threatType = Random.Range(1,10);
                }

                if (col == mapController.goodRoom.x && row == mapController.goodRoom.y)
                {
                    rColor = Color.blue;
                    roomScript.threatType = 0;
                    roomScript.setRoomType(0, (int)Util.RoomType.GOOD);
                }
                else if (col == mapController.badRoom.x && row == mapController.badRoom.y)
                {
                    roomScript.setRoomType(roomScript.threatType, (int)Util.RoomType.BAD);
                    rColor = Color.red;
                }
                else
                {
                    roomScript.setRoomType(roomScript.threatType, (int)Util.RoomType.NORMAL);
                    rColor = Color.white;
                }

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
        Room room = mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>();
        switch (direction)
        {
            case RIGHT_DIR:
                room.rightDoor.openDoor();
                doorToDeactivate = LEFT_DIR;
                break;
            case LEFT_DIR:
                room.leftDoor.openDoor();
                doorToDeactivate = RIGHT_DIR;
                break;
            case UP_DIR:
                room.upDoor.openDoor();
                doorToDeactivate = DOWN_DIR;
                break;
            case DOWN_DIR:
                room.downDoor.openDoor();
                doorToDeactivate = UP_DIR;
                break;
        }
        
        currentActiveRoom.x += x;
        currentActiveRoom.y += y;
        room = mapController.map[currentActiveRoom.x, currentActiveRoom.y].GetComponent<Room>();
        room.setCurseType(this.currentCurseType);
        setCurrentCurseType();
        room.gameObject.SetActive(true);
        room.isRoomActive = true;

        switch (doorToDeactivate)
        {
            case RIGHT_DIR:
                room.rightDoor.openDoor();
                break;
            case LEFT_DIR:
                room.leftDoor.openDoor();
                break;
            case UP_DIR:
                room.upDoor.openDoor();
                break;
            case DOWN_DIR:
                room.downDoor.openDoor();
                break;
        }

        

        mapController.updatePosibleDirections();
        setActiveAllUIArrows(false);
    }

    public void setGameOverText(bool active, string text)
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
        lightsOut.SetActive(false);
    }


    public void setActiveAllUIArrows(bool active)
    {
        right.gameObject.SetActive(active);
        left.gameObject.SetActive(active);
        up.gameObject.SetActive(active);
        down.gameObject.SetActive(active);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(1);
        initGame();
    }

    public void setCurseType(int curse)
    {
        this.currentCurseType = curse;
    }

    private void setCurrentCurseType()
    {
        if (this.currentCurseType == 0) curseTypeImage.gameObject.SetActive(false);
        else
        {
            curseTypeImage.gameObject.SetActive(true);
            curseTypeImage.sprite = curseTypes[this.currentCurseType - 1];
        }

        //Freeze bullet type
        if (this.currentCurseType == 4) lightsOut.gameObject.SetActive(true);
        else lightsOut.gameObject.SetActive(false);

        this.currentCurseType = 0;
    }
}
