using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWindow : GUIBase
{
    public override void Show()
    {
        base.Show();
        Rect.offsetMin = Vector2.zero;
        Rect.offsetMax = Vector2.zero;
    }
}
