using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileEditorUtilWindow : EditorWindow
{
    CreatorTile creator;

    private void OnEnable()
    {
        creator = (CreatorTile)FindObjectOfType(typeof(CreatorTile));
    }

    [MenuItem("Test/Tile Selector")]
    static void Init()
    {
        TileEditorUtilWindow window = (TileEditorUtilWindow)EditorWindow.GetWindow(typeof(TileEditorUtilWindow));
        window.Show();
    }

    private void OnGUI()
    {
        if (creator == null)
            creator = (CreatorTile)FindObjectOfType(typeof(CreatorTile));

        if (GUILayout.Button("UnSelect All"))
        {
            Selection.activeObject = null;
        }

        if (GUILayout.Button("Select Next"))
        {
            if (Selection.activeObject == null)
            {
                DungeonTile tile = creator.FindFirst();
                Selection.activeObject = tile.gameObject;
            }
            else
            {
                DungeonTile dTile = Selection.activeGameObject.GetComponent<DungeonTile>();
                if (dTile != null)
                {
                    DungeonTile tile = creator.FindNextTile(dTile.Tile);
                    if (tile != null)
                    {
                        Selection.activeObject = tile.gameObject;
                    }
                }
            }
        }

        if (GUILayout.Button("Select Prev"))
        {
            if (Selection.activeObject == null)
            {
                DungeonTile tile = creator.FindLast();
                Selection.activeObject = tile.gameObject;
            }
            else
            {
                DungeonTile dTile = Selection.activeGameObject.GetComponent<DungeonTile>();
                if (dTile != null)
                {
                    DungeonTile tile = creator.FindPrevTile(dTile.Tile);
                    if (tile != null)
                    {
                        Selection.activeObject = tile.gameObject;
                    }
                }
            }
        }

    }
}
