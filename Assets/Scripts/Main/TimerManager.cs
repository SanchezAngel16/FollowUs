using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    public event EventHandler OnTimerRanOut;

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
                OnTimerRanOut?.Invoke(this, EventArgs.Empty);
                timer = 0;
                run = false;
            }
        }
    }
}
