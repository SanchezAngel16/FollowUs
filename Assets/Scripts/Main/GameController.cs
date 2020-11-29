using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPunCallbacks
{
    public Map mapController;

    [SerializeField]
    private Button right = null, left = null, up = null, down = null;
    [SerializeField]
    private TextMeshProUGUI gameOverText = null;

    private Vector2Int currentActiveRoom = new Vector2Int(0, 0);

    public int enemiesCount = 0;
    public bool runningOnPC;

    private static GameController instance = null;

    [SerializeField]
    private GameObject playerPrefab = null;

    private Room startRoom;
    
    [SerializeField]
    private Room waitingRoom = null;
    [SerializeField]
    private Button startButton = null;
    [SerializeField]
    private PhotonView playerPhotonView;

    private Vector2 startPos;

    public static GameController Instance
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
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startButton.gameObject.SetActive(true);
                setGame();
                Room start = mapController.map[mapController.startRoom.x, mapController.startRoom.y];
                PhotonNetwork.Instantiate(playerPrefab.name, start.transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log(Util.mapSize);
                PlayerComponents.Instance.mainCamera.gameObject.SetActive(true);
                PlayerComponents.Instance.mainCamera.transform.position = new Vector2(-10, 10);
                Invoke("instantiatePlayer", 1.5f);
            }
        }
    }

    private void instantiatePlayer()
    {
        Room start = mapController.map[mapController.startRoom.x, mapController.startRoom.y];
        start.isRoomActive = true;
        start.gameObject.SetActive(true);
        PlayerComponents.Instance.mainCamera.gameObject.SetActive(false);
        PhotonNetwork.Instantiate(playerPrefab.name, start.transform.position, Quaternion.identity);
    }

    private void setGame()
    {
        mapController.initMap();
        startRoom = mapController.map[mapController.startRoom.x, mapController.startRoom.y];
        startRoom.isRoomActive = true;
        startRoom.gameObject.SetActive(true);

        /*Vector2 randStartPos = startRoom.transform.position;
        randStartPos.x = (int)Random.Range(randStartPos.x - 1, randStartPos.x + 1);
        randStartPos.y = (int)Random.Range(randStartPos.y - 1, randStartPos.y - 1);*/

        //playerController.transform.position = randStartPos;

        currentActiveRoom = mapController.startRoom;
        enemiesCount = 0;
        initUI();
    }

    public void initGame()
    {
        startRoom.GetComponent<PhotonView>().RPC("startMainRoom", RpcTarget.All);
        startButton.gameObject.SetActive(false);
        /*Vector3 startRoomPos = startRoom.transform.position;
        playerPhotonView.RPC("setStartPosition", RpcTarget.AllBuffered, startRoomPos);*/
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
        right.onClick.AddListener(() => openDoor(1, 0, Util.RIGHT_DIR));
        left.onClick.AddListener(() => openDoor(-1, 0, Util.LEFT_DIR));
        down.onClick.AddListener(() => openDoor(0, 1, Util.DOWN_DIR));
        up.onClick.AddListener(() => openDoor(0, -1, Util.UP_DIR));
        updateUIArrows();
    }

    private void openDoor(int x, int y, int direction)
    {
        mapController.desactivateDoor(x, y, direction, currentActiveRoom);

        currentActiveRoom.x += x;
        currentActiveRoom.y += y;
        
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
        Room activeRoom = mapController.map[currentActiveRoom.x, currentActiveRoom.y];
        if (activeRoom.isOpen(Util.RIGHT_DIR)) right.gameObject.SetActive(false);
        if (activeRoom.isOpen(Util.LEFT_DIR)) left.gameObject.SetActive(false);
        if (activeRoom.isOpen(Util.UP_DIR)) up.gameObject.SetActive(false);
        if (activeRoom.isOpen(Util.DOWN_DIR)) down.gameObject.SetActive(false);
        CurseManager.Instance.resetValues();
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

    #region Pun Callbacks

    public override void OnJoinedRoom()
    {
        int mapSize = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties[RoomProperty.MapSize].ToString());
        Util.mapSize = mapSize;
        Debug.Log(Util.mapSize);
    }


    #endregion

}
