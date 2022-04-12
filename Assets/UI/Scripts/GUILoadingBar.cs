using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUILoadingBar : GUIWindow
{
    public GUIProgressbar Bar;

    public OnClickMoveScene MoveScene;

    void Start()
    {
        Hide();

        MoveScene.LoadStarted.AddListener(Show);
        MoveScene.ChanegedPregressValue.AddListener(SetProgress);

        Bar.SetValue(0);
    }

    public void SetProgress(float value)
    {
        Bar.SetValue(value);
    }
}
