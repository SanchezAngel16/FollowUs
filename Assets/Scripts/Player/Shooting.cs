using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public PlayerController playerController;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public BulletPool bulletPool;

    public float bulletForce = 15f;

    private int maxBulletsCount;
    private int bulletsCount;
    public TextMeshProUGUI bulletsCountText;

    public GameObject gun;
    public Camera mainCamera;

    private void Start()
    {
        maxBulletsCount = 100;
        bulletsCount = 100;
        updateBulletsCountText();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Shoot();
        }
    }

    public void Shoot()
    {
        if(!(bulletsCount <= 0))
        {
            Transform target = getNearestTarget(Main.enemies);
            
            
            if(target != null)
            {
                Vector2 lookAtDir = target.position - gun.transform.position;
                float angle = Mathf.Atan2(lookAtDir.y, lookAtDir.x) * Mathf.Rad2Deg - 90f;
                if(target.position.x > playerController.transform.position.x) playerController.flipPlayer(-1);
                else playerController.flipPlayer(1);
                gun.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                Debug.Log("ninguno");
                playerController.targeting = false;
            }

            GameObject bullet = bulletPool.getBullet();
            bullet.GetComponent<Bullet>().bulletType = 0;
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

            bulletsCount--;
            updateBulletsCountText();
        }
        
    }

    private Transform getNearestTarget(List<Transform> elements) 
    {
        Transform nearestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in elements)
        {
            if (!potentialTarget.gameObject.activeInHierarchy) break;
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestTarget = potentialTarget;
            }
        }

        return nearestTarget;
    }

    private void updateBulletsCountText()
    {
        bulletsCountText.text = (bulletsCount + "/" + maxBulletsCount).ToString();
    }

    public void reloadBullets()
    {
        bulletsCount += 10;
        if (bulletsCount >= maxBulletsCount) bulletsCount = maxBulletsCount;
        updateBulletsCountText();
    }
}
