using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MPXFile;
using System;

public class GUISelectDungeon : GUIBase
{
    public DungeonListItem ItemPrefab;
    public RectTransform Contents;

    public UnityEvent OnLoadFiled = new UnityEvent();

    List<DungeonListItem> items;

    private void Start()
    {
        ItemPrefab.gameObject.SetActive(false);
    }

    public override void Show()
    {
        base.Show();

        ClearList();
        LoadList();
    }

    void ClearList()
    {
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                DestroyImmediate(items[i].gameObject);
            }
            items.Clear();
            items = null;
        }
    }

    void AddItem(DungeonListItem item)
    {
        if (items == null)
            items = new List<DungeonListItem>();

        items.Add(item);
    }

    private void LoadList()
    {
        string folder = PathManager.DUNGEON_DIR;
        string[] fileNames = MPXFileManager.GetFiles(folder);
        if (fileNames != null && fileNames.Length > 0)
        {
            string fullPath = string.Empty;
            for (int i = 0; i < fileNames.Length; i++)
            {
                fullPath = fileNames[i];
                DungeonFile file = MPXFileManager.Load<DungeonFile>(fullPath);
                if (file != null)
                {
                    Debug.Log("Dungeon file load ok. " + fullPath);

                    DungeonListItem newItem = Instantiate(ItemPrefab);
                    newItem.transform.SetParent(ItemPrefab.transform.parent);
                    newItem.transform.localScale = Vector3.one;
                    newItem.transform.localEulerAngles = Vector3.zero;
                    newItem.Show(file, file.Name, "00:00:00");

                    newItem.OnClick.AddListener(() => OnClickItem(newItem));

                    AddItem(newItem);

                }
                else
                {
                    Debug.LogError("Dungeon file load filed. " + fullPath);
                }
            }

            Contents.sizeDelta = new Vector2(Contents.sizeDelta.x, ItemPrefab.Rect.sizeDelta.y * fileNames.Length);
        }
    }

    private void OnClickItem(DungeonListItem item)
    {
        TileImporter.Inst.CurrentFile = item.File.Clone();
        OnLoadFiled.Invoke();
        Hide();
    }
}
