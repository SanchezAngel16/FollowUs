using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public TMP_Dropdown mapSize;
    public void startGame()
    {
        Debug.Log(mapSize.value);
        if(mapSize.value != 0)
        {
            Util.mapSize = mapSize.value + 3;
            SceneManager.LoadScene(1);
        }
    }
}
