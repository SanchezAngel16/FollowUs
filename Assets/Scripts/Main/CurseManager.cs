using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurseManager : MonoBehaviour
{
    public const int Nothing = 0;
    public const int FreezeBullet = 1;
    public const int ReduceTime = 2;
    public const int SpeedUpEnemies = 3;
    public const int LightsOut = 4;
    public const int FriendFire = 5;
    public const int IncreaseThreat = 6;

    public static float bulletSpeed = 1f;
    public static float enemiesSpeed = 1f;
    public static float rotationSpeed = 1f;


    [SerializeField]
    private int currentCurseType = -1;
    [SerializeField]
    private Image curseTypeImage = null;
    [SerializeField]
    private Sprite[] curseTypes = null;

    [SerializeField]
    private GameObject lightsOut = null;

    private static CurseManager instance = null;
    public static CurseManager Instance
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
        }
        else Destroy(this);
    }

    public void resetValues()
    {
        bulletSpeed = 1f;
        enemiesSpeed = 1f;
        rotationSpeed = 1f;
        lightsOut.SetActive(false);
    }

    public void activateCurse(Room room)
    {
        activeUI();
        resetValues();
        switch (currentCurseType)
        {
            case Nothing:
                break;
            case FreezeBullet:
                bulletSpeed = 0.45f;
                break;
            case SpeedUpEnemies:
                enemiesSpeed = 1.8f;
                rotationSpeed = 2f;
                break;
            case LightsOut:
                lightsOut.SetActive(true);
                break;
            case FriendFire:
                break;
            case IncreaseThreat:
                room.increaseThreatType();
                break;
            case ReduceTime:
                room.reduceTime();
                break;
        }
        currentCurseType = 0;
    }

    private void activeUI()
    {
        if (currentCurseType == 0) curseTypeImage.gameObject.SetActive(false);
        else
        {
            curseTypeImage.gameObject.SetActive(true);
            curseTypeImage.sprite = curseTypes[currentCurseType - 1];
        }
    }

    public void setCurseType(int type)
    {
        this.currentCurseType = type;
    }
}
