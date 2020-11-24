using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int cols = 6;
    public int rows = 6;
    public Room[,] map;

    public Vector2Int goodRoom;
    public Vector2Int badRoom;
    public Vector2Int startRoom;

    [SerializeField]
    private GameObject roomPrefab;

    private void Start()
    {
        cols = rows = Util.mapSize;
        map = new Room[cols, rows];
        initMap();
    }

    private void initMap()
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
        Color rColor;
        setRoomsType();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector2Int newRoomLocation = new Vector2Int(col, row);

                GameObject newRoom = Instantiate(roomPrefab, transform);
                Room roomScript = newRoom.GetComponent<Room>();

                float posX = col * wRoom;
                float posY = row * -hRoom;

                newRoom.transform.position = new Vector2(posX, posY);


                if (Vector2Int.Equals(newRoomLocation, startRoom))
                {
                    roomScript.setRoomType(0, (int)Util.RoomType.MAIN);
                    newRoom.SetActive(true);
                }
                else
                {
                    roomScript.threatType = Random.Range(1, 10);
                }

                if (Vector2Int.Equals(newRoomLocation, goodRoom))
                {
                    rColor = Color.blue;
                    roomScript.threatType = 0;
                    roomScript.setRoomType(0, (int)Util.RoomType.GOOD);
                }
                else if (Vector2Int.Equals(newRoomLocation, badRoom))
                {
                    roomScript.setRoomType(roomScript.threatType, (int)Util.RoomType.BAD);
                    rColor = Color.red;
                }
                else
                {
                    roomScript.setRoomType(roomScript.threatType, (int)Util.RoomType.NORMAL);
                    rColor = Color.white;
                }

                roomScript.mapLocation = newRoomLocation;
                newRoom.GetComponent<SpriteRenderer>().color = rColor;

                newRoom.SetActive(false);
                roomScript.isRoomActive = false;
                roomScript.setDoorSprites(cols, rows);
                map[col, row] = roomScript;
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

    public void desactivateDoor(int x, int y, int direction, Vector2Int currentActiveRoom, int curseType)
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
        room.setCurseType(curseType);
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


}
