using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MasterClientManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PhotonView playerPhotonView = null;

    private void OnApplicationQuit()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            if (!GameController.Instance.runningOnPC)
            {
                disconnectFromServer();
            }
        }
    }

    private void OnApplicationFocus(bool focused)
    {
        if (!focused)
        {
            if (!GameController.Instance.runningOnPC)
            {
                disconnectFromServer();
            }
        }
    }

    private void disconnectFromServer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(playerPhotonView);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(0);
        }
    }
}
