using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public Vector2Int currentLocation;

    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private float speed = 0f;

    public Rigidbody2D rb = null;

    private Vector2 moveVelocity;
    private Vector2 mousePos;
    private Vector2 moveInput;
    private Vector2 lookDir;

    [SerializeField]
    private Camera mainCamera = null;
    
    public GameObject gun;

    public int lifePoints;
    private int maxLifePoints = 100;

    [SerializeField]
    private Slider slider = null;
    [SerializeField]
    private Joystick joystick = null;

    public bool living;
    public bool targeting;

    public Button restart;

    public SpriteRenderer spriteRenderer;
    private SpriteRenderer gunSpriteRenderer;

    private bool flipped;

    private Vector3 lookAngle;
    [SerializeField]
    private Image ammoImage = null;

    private void Awake()
    {
        setPlayerComponents();
    }

    void Start()
    {
        initPlayer();
    }

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;
        if (living)
        {
            moveInput.x = moveInput.y = 0;
            mousePos.x = mousePos.y = 0;
            if (GameController.Instance.runningOnPC)
            {
                desktopInput();
            }
            else
            {
                mobileInput();
            }
            moveVelocity = moveInput.normalized * speed;
            setWalkingAnimation();
        }
    }

    private void setWalkingAnimation()
    {
        if (moveInput.x != 0 || moveInput.y != 0) animator.SetFloat("speed", 1);
        else animator.SetFloat("speed", 0);
    }

    private void desktopInput()
    {
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

    private void mobileInput()
    {
        if (Mathf.Abs(joystick.Horizontal) > .2f) moveInput.x = joystick.Horizontal;
        else moveInput.x = 0;

        if (Mathf.Abs(joystick.Vertical) > .2f) moveInput.y = joystick.Vertical;
        else moveInput.y = 0;

        if (moveInput.x > 0 && !spriteRenderer.flipX) spriteRenderer.flipX = true;
        if (moveInput.x < 0 && spriteRenderer) spriteRenderer.flipX = false;
    }

    private void initPlayer()
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

        if (GameController.Instance.runningOnPC)
        {
            joystick.gameObject.SetActive(false);
            ammoImage.gameObject.SetActive(true);
        }
    }

    private void setPlayerComponents()
    {
        joystick = PlayerComponents.Instance.movementJoystick;
        slider = PlayerComponents.Instance.slider;
        restart = PlayerComponents.Instance.restartButton;
        ammoImage = PlayerComponents.Instance.ammoImage;
        /*mainCamera = PlayerComponents.Instance.mainCamera;
        mainCamera.GetComponent<CameraFollow>().target = transform;*/
    }

    private void FixedUpdate()
    {
        if (living)
        {
            rb.MovePosition(rb.position + moveVelocity * Time.deltaTime);
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
        else if(lifePoints <= 0)
        {
            //Kill player
            lifePoints = 0;
            living = false;
            gun.gameObject.SetActive(false);
            animator.SetBool("death", true);

            restart.gameObject.SetActive(true);
        }
        updateHealthUI();
    }

    private void updateHealthUI()
    {
        slider.value = this.lifePoints;
    }

    /*
    [PunRPC]
    public void setStartPosition(Vector3 startPos)
    {   
        Debug.Log("startPos: " + startPos);
        transform.localPosition = startPos;
        Debug.Log("Change via RPC: " + transform.localPosition);
    }*/

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(spriteRenderer.flipX);
        }else if (stream.IsReading)
        {
            spriteRenderer.flipX = (bool)stream.ReceiveNext();
        }
    }
}
