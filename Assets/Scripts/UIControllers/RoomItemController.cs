using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class RoomItemController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomName = null;
    [SerializeField]
    private TextMeshProUGUI maxPlayers = null;
    [SerializeField]
    private TextMeshProUGUI badLeaders = null;
    [SerializeField]
    private TextMeshProUGUI mapSize = null;

    public void setRoomInfo(string roomName, string playerCount, string maxPlayers, string badLeaders, string mapSize)
    {
        this.roomName.text = roomName;
        this.maxPlayers.text = playerCount + "/" + maxPlayers;
        this.badLeaders.text = badLeaders;
        this.mapSize.text = mapSize;
    }
}
