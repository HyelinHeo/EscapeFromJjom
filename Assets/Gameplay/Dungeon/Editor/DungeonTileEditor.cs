using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonTile))]
public class DungeonTileEditor : Editor
{
    DungeonTile tile;

    private void OnEnable()
    {
        tile = target as DungeonTile;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (tile == null) return;

        if (GUILayout.Button("Create"))
        {
        }
    }
}
