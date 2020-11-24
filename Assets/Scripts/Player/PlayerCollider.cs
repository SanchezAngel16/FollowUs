﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollider : MonoBehaviour
{
    public PlayerController playerController;
    public Shooting shooting;
    private SpriteRenderer sprite;
    private bool hitted;

    public Button reloadButton;

    public TextMeshProUGUI timerText;

    public float timer;

    [SerializeField]
    private GameObject[] roomsPreview = null;
    [SerializeField]
    private GameObject roomsAround = null;

    private bool gameOver = false;

    private string minutes;
    private string seconds;

    private List<string> enemiesTags;

    private void Start()
    {
        hitted = false;
        sprite = playerController.GetComponent<SpriteRenderer>();
        setEnemiesTag();
    }

    private void Update()
    {
        if (timer > 0)
        {
            if (!gameOver)
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

    private void setEnemiesTag()
    {
        enemiesTags = new List<string>();
        enemiesTags.Add("Enemy");
        enemiesTags.Add("EnemyBullet");
        enemiesTags.Add("Laser");
        enemiesTags.Add("EnemyShield");
        enemiesTags.Add("LightningLaser");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (Util.compareTags(enemiesTags, collision.gameObject))
        {
            if (hitted) return;
            hitted = true;
            if (playerController.living)
            {
                InvokeRepeating("startHitAnimation", 0f, 0.05f);
                Invoke("stopTakingDamageAnimation", 3.5f);
                playerController.updateLifePoints(-10);
                if (collision.gameObject.CompareTag("EnemyBullet")) collision.gameObject.SetActive(false);
            }
        }else if (collision.gameObject.CompareTag("Pickable_Area"))
        {
            reloadButton.gameObject.SetActive(true);
        }else if (collision.gameObject.CompareTag("Collectable_Ammo"))
        {
            shooting.reloadBullets(30);
            Destroy(collision.gameObject);
        }else if (collision.gameObject.CompareTag("Collectable_Health"))
        {
            Destroy(collision.gameObject);
            playerController.updateLifePoints(40);
        }else if (collision.gameObject.CompareTag("Room"))
        {
            Room currentRoom = collision.gameObject.GetComponent<Room>();
            roomsAround.transform.position = currentRoom.transform.position;
            setRoomsPreview(currentRoom.mapLocation);
            playerController.currentLocation = currentRoom.mapLocation;
            timer = currentRoom.timer;
            if (currentRoom.isDestroyed) playerController.updateLifePoints(-playerController.lifePoints);
            Main.Instance.setGameOverText(false,"");
            if (Vector2Int.Equals(playerController.currentLocation, Main.Instance.mapController.goodRoom))
            {
                Main.Instance.setGameOverText(true, "You win!");
                playerController.restart.gameObject.SetActive(true);
                Main.Instance.setActiveAllUIArrows(false);
                gameOver = true;
            }else if(Vector2Int.Equals(playerController.currentLocation, Main.Instance.mapController.badRoom))
            {
                Main.Instance.setGameOverText(true, "You lose!");
                playerController.updateLifePoints(-playerController.lifePoints);
                playerController.restart.gameObject.SetActive(true);
                Main.Instance.setActiveAllUIArrows(false);
                gameOver = true;
            }
        }
    }

    private void setRoomsPreview(Vector2Int location)
    {

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

    public void takeDamage(int lifePoints)
    {
        if (hitted) return;
        // Take damage animation and deactivate collider layer.
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
        sprite.enabled = !sprite.enabled;
    }

    private void stopTakingDamageAnimation()
    {
        sprite.enabled = true;
        hitted = false;
        CancelInvoke("startHitAnimation");
    }
}
