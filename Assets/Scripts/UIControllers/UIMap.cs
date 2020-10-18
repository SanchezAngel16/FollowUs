using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIMap : MonoBehaviour
{
    GridLayoutGroup gl;
    public Map mapController;
    public GameObject mapTile;

    public GameObject[,] mapTiles;
    private bool firstTime = true;
    void Start()
    {
        gl = GetComponent<GridLayoutGroup>();
        gl.cellSize = new Vector2(GetComponent<RectTransform>().rect.width / mapController.cols, GetComponent<RectTransform>().rect.height / mapController.rows);
        mapTiles = new GameObject[mapController.cols, mapController.rows];
        makeUIMap();
    }

    private void OnEnable()
    {
        if (firstTime) return;
        updateUIMap();
    }

    private void updateUIMap()
    {
        for (int row = 0; row < mapController.rows; row++)
        {
            for (int col = 0; col < mapController.cols; col++)
            {
                mapTiles[col, row].GetComponent<Image>().color = getBgMapTileColor(col, row);
            }
        }
    }

    private void makeUIMap()
    {
        for (int row = 0; row < mapController.rows; row++)
        {
            for (int col = 0; col < mapController.cols; col++)
            {
                GameObject newMapTile = (GameObject)Instantiate(mapTile);
                mapTiles[col, row] = newMapTile;
                newMapTile.GetComponent<Image>().color = getBgMapTileColor(col, row);
                newMapTile.transform.SetParent(this.transform);
                string enemiesCount;
                if (col == 0 && row == 0) enemiesCount = "";
                else enemiesCount = mapController.map[col, row].GetComponent<Room>().enemiesCount.ToString();
                newMapTile.GetComponentInChildren<TextMeshProUGUI>().text = enemiesCount;
            }
        }
        firstTime = false;
    }

    private Color getBgMapTileColor(int col, int row)
    {
        Vector2Int currentRoomMapTile = new Vector2Int(col, row);
        if (Vector2Int.Equals(mapController.goodRoom, currentRoomMapTile)) return Color.blue;
        else if (Vector2Int.Equals(mapController.badRoom, currentRoomMapTile)) return Color.red;
        else if (mapController.map[col, row].GetComponent<Room>().isRoomActive) return Color.white;
        else return Color.gray;
    }
}
