using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject waitingPanel = null;
    [SerializeField]
    private GameObject gameStartedPanel = null;
    [SerializeField]
    private GameObject votingPanel = null;

    private static GameUIManager instance = null;

    public int currentState { get; private set; }

    public static GameUIManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }


    private void Start()
    {
        resetPanels();
        setState(GameUIStates.WAITING);
    }

    #region Private methods

    private void resetPanels()
    {
        waitingPanel.SetActive(false);
        gameStartedPanel.SetActive(false);
        votingPanel.SetActive(false);
    }

    #endregion

    #region Public methods

    public void setState(int state)
    {
        resetPanels();
        currentState = state;
        switch (currentState)
        {
            case GameUIStates.WAITING:
                waitingPanel.SetActive(true);
                break;
            case GameUIStates.GAME_HAS_STARTED:
                gameStartedPanel.SetActive(true);
                break;
            case GameUIStates.VOTING:
                votingPanel.SetActive(true);
                break;
        }
    }

    #endregion
}
