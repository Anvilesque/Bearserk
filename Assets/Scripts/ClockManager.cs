using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public float counter;
    public float secCounter;
    public float interval = 20;
    public int lapsCount;
    public bool clockEnabled = true;

    public class ClockArgs : EventArgs {
        public float time;
    }
    public event System.EventHandler<ClockArgs> OnClockTick;
    public event System.EventHandler<EventArgs> OnClockBackToZero;
    
    void Start()
    {
        interval = 20;
    }

    void Update()
    {
        if (clockEnabled)
        {
            counter += Time.deltaTime;
            secCounter += Time.deltaTime;
            if (counter >= interval)
            {
                lapsCount += 1;
                counter -= interval;
                OnClockBackToZero?.Invoke(this, EventArgs.Empty);
            }
            if (secCounter >= 1)
            {
                secCounter -= 1;
                OnClockTick?.Invoke(this, new ClockArgs {time = Mathf.CeilToInt(counter)});
            }
        }
    }

    /// <summary>
    /// 0 to reset counter only, 1 to reset entire clock
    /// </summary>
    public void ResetClock(int value)
    {
        switch (value)
        {
            case 0:
                counter = 0;
                secCounter = 0;
                break;
            case 1:
                counter = 0;
                secCounter = 0;
                lapsCount = 0;
                break;
        }
    }

    public void EnableClock()
    {
        clockEnabled = true;
    }

    public void DisableClock()
    {
        clockEnabled = false;
    }
}
