using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GUIBase))]
public class GUIBaseEditor : Editor
{
    GUIBase guiBase;

    private void OnEnable()
    {
        if (guiBase == null)
            guiBase = (GUIBase)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (guiBase == null) return;

        if (GUILayout.Button("Show/Hide"))
        {
            if (!guiBase.isShow)
                guiBase.Show();
            else
                guiBase.Hide();
        }
    }

}
