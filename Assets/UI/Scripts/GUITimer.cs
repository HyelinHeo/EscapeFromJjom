using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GUITimer : GUIItem
{
    public Text TxtTimer;

    private float currentTime;
    private int seconds;
    private int minutes;
    private int hours;

    public string TimeRecord { get { return TxtTimer.text; } }

    public bool Playing;

    public override void Awake()
    {
        base.Awake();
        ResetTimer();
    }

    public DateTime GetTime()
    {
        return new DateTime(2000, 1, 1, hours, minutes, seconds);
    }

    void SetTime()
    {
        seconds = (int)currentTime;
        hours = (int)(Math.Floor(currentTime / 3600));
        seconds = seconds % 3600;
        minutes = (int)(Math.Floor((double)(seconds / 60)));
        seconds = seconds % 60;

        TxtTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

    private void Update()
    {
        if (Playing)
        {
            currentTime += Time.deltaTime;
            SetTime();
        }
    }

    public void Play()
    {
        Playing = true;
    }

    public void Stop()
    {
        Playing = false;
    }

    public void ResetTimer()
    {
        Playing = false;
        currentTime = 0;
    }
}
