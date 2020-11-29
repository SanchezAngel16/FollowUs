using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    public Joystick movementJoystick;
    public Joystick aimingJoystick;
    public BulletPool bulletPool;
    public Camera mainCamera;
    public Button restartButton;
    public Image ammoImage;
    public Button reloadButton;
    public TextMeshProUGUI timerText;
    public GameObject[] roomsPreview;
    public GameObject roomsAround;
    public Slider slider;
    public TextMeshProUGUI bulletsCountText;

    public Transform map;
    public Map mapController;

    private static PlayerComponents instance = null;

    public static PlayerComponents Instance
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
}
