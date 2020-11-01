using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int cols = 6;
    public int rows = 6;
    public GameObject[,] map;

    public Vector2Int goodRoom;
    public Vector2Int badRoom;
    public Vector2Int startRoom;

    public void initMapArray()
    {
        map = new GameObject[cols, rows];
    }
    public void updatePosibleDirections()
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


    public void checkAroundRooms(int currentCol, int currentRow)
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
                //if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;
                down = getPassability(DOWN);
            }
            else if (currentRow == rows - 1)
            {
                down = 0;
                //if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
                up = getPassability(UP);
            }
            else
            {
                //if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
                //if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;
                up = getPassability(UP);
                down = getPassability(DOWN);
            }
            //if (map[currentCol + 1, currentRow].GetComponent<Room>().isRoomActive) right = 0;
            right = getPassability(RIGHT);
        }else if(currentRow == 0)
        {
            up = 0;
            if (currentCol == cols - 1) right = 0;
            else right = getPassability(RIGHT);
            //else if (map[currentCol + 1, currentRow].GetComponent<Room>().isRoomActive) right = 0;

            //if (map[currentCol - 1, currentRow].GetComponent<Room>().isRoomActive) left = 0;
            //if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;
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
            //else if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;

            //if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
            //if (map[currentCol - 1, currentRow].GetComponent<Room>().isRoomActive) left = 0;
            up = getPassability(UP);
            left = getPassability(LEFT);
        }else if(currentRow == rows - 1)
        {
            down = 0;

            /*if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
            if (map[currentCol - 1, currentRow].GetComponent<Room>().isRoomActive) left = 0;
            if (map[currentCol + 1, currentRow].GetComponent<Room>().isRoomActive) right = 0;*/
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
            /*if (map[currentCol + 1, currentRow].GetComponent<Room>().isRoomActive) right = 0;
            if (map[currentCol - 1, currentRow].GetComponent<Room>().isRoomActive) left = 0;
            if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
            if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;*/
        }
        map[currentCol, currentRow].GetComponent<Room>().setPosibleDirections(right, left, up, down);
    }
}
