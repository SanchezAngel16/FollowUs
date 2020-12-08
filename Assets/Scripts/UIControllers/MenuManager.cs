using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField]
    private GameObject mainMenu = null;
    [SerializeField]
    private GameObject singlePlayerMenu = null;
    [SerializeField]
    private GameObject multiplayerMenu = null;
    [SerializeField]
    private GameObject newsMenu = null;
    [SerializeField]
    private GameObject storeMenu = null;
    [SerializeField]
    private GameObject howToPlayMenu = null;
    [SerializeField]
    private GameObject lobbyPanel = null;


    void Start()
    {
        resetMenus();
        setState(MenuStatus.MAIN);
    }

    private void resetMenus()
    {
        mainMenu.SetActive(false);
        singlePlayerMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
        newsMenu.SetActive(false);
        storeMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    public void setState(int state)
    {
        resetMenus();
        switch (state)
        {
            case MenuStatus.MAIN:
                mainMenu.SetActive(true);
                break;
            case MenuStatus.SINGLE_PLAYER:
                singlePlayerMenu.SetActive(true);
                break;
            case MenuStatus.MULTIPLAYER:
                multiplayerMenu.SetActive(true);
                break;
            case MenuStatus.NEWS:
                newsMenu.SetActive(true);
                break;
            case MenuStatus.STORE:
                storeMenu.SetActive(true);
                break;
            case MenuStatus.HOW_TO_PLAY:
                howToPlayMenu.SetActive(true);
                break;
            case MenuStatus.LOBBY:
                lobbyPanel.SetActive(true);
                break;
        }
    }
}
