using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIRecord : GUIWindow
{
    public Button BtnPrev;

    public GUIRecordItem ItemPrefab;

    PlayerRecrodList recordList;

    List<GUIRecordItem> items;

    void Start()
    {
        BtnPrev.onClick.AddListener(Hide);
    }

    public override void Show()
    {
        base.Show();
        recordList = FileManager.Inst.Load<PlayerRecrodList>("record");

        if (recordList != null)
        {
            recordList.Sort();
            ClearItems();
            List<PlayerRecord> records = recordList.Records;

            for (int i = 0; i < records.Count; i++)
            {

                int rank = i + 1;
                GUIRecordItem newItem = Instantiate(ItemPrefab);
                newItem.gameObject.SetActive(true);
                newItem.transform.SetParent(ItemPrefab.transform.parent);

                newItem.SetValue(rank, records[i]);
                newItem.transform.localScale = Vector3.one;

                AddItem(newItem);
            }
        }
    }

    void AddItem(GUIRecordItem item)
    {
        if (items == null)
            items = new List<GUIRecordItem>();

        items.Add(item);
    }

    void ClearItems()
    {
        if (items != null)
        {
            for (int i = items.Count - 1; i > 0; i--)
            {
                Destroy(items[i]);
            }
            items.Clear();
        }
    }

    public override void Hide()
    {
        base.Hide();
    }
}
