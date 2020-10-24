using System.Collections;
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

    private void Start()
    {
        hitted = false;
        sprite = playerController.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.RoundToInt(timer % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            playerController.updateLifePoints(-playerController.lifePoints);
            timer = 0;
            timerText.text = "0:00";
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Room"))
        {
            playerController.mainCamera.GetComponent<CameraFollow>().moveCamera(collision.transform.position);
            Room currentRoom = collision.gameObject.GetComponent<Room>();
            playerController.currentLocation = currentRoom.mapLocation;
            timer = currentRoom.timer;
            
            if (Vector2Int.Equals(playerController.currentLocation, playerController.gameController.currentActiveRoom))
            {
                playerController.gameController.setActiveAllUIArrows(true);
                playerController.gameController.updateUIArrows();
            }
            else
            {
                playerController.gameController.setActiveAllUIArrows(false);
            }
        }
        else if (tag.Equals("Enemy") || tag.Equals("EnemyBullet"))
        {
            if (hitted) return;
            // Take damage animation and deactivate collider layer.
            hitted = true;
            if (playerController.living)
            {
                InvokeRepeating("startHitAnimation", 0f, 0.05f);
                Invoke("stopTakingDamageAnimation", 3.5f);
                playerController.updateLifePoints(-10);
                if (tag.Equals("EnemyBullet")) collision.gameObject.SetActive(false);
            }
        }else if (tag.Equals("Pickable_Area"))
        {
            reloadButton.gameObject.SetActive(true);
        }else if (tag.Equals("Collectable_Ammo"))
        {
            shooting.reloadBullets(30);
            collision.gameObject.SetActive(false);
            
        }else if (tag.Equals("Collectable_Health"))
        {
            collision.gameObject.SetActive(false);
            playerController.updateLifePoints(40);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Pickable_Area"))
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
