using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlayerHP : GUIItem
{
    public GUIProgressbar GuiHP;

    /// <summary>
    /// 소모 HP(배터리 0일때)
    /// </summary>
    public int DecreaseValue = 1;
    private WaitForSeconds waitDecrease = new WaitForSeconds(1);

    private Coroutine decreaseCoroutine;

    public int HP { get { return GuiHP.CurrentValue; } }

    void Start()
    {
        Init();
    }

    public void Use(int value)
    {
        GuiHP.AddValue(-value);
    }

    public void Recovery(int value)
    {
        GuiHP.AddValue(value);
    }

    public void Init()
    {
        decreaseCoroutine = null;
        GuiHP.SetValue(1f);
    }

    public void SetStatus(PlayerStatus status)
    {
        switch (status)
        {
            case PlayerStatus.NORMAL:
                StopDecrease();
                break;
            case PlayerStatus.FEAR:
                StartCoroutine(StartDecrease());
                break;
            default:
                break;
        }
    }

    void StopDecrease()
    {
        if (decreaseCoroutine != null)
        {
            StopCoroutine(decreaseCoroutine);
        }
    }

    public IEnumerator StartDecrease()
    {
        if (decreaseCoroutine == null)
        {
            while (true)
            {
                Use(DecreaseValue);
                yield return waitDecrease;
            }
        }
    }
}
