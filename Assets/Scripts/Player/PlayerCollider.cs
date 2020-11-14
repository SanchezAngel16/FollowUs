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
        
        if (tag.Equals("Enemy") || tag.Equals("EnemyBullet") || tag.Equals("Laser") || tag.Equals("EnemyShield"))
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
            Destroy(collision.gameObject);
        }else if (tag.Equals("Collectable_Health"))
        {
            Destroy(collision.gameObject);
            playerController.updateLifePoints(40);
        }else if (tag.Equals("Room"))
        {
            Room currentRoom = collision.gameObject.GetComponent<Room>();
            playerController.currentLocation = currentRoom.mapLocation;
            timer = currentRoom.timer;
            if (currentRoom.isDestroyed) playerController.updateLifePoints(-playerController.lifePoints);
            if (Vector2Int.Equals(playerController.currentLocation, Main.Instance.mapController.goodRoom))
            {
                playerController.restart.gameObject.SetActive(true);
                Main.Instance.setActiveAllUIArrows(false);
            }else if(Vector2Int.Equals(playerController.currentLocation, Main.Instance.mapController.badRoom))
            {
                playerController.updateLifePoints(-playerController.lifePoints);
                playerController.restart.gameObject.SetActive(true);
                Main.Instance.setActiveAllUIArrows(false);
            }
        }else if (tag.Equals("EmptyRoom"))
        {
            playerController.updateLifePoints(-playerController.lifePoints);
        }
    }

    public void takeDamage(int lifePoints)
    {
        if (hitted) return;
        // Take damage animation and deactivate collider layer.
        hitted = true;
        if (playerController.living)
        {
            InvokeRepeating("startHitAnimation", 0f, 0.05f);
            Invoke("stopTakingDamageAnimation", 3.5f);
            playerController.updateLifePoints(-lifePoints);
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
