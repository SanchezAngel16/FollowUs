using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject mainCamera = null;
    [SerializeField]
    private GameObject roomsPreview = null;
    void Start()
    {
        if (photonView.IsMine)
        {
            /*transform.GetComponent<PlayerController>().enabled = true;
            transform.GetComponent<Shooting>().enabled = true;*/
            transform.GetChild(0).GetComponent<PlayerCollider>().enabled = true;
            mainCamera.GetComponent<Camera>().enabled = true;
            mainCamera.GetComponent<AudioListener>().enabled = true;
            roomsPreview.SetActive(true);
        }
        else
        {
            /*transform.GetComponent<PlayerController>().enabled = false;
            transform.GetComponent<Shooting>().enabled = false;*/
            transform.GetChild(0).GetComponent<PlayerCollider>().enabled = false;
            mainCamera.GetComponent<Camera>().enabled = false;
            mainCamera.GetComponent<AudioListener>().enabled = false;
            roomsPreview.SetActive(false);
        }
    }
}
