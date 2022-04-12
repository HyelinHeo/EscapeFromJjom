using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUIProgressbar : GUIItem
{
    public Image ImgBar;

    public float ProgressBarValue
    {
        get { return ImgBar.fillAmount; }
    }

    public int CurrentValue;
    public int MaxValue = 100;
    public int MinValue = 0;

    public UnityEvent<float> OnChangedValue = new UnityEvent<float>();
    public UnityEvent OnChangedZero = new UnityEvent();

    public void Init(int max, int min = 0)
    {
        MaxValue = max;
        MinValue = min;

        CurrentValue = MaxValue;
    }

    public override void Awake()
    {
        base.Awake();
        CurrentValue = MaxValue;
    }


    /// <summary>
    /// min - max 사이의 값
    /// </summary>
    /// <param name="value"></param>
    public virtual void SetValue(int value)
    {
        SetValue((float)value / MaxValue);
    }

    public virtual void AddValue(float value)
    {
        SetValue(ProgressBarValue + value);
    }

    public virtual void AddValue(int value)
    {
        SetValue((float)(CurrentValue + value) / MaxValue);
    }

    /// <summary>
    /// 0 - 1사이의 값
    /// </summary>
    /// <param name="pValue"></param>
    public virtual bool SetValue(float pValue)
    {
        if (pValue != ProgressBarValue)
        {
            if (pValue > 1f)
            {
                pValue = 1f;
            }
            else if (pValue < 0f)
            {
                pValue = 0f;
            }

            ImgBar.fillAmount = pValue;
            OnChangedValue.Invoke(pValue);
            if (pValue <= 0)
                OnChangedZero.Invoke();

            CurrentValue = (int)(pValue * MaxValue);
            return true;
        }
        return false;
    }
}
