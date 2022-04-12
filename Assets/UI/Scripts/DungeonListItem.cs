using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MPXFile;

public class DungeonListItem : GUIItem
{
    public Button Btn;

    public Text TxtName;
    public Text TxtTime;

    public Vector2 Size;

    public DungeonFile File;

    public UnityEvent OnClick { get { return Btn.onClick; } }

    public override void Show()
    {
        base.Show();
    }

    public void Show(DungeonFile file, string title, string time)
    {
        File = file;

        TxtName.text = title;
        TxtTime.text = time;

        Show();
    }
}
