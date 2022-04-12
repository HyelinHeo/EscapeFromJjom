using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStatus
{
    NORMAL,
    FEAR
}


public class GUIPlayer : GUIItem
{
    public Text TxtPlayerName;
    public Image ImgPlayerImage;

    public GUIPlayerHP GuiHP;
    public GUIPlayerStamina GuiStamina;

    public PlayerStatus myStatus;

    public int Stamina { get { return GuiStamina.Stamina; } }
    public float StaminaValue { get { return GuiStamina.StaminaValue; } }
    public int HP { get { return GuiHP.HP; } }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        myStatus = PlayerStatus.NORMAL;

        GuiHP.Init();
        GuiStamina.Init();
    }

    public void SetStatus(PlayerStatus status)
    {
        if (status != myStatus)
        {
            myStatus = status;
            GuiHP.SetStatus(myStatus);
        }
    }

    public void SetPlayerName(string playerName)
    {
        TxtPlayerName.text = playerName;
    }
}
