using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreatorTile))]
public class CreatorTileEditor : Editor
{
    CreatorTile creator;


    private void OnEnable()
    {
        creator = target as CreatorTile;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (creator == null) return;

        if (GUILayout.Button("ReCreate"))
        {
            creator.ClearMap();
            creator.CreateNewMap();
        }

        if (GUILayout.Button("Clear"))
        {
            creator.ClearMap();
        }

        if (GUILayout.Button("Show Tile"))
        {
            creator.ShowTileAll();
        }


     
    }
}
