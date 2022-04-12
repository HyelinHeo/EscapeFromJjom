using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlayerStamina : GUIItem
{
    public GUIProgressbar GuiStamina;

    public int DecreaseValue;
    public int IncreaseValue;
    private Coroutine coroutine;

    public int Stamina
    {
        get { return GuiStamina.CurrentValue; }
    }

    public float StaminaValue
    {
        get { return GuiStamina.ProgressBarValue; }
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        coroutine = null;
        GuiStamina.Init(2000);
    }

    public void Use(int value)
    {
        GuiStamina.AddValue(-value);
    }

    public void Recovery(int value)
    {
        GuiStamina.AddValue(value);
    }

    public void StopDecrease()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void RunDecrease()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(StartDecrease());
        }
    }

    IEnumerator StartDecrease()
    {
        while (true)
        {
            Use(DecreaseValue);
            yield return null;
        }
    }

    public void StopIncrease()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void RunIncrease()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(StartIncrease());
        }
    }

    IEnumerator StartIncrease()
    {
        while (true)
        {
            Recovery(IncreaseValue);
            yield return null;
        }
    }
}
