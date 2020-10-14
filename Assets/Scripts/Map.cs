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

    public void checkAroundRooms(int currentCol, int currentRow)
    {
        int right = 1;
        int left = 1;
        int up = 1;
        int down = 1;

        if(currentCol == 0)
        {
            left = 0;
            if (currentRow == 0)
            {
                up = 0;
                if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;
            }
            else if (currentRow == rows - 1)
            {
                down = 0;
                if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
            }
            else
            {
                if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
                if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;
            }

            if (map[currentCol + 1, currentRow].GetComponent<Room>().isRoomActive) right = 0;
        }else if(currentRow == 0)
        {
            up = 0;
            if (currentCol == cols - 1) right = 0;
            else if (map[currentCol + 1, currentRow].GetComponent<Room>().isRoomActive) right = 0;

            if (map[currentCol - 1, currentRow].GetComponent<Room>().isRoomActive) left = 0;
            if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;
        }else if(currentCol == cols - 1)
        {
            right = 0;
            if (currentRow == rows - 1) down = 0;
            else if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;

            if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
            if (map[currentCol - 1, currentRow].GetComponent<Room>().isRoomActive) left = 0;
        }else if(currentRow == rows - 1)
        {
            down = 0;

            if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
            if (map[currentCol - 1, currentRow].GetComponent<Room>().isRoomActive) left = 0;
            if (map[currentCol + 1, currentRow].GetComponent<Room>().isRoomActive) right = 0;
        }
        else
        {
            if (map[currentCol + 1, currentRow].GetComponent<Room>().isRoomActive) right = 0;
            if (map[currentCol - 1, currentRow].GetComponent<Room>().isRoomActive) left = 0;
            if (map[currentCol, currentRow - 1].GetComponent<Room>().isRoomActive) up = 0;
            if (map[currentCol, currentRow + 1].GetComponent<Room>().isRoomActive) down = 0;
        }
        map[currentCol, currentRow].GetComponent<Room>().setPosibleDirections(right, left, up, down);
    }
}
