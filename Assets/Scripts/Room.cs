using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int mapLocation;
    public bool isRoomActive;
    public int[] posibleDirections = new int[4];

    public int enemiesCount;
    public Transform enemiesGenerationPoint;
    public GameObject enemyPrefab;

    public GameObject right;
    public GameObject left;
    public GameObject down;
    public GameObject up;

    public Sprite[] backgroundSprites;
    public Sprite[] doorSprites;

    public GameObject staticElementsGroup;
    public GameObject[] staticElements;
    void Awake()
    {
        posibleDirections[0] = 1;
        posibleDirections[1] = 1;
        posibleDirections[2] = 1;
        posibleDirections[3] = 1;
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = backgroundSprites[Random.Range(0, backgroundSprites.Length)];
    }

    public void generateEnemies()
    {
        for (int i = 0; i < enemiesCount; i++)
        {
            Instantiate(enemyPrefab, enemiesGenerationPoint);
        }
    }

    public void setDoorSprites(int cols, int rows)
    {
        Sprite notPassSprite = doorSprites[0];
        if (mapLocation.x == 0) left.GetComponent<SpriteRenderer>().sprite = notPassSprite;
        else if (mapLocation.x == cols - 1) right.GetComponent<SpriteRenderer>().sprite = notPassSprite;

        if (mapLocation.y == 0) up.GetComponent<SpriteRenderer>().sprite = notPassSprite;
        else if (mapLocation.y == rows - 1) down.GetComponent<SpriteRenderer>().sprite = notPassSprite;
    }

    public void setPosibleDirections(int right, int left, int up, int down)
    {
        posibleDirections[0] = right;
        posibleDirections[1] = left;
        posibleDirections[2] = up;
        posibleDirections[3] = down;
    }

    public void generateStaticElements(int numElements)
    {
        if (numElements == 1)
        {
            Instantiate(staticElements[0], staticElementsGroup.transform).transform.position = staticElementsGroup.transform.position;
        }
        else
        {
            for(int i = 0; i < numElements; i++)
            {
                GameObject newStaticElement = Instantiate(staticElements[Random.Range(1, staticElements.Length)], staticElementsGroup.transform);
                newStaticElement.transform.position = Util.getRandomPosition(staticElementsGroup.transform);
            }
        }
    }
}
