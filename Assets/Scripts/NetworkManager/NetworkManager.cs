using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Unity Fields
    
    [SerializeField]
    private GameObject connectionStatusPanel = null;
    [SerializeField]
    private MenuManager menuManager = null;
    [SerializeField]
    private TMP_InputField inputPlayerName = null;

    [Header("Create Room")]
    [SerializeField]
    private TMP_InputField inputRoomName = null;
    [SerializeField]
    private TMP_InputField inputMaxPlayers = null;

    [Header("Room List")]
    [SerializeField]
    private GameObject roomListParent = null;
    [SerializeField]
    private GameObject roomItemList = null;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;

    #endregion

    #region Unity Methods

    private void Start()
    {
        menuManager.setStatus(MenuStatus.MAIN);
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();
    }

    #endregion

    #region UI Methods

    public void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            string playerName = inputPlayerName.text;
            if (!string.IsNullOrEmpty(playerName))
            {
                PhotonNetwork.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
                connectionStatusPanel.SetActive(true);
            }
        }
    }

    public void CreateRoom()
    {
        string roomName = inputRoomName.text;
        string maxPlayers = inputMaxPlayers.text;

        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(100, 100000);
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(maxPlayers);
        

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void leaveLobby()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.Disconnect();
        }
    }

    #endregion

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        menuManager.setStatus(MenuStatus.LOBBY);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " connected to Photon");
        connectionStatusPanel.SetActive(false);
        showRoomList();
    }

    public override void OnConnected()
    {
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " created.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        clearRoomList();

        foreach (RoomInfo room in roomList)
        {
            if(!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList.Remove(room.Name);
                }
            }
            else
            {
                if (cachedRoomList.ContainsKey(room.Name)) cachedRoomList[room.Name] = room;
                else cachedRoomList.Add(room.Name,room);
            }
        }

        foreach(RoomInfo room in cachedRoomList.Values)
        {
            GameObject roomItemListGameObject = Instantiate(roomItemList, roomListParent.transform);
            roomItemListGameObject.transform.localScale = Vector3.one;


            roomItemListGameObject.transform.Find("RoomName").GetComponent<TextMeshProUGUI>().text = room.Name;
            roomItemListGameObject.transform.Find("RoomPlayers").GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/" + room.MaxPlayers;
            roomItemListGameObject.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                joinRoom(room.Name);
            });
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected");
    }

    public override void OnLeftLobby()
    {
        clearRoomList();
        cachedRoomList.Clear();
    }

    #endregion

    #region Private Methods

    private void showRoomList()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    private void joinRoom(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    private void clearRoomList()
    {
        foreach(var roomListGameObject in roomListGameObjects.Values)
        {
            Destroy(roomListGameObject);
        }

        roomListGameObjects.Clear();
    }

    #endregion
}
