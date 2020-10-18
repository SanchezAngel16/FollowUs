using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Main gameController;
    public Vector2Int currentLocation;

    public Animator animator;

    public float speed;

    public Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Vector2 mousePos;

    public Camera mainCamera;

    public GameObject gun;

    public int lifePoints;
    public Image[] lifesImages = new Image[3];

    void Start()
    {
        lifePoints = 3;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        moveVelocity = moveInput.normalized * speed;

        if (moveInput.x > 0) flipPlayer(-1);
        if (moveInput.x < 0) flipPlayer(1);
        if (mousePos.x > transform.position.x) flipPlayer(-1);
        if (mousePos.x < transform.position.x) flipPlayer(1);

        if (moveInput.x != 0 || moveInput.y != 0) animator.SetFloat("speed", 1);
        else animator.SetFloat("speed", 0);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        gun.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    private void flipPlayer(int flip)
    {
        if (flip < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            gun.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
            gun.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void removeLifePoints(int points)
    {
        this.lifePoints -= points;
        updateHealthUI();
        if(lifePoints == 0)
        {
            //Kill player
        }
    }

    private void updateHealthUI()
    {
        for (int i = 0; i < lifesImages.Length; i++)
        {
            if (i >= lifePoints) lifesImages[i].enabled = false;
            else lifesImages[i].enabled = true;
        }
    }

}
