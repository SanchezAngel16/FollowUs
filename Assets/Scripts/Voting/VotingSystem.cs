using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class VotingSystem : MonoBehaviourPunCallbacks
{
    private int votesCount = 0;
    private int currentPlayers = 0;

    [SerializeField]
    private int right = 0, left = 0, up = 0, down = 0;

    [SerializeField]
    private Button rightButton = null, leftButton = null, upButton = null, downButton = null;

    public TextMeshProUGUI testCurrentPlayers;


    private static VotingSystem instance = null;


    #region Singleton 
    public static VotingSystem Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient) initDirectionButtons();
            else Invoke("initDirectionButtons", 1.5f);
            currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
            testCurrentPlayers.text = currentPlayers.ToString();
        }
    }

    #endregion

    #region Public Methods

    public void updateUIDirectionsButtons()
    {
        setActiveUIDirectionsButtons(true);
        Vector2Int currentActiveRoom = GameController.Instance.currentActiveRoom;
        Debug.Log(currentActiveRoom);
        Room activeRoom = GameController.Instance.mapController.map[currentActiveRoom.x, currentActiveRoom.y];
        if (activeRoom.isOpen(Util.RIGHT_DIR)) rightButton.gameObject.SetActive(false);
        if (activeRoom.isOpen(Util.LEFT_DIR)) leftButton.gameObject.SetActive(false);
        if (activeRoom.isOpen(Util.UP_DIR)) upButton.gameObject.SetActive(false);
        if (activeRoom.isOpen(Util.DOWN_DIR)) downButton.gameObject.SetActive(false);
        CurseManager.Instance.resetValues();
    }

    public void setActiveUIDirectionsButtons(bool active)
    {
        rightButton.gameObject.SetActive(active);
        leftButton.gameObject.SetActive(active);
        upButton.gameObject.SetActive(active);
        downButton.gameObject.SetActive(active);
    }

    #endregion

    #region Private Methods
    private void onVoteButtonClick(int direction)
    {
        setActiveUIDirectionsButtons(false);
        photonView.RPC("vote", RpcTarget.All, direction);
        checkVoting();
    }

    private void initDirectionButtons()
    {
        rightButton.onClick.AddListener(() => {
            onVoteButtonClick(Util.RIGHT_DIR);
        });
        leftButton.onClick.AddListener(() => {
            onVoteButtonClick(Util.LEFT_DIR);
        });
        downButton.onClick.AddListener(() => {
            onVoteButtonClick(Util.DOWN_DIR);
        });
        upButton.onClick.AddListener(() => {
            onVoteButtonClick(Util.UP_DIR);
        });
        updateUIDirectionsButtons();
    }


    private void checkVoting()
    {
        if(votesCount >= currentPlayers)
        {
            if (right > left && right > up && right > down)
            {

                photonView.RPC("openDoor", RpcTarget.All, 1, 0, Util.RIGHT_DIR);
            }
            else if (left > right && left > up && left > down)
            {
                photonView.RPC("openDoor", RpcTarget.All, -1, 0, Util.LEFT_DIR);
            }
            else if (up > right && up > left && up > down)
            {
                photonView.RPC("openDoor", RpcTarget.All, 0, -1, Util.UP_DIR);
            }
            else if (down > right && down > left && down > up)
            {
                photonView.RPC("openDoor", RpcTarget.All, 0, 1, Util.DOWN_DIR);
            }
        }
    }

    #endregion

    #region RPC Calls

    [PunRPC]
    public void openDoor(int x, int y, int direction)
    {
        GameController.Instance.openDoor(x, y, direction);
        votesCount = right = left = up = down = 0;
    }

    [PunRPC]
    public void vote(int direction)
    {
        switch (direction)
        {
            case Util.RIGHT_DIR:
                right++;
                break;
            case Util.LEFT_DIR:
                left++;
                break;
            case Util.UP_DIR:
                up++;
                break;
            case Util.DOWN_DIR:
                down++;
                break;
        }
        votesCount++;
    }

    #endregion

    #region Photon Callbacks
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        currentPlayers++;
        testCurrentPlayers.text = currentPlayers.ToString();
        checkVoting();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        currentPlayers--;
        testCurrentPlayers.text = currentPlayers.ToString();
        checkVoting();
    }

    #endregion
}
