using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollider : MonoBehaviour
{
    public PlayerController playerController;
    private SpriteRenderer sprite;
    private bool hitted;

    public Button reloadButton;

    private void Start()
    {
        hitted = false;
        sprite = playerController.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Room"))
        {
            playerController.mainCamera.GetComponent<CameraFollow>().moveCamera(collision.transform.position);
            Room currentRoom = collision.gameObject.GetComponent<Room>();
            playerController.currentLocation = currentRoom.mapLocation;

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
                playerController.removeLifePoints(1);
                if (tag.Equals("EnemyBullet")) collision.gameObject.SetActive(false);
            }
        }else if (tag.Equals("Pickable_Area"))
        {
            reloadButton.gameObject.SetActive(true);
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
