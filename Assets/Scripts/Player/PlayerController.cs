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

    public Camera mainCamera;

    public GameObject gun;

    public int lifePoints;
    public Image[] lifesImages = new Image[3];

    public Joystick joystick;

    public bool living;
    public bool targeting;

    public Button restart;
    void Start()
    {
        living = true;
        lifePoints = 3;
        rb = GetComponent<Rigidbody2D>();

        moveInput = new Vector2(0, 0);
        mousePos = new Vector2(0, 0);

        if (Main.runningOnPC)
        {
            Debug.Log("Desactivate");
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
                moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 lookDir = mousePos - rb.position;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                gun.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                if (moveInput.x > 0) flipPlayer(-1);
                if (moveInput.x < 0) flipPlayer(1);
                if (mousePos.x > transform.position.x) flipPlayer(-1);
                if (mousePos.x < transform.position.x) flipPlayer(1);
            }
            else
            {
                moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
                if(Mathf.Abs(joystick.Horizontal) > .2f) moveInput.x = joystick.Horizontal;
                else moveInput.x = 0;

                if(Mathf.Abs(joystick.Vertical) > .2f) moveInput.y = joystick.Vertical;
                else moveInput.y = 0;

                if (moveInput.x > 0) flipPlayer(-1);
                if (moveInput.x < 0) flipPlayer(1);
            }

            moveVelocity = moveInput.normalized * speed;
            /*if (moveInput.x > 0) flipPlayer(-1);
            if (moveInput.x < 0) flipPlayer(1);
            if (mousePos.x > transform.position.x) flipPlayer(-1);
            if (mousePos.x < transform.position.x) flipPlayer(1);*/
            

            
            // ROTATE GUN FROM JOYSTICK COORDINATES
            /*if((moveInput.x != 0 || moveInput.y != 0) && !targeting)
            {
                Vector2 lookDir = mousePos - rb.position;
                float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
                gun.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }*/



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
            
            restart.gameObject.SetActive(true);
            living = false;
            gun.gameObject.SetActive(false);
            animator.SetBool("death", true);
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene("SampleScene");
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
