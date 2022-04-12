using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GUIQuestionDungeonName : GUIWindow
{
    public InputField InputName;
    public Button BtnOK;

    public UnityEvent<string> OnClickSaveName = new UnityEvent<string>();

    void Start()
    {
        BtnOK.onClick.AddListener(SaveNameOK);
    }

    public void SaveNameOK()
    {
        OnClickSaveName.Invoke(InputName.text);
    }
}
