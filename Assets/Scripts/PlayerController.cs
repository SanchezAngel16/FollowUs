using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Main gameController;
    public Vector2Int currentLocation;

    public float speed;

    public Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Vector2 mousePos;

    public Camera mainCamera;

    public GameObject gun;
    private int flipGunValue;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flipGunValue = 1;
    }

    
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        moveVelocity = moveInput.normalized * speed;
        if (moveInput.x > 0 || mousePos.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            flipGunValue = -1;
        }
        else if (moveInput.x < 0 || mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            flipGunValue = 1;
        }
        
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg -90f;
        gun.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle * flipGunValue));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Room"))
        {
            Debug.Log("Bla");
            mainCamera.GetComponent<CameraFollow>().moveCamera(collision.transform.position);

            Room currentRoom = collision.gameObject.GetComponent<Room>();
            currentLocation = currentRoom.mapLocation;

            if(Vector2Int.Equals(currentLocation, gameController.currentActiveRoom))
            {
                gameController.setActiveAllUIArrows(true);
                gameController.updateUIArrows();
            }
            else
            {
                gameController.setActiveAllUIArrows(false);
            }

            
        }
    }

}
