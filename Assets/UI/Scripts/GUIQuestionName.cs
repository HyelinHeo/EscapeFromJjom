using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUIQuestionName : GUIWindow
{
    public OnClickMoveScene MoveScene;

    public InputField InputName;
    public Button BtnOK;

    public UnityEvent OnClickSaveName = new UnityEvent();

    void Start()
    {
        BtnOK.onClick.AddListener(SaveNameOK);
    }

    public void SaveNameOK()
    {
        PlayerPrefs.SetString(PlayerPrefManager.STR_PLAYER_NAME, InputName.text);
        PlayerPrefs.Save();

        OnClickSaveName.Invoke();
    }

    public override void Hide()
    {
        OnClickSaveName.RemoveAllListeners();

        base.Hide();
    }
}
