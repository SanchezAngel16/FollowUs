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

    public Map mapController;


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
            checkRunningPlatform();
            Application.targetFrameRate = 60;
        }
        else Destroy(this);
    }

    void Start()
    {
        initGame();
    }

    private void initGame()
    {
        Room startRoom = mapController.map[mapController.startRoom.x, mapController.startRoom.y];
        startRoom.isRoomActive = true;
        startRoom.gameObject.SetActive(true);

        Vector2 randStartPos = startRoom.transform.position;
        randStartPos.x = (int)Random.Range(randStartPos.x - 1, randStartPos.x + 1);
        randStartPos.y = (int)Random.Range(randStartPos.y - 1, randStartPos.y - 1);

        playerController.transform.position = randStartPos;

        currentActiveRoom = mapController.startRoom;

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

    private void initializeArrows()
    {
        right.onClick.AddListener(() => openDoor(1, 0, RIGHT_DIR));
        left.onClick.AddListener(() => openDoor(-1, 0, LEFT_DIR));
        down.onClick.AddListener(() => openDoor(0, 1, DOWN_DIR));
        up.onClick.AddListener(() => openDoor(0, -1, UP_DIR));
        updateUIArrows();
    }

    private void openDoor(int x, int y, int direction)
    {
        mapController.desactivateDoor(x, y, direction, currentActiveRoom, currentCurseType);

        currentActiveRoom.x += x;
        currentActiveRoom.y += y;
        
        setActiveAllUIArrows(false);
        setCurrentCurseType();
    }

    public void setGameOverText(bool active, string text)
    {
        gameOverText.gameObject.SetActive(active);
        gameOverText.text = text;
    }

    public void updateUIArrows()
    {
        setActiveAllUIArrows(true);
        Room activeRoom = mapController.map[currentActiveRoom.x, currentActiveRoom.y];
        if (activeRoom.isOpen(Util.RIGHT_DIR)) right.gameObject.SetActive(false);
        if (activeRoom.isOpen(Util.LEFT_DIR)) left.gameObject.SetActive(false);
        if (activeRoom.isOpen(Util.UP_DIR)) up.gameObject.SetActive(false);
        if (activeRoom.isOpen(Util.DOWN_DIR)) down.gameObject.SetActive(false);
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
        if (this.currentCurseType == 4) lightsOut.gameObject.SetActive(true);
        else lightsOut.gameObject.SetActive(false);

        this.currentCurseType = 0;
    }
}
