using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Vector2 moveInput;
    private Vector2 lookDir;

    public Camera mainCamera;

    public GameObject gun;

    public int lifePoints;
    private int maxLifePoints = 100;
    public Slider slider;

    public Joystick joystick;

    public bool living;
    public bool targeting;

    public Button restart;

    public SpriteRenderer spriteRenderer;
    private SpriteRenderer gunSpriteRenderer;

    private bool flipped;

    private Vector3 lookAngle;
    void Start()
    {
        living = true;
        lifePoints = maxLifePoints;
        slider.maxValue = maxLifePoints;
        slider.value = slider.maxValue;

        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();

        moveInput = new Vector2(0, 0);
        mousePos = new Vector2(0, 0);
        lookDir = new Vector2(0, 0);
        lookAngle = new Vector3(0, 0, 0);

        flipped = false;

        if (Main.runningOnPC)
        {
            joystick.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (living)
        {
            moveInput.x = moveInput.y = 0;
            mousePos.x = mousePos.y = 0;
            if (Main.runningOnPC)
            {
                //moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                moveInput.x = Input.GetAxisRaw("Horizontal");
                moveInput.y = Input.GetAxisRaw("Vertical");
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                lookDir = mousePos - rb.position;
                lookAngle.z = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                gun.transform.localRotation = Quaternion.Euler(lookAngle);
                if (moveInput.x > 0 && !flipped) flipPlayer(-1);
                if (moveInput.x < 0 && flipped) flipPlayer(1);
                if (mousePos.x > transform.position.x && !flipped) flipPlayer(-1);
                if (mousePos.x < transform.position.x && flipped) flipPlayer(1);
            }
            else
            {
                /*moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
                moveInput.x = joystick.Horizontal;*/
                if(Mathf.Abs(joystick.Horizontal) > .2f) moveInput.x = joystick.Horizontal;
                else moveInput.x = 0;

                if(Mathf.Abs(joystick.Vertical) > .2f) moveInput.y = joystick.Vertical;
                else moveInput.y = 0;

                if (moveInput.x > 0 && !spriteRenderer.flipX) spriteRenderer.flipX = true;
                if (moveInput.x < 0 && spriteRenderer) spriteRenderer.flipX = false;
            }

            moveVelocity = moveInput.normalized * speed;

            if (moveInput.x != 0 || moveInput.y != 0) animator.SetFloat("speed", 1);
            else animator.SetFloat("speed", 0);
        }
    }

    private void FixedUpdate()
    {
        if (living)
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }

    }


    public void flipPlayer(int flip)
    {
        if (flip < 0)
        {
            flipped = true;
            spriteRenderer.flipX = true;
            gunSpriteRenderer.flipX = true;
        }
        else
        {
            flipped = false;
            spriteRenderer.flipX = false;
            gunSpriteRenderer.flipX = false;
        }
    }

    public void updateLifePoints(int points)
    {
        this.lifePoints += points;
        if (lifePoints >= maxLifePoints) lifePoints = maxLifePoints;
        updateHealthUI();
        if(lifePoints == 0)
        {
            //Kill player
            
            restart.gameObject.SetActive(true);
            living = false;
            gun.gameObject.SetActive(false);
            animator.SetBool("death", true);
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void updateHealthUI()
    {
        slider.value = this.lifePoints;
    }

}
