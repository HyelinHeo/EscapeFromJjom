using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GUIProgressbar))]
public class GUIProgressBarEditor : GUIBaseEditor
{
    private GUIProgressbar gui;

    private void OnEnable()
    {
        gui = target as GUIProgressbar;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
