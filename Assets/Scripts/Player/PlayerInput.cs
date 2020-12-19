using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController = null;

    public bool isMobileInput { get; private set; }

    public Vector2 moveInput;
    public Vector2 mousePos;

    private Vector2 lookDir;
    private Vector3 lookAngle;

    [SerializeField]
    private Camera mainCamera = null;

    private Joystick joystick = null;

    #region Unity Methods

    private void Start()
    {
        moveInput = new Vector2(0, 0);
        mousePos = new Vector2(0, 0);
        lookDir = new Vector2(0, 0);
        lookAngle = new Vector3(0, 0, 0);

        joystick = PlayerComponents.Instance.movementJoystick;
        

        if (GameController.Instance.runningOnPC)
        {
            isMobileInput = false;
            joystick.gameObject.SetActive(false);
            playerController.ammoImage.gameObject.SetActive(true);
        }
        else isMobileInput = true;
    }

    private void Update()
    {
        if (playerController.living)
        {
            moveInput = Vector2.zero;
            mousePos = Vector2.zero;
            if (GameController.Instance.runningOnPC)
            {
                desktopInput();
            }
            else
            {
                mobileInput();
            }
            setWalkingAnimation();
        }
    }

    #endregion

    #region Private Methods

    private void desktopInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        lookDir = mousePos - playerController.rb.position;
        lookAngle.z = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        playerController.gun.transform.localRotation = Quaternion.Euler(lookAngle);
    }

    private void mobileInput()
    {
        if (Mathf.Abs(joystick.Horizontal) > .2f) moveInput.x = joystick.Horizontal;
        else moveInput.x = 0;

        if (Mathf.Abs(joystick.Vertical) > .2f) moveInput.y = joystick.Vertical;
        else moveInput.y = 0;

    }

    private void setWalkingAnimation()
    {
        if (moveInput.x != 0 || moveInput.y != 0) playerController.animator.SetFloat("speed", 1);
        else playerController.animator.SetFloat("speed", 0);
    }

    #endregion
}
