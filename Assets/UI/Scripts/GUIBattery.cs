using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class GUIBattery : GUIProgressbar
{
    public Color Red;
    public Color Yellow;
    public Color Green;

    public enum BatteryStatus
    {
        GREEN,
        YELLOW,
        RED,
        OFF
    }

    public UnityEvent<BatteryStatus> OnChangedStatus = new UnityEvent<BatteryStatus>();

    /// <summary>
    /// 배터리 시간(초))
    /// </summary>
    public float FullTime = 180;
    public float UsedTime = 0;

    public BatteryStatus Status;
    public override void Show()
    {
        base.Show();
        Init((int)FullTime, 0);
        FullBattery();
    }

    public override bool SetValue(float pValue)
    {
        if (base.SetValue(pValue))
        {
            if (pValue <= 0)
            {
                SetStutus(BatteryStatus.OFF);
            }
            else if (pValue <= 0.2f)
            {
                ImgBar.color = Red;
                SetStutus(BatteryStatus.RED);
            }
            else if (pValue <= 0.35f)
            {
                ImgBar.color = Yellow;
                SetStutus(BatteryStatus.YELLOW);
            }
            else
            {
                ImgBar.color = Green;
                SetStutus(BatteryStatus.GREEN);
            }
        }
        return false;
    }

    void SetStutus(BatteryStatus status)
    {
        if (Status != status)
        {
            Status = status;
            OnChangedStatus.Invoke(Status);
        }
    }

    public void ChargeBattery(float value)
    {
        AddValue(value);
        UsedTime = FullTime * ProgressBarValue;

    }

    public void FullBattery()
    {
        SetValue(1f);
        UsedTime = FullTime;
    }

    private void Update()
    {
        if (UsedTime >= 0)
        {
            UsedTime -= Time.deltaTime;
            SetValue(UsedTime / FullTime);
        }
    }
}
