using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController = null;

    private Vector2 moveVelocity = new Vector2(0,0);
    [SerializeField]
    private float speed = 0;
    private bool flipped = false;

    private void Update()
    {
        moveVelocity = playerController.playerInput.moveInput.normalized * speed;
        managePlayerDirection();
    }

    private void FixedUpdate()
    {
        if (playerController.living)
        {
            playerController.rb.MovePosition(playerController.rb.position + moveVelocity * Time.deltaTime);
        }
    }

    #region Private Methods

    private void managePlayerDirection()
    {
        if (playerController.playerInput.moveInput.x > 0 && !flipped) flipPlayer(-1);
        if (playerController.playerInput.moveInput.x < 0 && flipped) flipPlayer(1);
        if (!playerController.playerInput.isMobileInput)
        {
            if (playerController.playerInput.mousePos.x > transform.position.x && !flipped) flipPlayer(-1);
            if (playerController.playerInput.mousePos.x < transform.position.x && flipped) flipPlayer(1);
        }
    }

    private void flipPlayer(int flip)
    {
        if (flip < 0)
        {
            flipped = true;
            playerController.spriteRenderer.flipX = true;
            playerController.gunSpriteRenderer.flipX = true;
        }
        else
        {
            flipped = false;
            playerController.spriteRenderer.flipX = false;
            playerController.gunSpriteRenderer.flipX = false;
        }
    }

    #endregion

}
