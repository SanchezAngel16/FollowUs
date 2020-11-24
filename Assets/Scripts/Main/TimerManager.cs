using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    private bool run = false;

    public float timer;

    public void startRunning(float maxTimer)
    {
        run = true;
        timer = maxTimer;
    }


    void Update()
    {
        if (run)
        {

            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                timer = 0;
                run = false;
            }
        }
    }
}
