using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public BulletPool bulletPool;

    public float bulletForce = 15f;

    public CameraShake camShake;

    private int maxBulletsCount;
    private int bulletsCount;
    public TextMeshProUGUI bulletsCountText;

    private void Start()
    {
        maxBulletsCount = 20;
        bulletsCount = 20;
        updateBulletsCountText();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(!(bulletsCount <= 0))
        {
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
