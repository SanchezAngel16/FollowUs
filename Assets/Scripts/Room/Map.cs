using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Map : MonoBehaviour, IPunObservable
{
    public int cols = 6;
    public int rows = 6;
    public Room[,] map;

    public Vector2Int goodRoom = new Vector2Int();
    public Vector2Int badRoom = new Vector2Int();
    public Vector2Int startRoom = new Vector2Int();

    [SerializeField]
    private GameObject roomPrefab = null;
    [SerializeField]
    private Room tempRoom = null;

    private bool firstTimeChecking = true;

    private void Awake()
    {
        int mapSize = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties[RoomProperty.MapSize].ToString());
        Util.mapSize = mapSize;
        cols = rows = Util.mapSize;
        map = new Room[cols, rows];
    }

    public void initMap()
    {
        Vector2 roomSize = new Vector2(roomPrefab.GetComponent<Renderer>().bounds.size.x, roomPrefab.GetComponent<Renderer>().bounds.size.y);
        createMap(roomSize.x, roomSize.y);
        updatePosibleDirections();
    }

    private void setRoomsType()
    {
        goodRoom = new Vector2Int(Random.Range(0, cols), Random.Range(0, rows));
        do
        {
            badRoom = new Vector2Int(Random.Range(0, cols), Random.Range(0, rows));
        } while (Vector2Int.Equals(goodRoom, badRoom));

        do
        {
            startRoom = new Vector2Int(Random.Range(0, cols), Random.Range(0, rows));
        } while (Vector2Int.Equals(startRoom, goodRoom)
        || Vector2Int.Equals(startRoom, badRoom));
    }

    private void createMap(float wRoom, float hRoom)
    {
        setRoomsType();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector2Int newRoomLocation = new Vector2Int(col, row);

                float posX = col * wRoom;
                float posY = row * -hRoom;

                if (Vector2Int.Equals(newRoomLocation, goodRoom))
                {
                    tempRoom.threatType = 0;
                    tempRoom.setRoomType(0, Util.GoodRoom);
                }
                else if (Vector2Int.Equals(newRoomLocation, badRoom))
                {
                    tempRoom.setRoomType(tempRoom.threatType, Util.BadRoom);
                }
                else
                {
                    if (Vector2Int.Equals(newRoomLocation, startRoom))
                    {
                        tempRoom.setRoomType(0, Util.MainRoom);
                    }
                    else
                    {
                        tempRoom.threatType = Random.Range(1, 10);
                        tempRoom.setRoomType(tempRoom.threatType, Util.NormalRoom);
                    }
                }

                tempRoom.mapLocation = newRoomLocation;
                tempRoom.isRoomActive = false;
                object[] initData = Util.serializeRoomObject(tempRoom);

                Vector2 newPos = new Vector2(posX, posY);
                PhotonNetwork.Instantiate(roomPrefab.name, newPos, Quaternion.identity, 0, initData);
            }
        }
    }

    private void updatePosibleDirections()
    {
        for(int row = 0; row < rows; row++)
        {
            for(int col = 0; col < cols; col++)
            {
                checkAroundRooms(col, row);
            }
        }
    }

    private int getPassability(Vector2Int direction)
    {
        Room r = map[direction.x, direction.y].GetComponent<Room>();
        if (r.isRoomActive || r.isDestroyed) return 0;
        return 1;
    }

    private void checkAroundRooms(int currentCol, int currentRow)
    {
        int right = 1;
        int left = 1;
        int up = 1;
        int down = 1;

        Vector2Int RIGHT = new Vector2Int(currentCol + 1, currentRow);
        Vector2Int LEFT = new Vector2Int(currentCol - 1, currentRow);
        Vector2Int UP = new Vector2Int(currentCol, currentRow - 1);
        Vector2Int DOWN = new Vector2Int(currentCol, currentRow + 1);

        if (currentCol == 0)
        {
            left = 0;
            if (currentRow == 0)
            {
                up = 0;
                down = getPassability(DOWN);
            }
            else if (currentRow == rows - 1)
            {
                down = 0;
                up = getPassability(UP);
            }
            else
            {
                up = getPassability(UP);
                down = getPassability(DOWN);
            }
            right = getPassability(RIGHT);
        }else if(currentRow == 0)
        {
            up = 0;
            if (currentCol == cols - 1) right = 0;
            else right = getPassability(RIGHT);
            left = getPassability(LEFT);
            down = getPassability(DOWN);
        }else if(currentCol == cols - 1)
        {
            right = 0;
            if (currentRow == rows - 1)
            {
                down = 0;
            }
            else {
                down = getPassability(DOWN);
            }
            up = getPassability(UP);
            left = getPassability(LEFT);
        }else if(currentRow == rows - 1)
        {
            down = 0;
            up = getPassability(UP);
            left = getPassability(LEFT);
            right = getPassability(RIGHT);
        }
        else
        {
            down = getPassability(DOWN);
            up = getPassability(UP);
            left = getPassability(LEFT);
            right = getPassability(RIGHT);
        }
        map[currentCol, currentRow].setPosibleDirections(right, left, up, down);
    }

    public void desactivateDoor(int x, int y, int direction, Vector2Int currentActiveRoom)
    {
        Vector2Int activeRoom = currentActiveRoom;
        int doorToDeactivate = 0;
        Room room = map[activeRoom.x, activeRoom.y];
        switch (direction)
        {
            case Util.RIGHT_DIR:
                room.rightDoor.openDoor();
                doorToDeactivate = Util.LEFT_DIR;
                break;
            case Util.LEFT_DIR:
                room.leftDoor.openDoor();
                doorToDeactivate = Util.RIGHT_DIR;
                break;
            case Util.UP_DIR:
                room.upDoor.openDoor();
                doorToDeactivate = Util.DOWN_DIR;
                break;
            case Util.DOWN_DIR:
                room.downDoor.openDoor();
                doorToDeactivate = Util.UP_DIR;
                break;
        }

        activeRoom.x += x;
        activeRoom.y += y;
        room = map[activeRoom.x, activeRoom.y];
        CurseManager.Instance.activateCurse(room);
        room.gameObject.SetActive(true);
        room.isRoomActive = true;

        switch (doorToDeactivate)
        {
            case Util.RIGHT_DIR:
                room.rightDoor.openDoor();
                break;
            case Util.LEFT_DIR:
                room.leftDoor.openDoor();
                break;
            case Util.UP_DIR:
                room.upDoor.openDoor();
                break;
            case Util.DOWN_DIR:
                room.downDoor.openDoor();
                break;
        }

        updatePosibleDirections();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(startRoom.x);
            stream.SendNext(startRoom.y);
            stream.SendNext(goodRoom.x);
            stream.SendNext(goodRoom.y);
            stream.SendNext(badRoom.x);
            stream.SendNext(badRoom.y);
            /*stream.SendNext(startRoom.x);
            stream.SendNext(startRoom.x);*/
        }
        else if (stream.IsReading)
        {
            startRoom.x = (int)stream.ReceiveNext();
            startRoom.y = (int)stream.ReceiveNext();
            goodRoom.x = (int)stream.ReceiveNext();
            goodRoom.y = (int)stream.ReceiveNext();
            badRoom.x = (int)stream.ReceiveNext();
            badRoom.y = (int)stream.ReceiveNext();
            if (firstTimeChecking)
            {
                updatePosibleDirections();
                firstTimeChecking = false;
            }
        }
    }
}
