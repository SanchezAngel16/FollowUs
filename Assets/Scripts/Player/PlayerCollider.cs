using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController = null;
    
    private bool hitted;

    private float timer;

    [SerializeField]
    private GameObject[] roomsPreview = null;
    [SerializeField]
    private GameObject roomsAround = null;
    [SerializeField]
    private Button reloadButton = null;
    [SerializeField]
    private TextMeshProUGUI timerText = null;


    private string minutes;
    private string seconds;

    private bool waiting = true;

    private void Awake()
    {
        setComponents();
    }

    private void Start()
    {
        hitted = false;
    }

    private void Update()
    {
        if (!waiting)
        {
            if (timer > 0)
            {
                if (playerController.living)
                {
                    timer -= Time.deltaTime;
                }
                minutes = Mathf.Floor(timer / 60).ToString("00");
                seconds = (timer % 60).ToString("00");

                timerText.text = string.Format("{0}:{1}", minutes, seconds);

            }
            else
            {
                playerController.updateLifePoints(-playerController.lifePoints);
                timer = 0;
                timerText.text = "0:00";
            }
        }
    }

    private void setComponents()
    {
        reloadButton = PlayerComponents.Instance.reloadButton;
        timerText = PlayerComponents.Instance.timerText;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            Door door = collision.gameObject.GetComponent<Door>();
            if (door.opened)
            {
                Vector2 pPos = playerController.transform.position;
                playerController.rb.position = new Vector2(pPos.x + door.direction.x, pPos.y + door.direction.y);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Util.compareTags(collision.gameObject))
        {
            //takeDamage(10, collision);
        }else if (collision.gameObject.CompareTag("Pickable_Area"))
        {
            //reloadButton.gameObject.SetActive(true);
        }else if (collision.gameObject.CompareTag("Collectable_Ammo"))
        {
            playerController.playerShooting.reloadBullets(30);
            Destroy(collision.gameObject);
        }else if (collision.gameObject.CompareTag("Collectable_Health"))
        {
            Destroy(collision.gameObject);
            playerController.updateLifePoints(30);
        }else if (collision.gameObject.CompareTag("Room"))
        {
            Room currentRoom = collision.gameObject.GetComponent<Room>();
            enterRoom(currentRoom);
        }
    }

    private void OnRoomStarted(object sender, Room.OnRoomStartedArgs e)
    {
        waiting = false;
        timer = e.initTimer;
    }

    public void enterRoom(Room r)
    {
        updateRoomsPreview(r);
        playerController.currentLocation = r.mapLocation;
        waiting = false;
        timer = r.timerManager.timer;
        //r.OnRoomStarted += OnRoomStarted;

        if (r.isDestroyed) playerController.updateLifePoints(-playerController.lifePoints);
        else
        {
            if (r.roomType == Util.GoodRoom) finishGame(true);
            else if (r.roomType == Util.BadRoom) finishGame(false);
        }
    }

    private void finishGame(bool hasWin)
    {
        playerController.restart.gameObject.SetActive(true);
        if (hasWin) GameController.Instance.setGameOverText(true, "You Win!");
        else
        {
            GameController.Instance.setGameOverText(true, "You lose!");
            playerController.updateLifePoints(-playerController.lifePoints);
        }
        VotingSystem.Instance.setActiveUIDirectionsButtons(false);
    }

    private void updateRoomsPreview(Room r)
    {
        Vector2Int location = r.mapLocation;
        roomsAround.transform.position = r.transform.position;
        for(int i = 0; i < roomsPreview.Length; i++)
        {
            roomsPreview[i].SetActive(true);
        }

        if(location.x == 0)
        {
            roomsPreview[0].SetActive(false);
            roomsPreview[3].SetActive(false);
            roomsPreview[5].SetActive(false);
        }else if(location.x == Util.mapSize - 1)
        {
            roomsPreview[2].SetActive(false);
            roomsPreview[4].SetActive(false);
            roomsPreview[7].SetActive(false);
        }

        if(location.y == 0)
        {
            roomsPreview[0].SetActive(false);
            roomsPreview[1].SetActive(false);
            roomsPreview[2].SetActive(false);
        }else if(location.y == Util.mapSize - 1)
        {
            roomsPreview[5].SetActive(false);
            roomsPreview[6].SetActive(false);
            roomsPreview[7].SetActive(false);
        }
    }

    public void takeDamage(int lifePoints, Collider2D collision)
    {
        if (hitted) return;
        hitted = true;
        playerController.updateLifePoints(-lifePoints);
        if (playerController.living)
        {
            InvokeRepeating("startHitAnimation", 0f, 0.05f);
            Invoke("stopTakingDamageAnimation", 3.5f);
            if (collision.gameObject.CompareTag("EnemyBullet")) collision.gameObject.SetActive(false);
        }
    }

    public void takeDamage(int lifePoints)
    {
        if (hitted) return;
        hitted = true;
        playerController.updateLifePoints(-lifePoints);
        if (playerController.living)
        {
            InvokeRepeating("startHitAnimation", 0f, 0.05f);
            Invoke("stopTakingDamageAnimation", 3.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickable_Area"))
        {
            reloadButton.gameObject.SetActive(false);
        }
        
    }

    private void startHitAnimation()
    {
        playerController.spriteRenderer.enabled = !playerController.spriteRenderer.enabled;
    }

    private void stopTakingDamageAnimation()
    {
        playerController.spriteRenderer.enabled = true;
        hitted = false;
        CancelInvoke("startHitAnimation");
    }
}
