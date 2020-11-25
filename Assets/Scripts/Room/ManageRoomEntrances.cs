using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageRoomEntrances : MonoBehaviour
{
    private Room room;

    private void Start()
    {
        room = GetComponent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        }
    }
}
