using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIRecordItem : GUIItem
{
    public Text TxtRank;
    public Text TxtName;
    public Text TxtRecord;

    public void SetValue(int rank, PlayerRecord record)
    {
        TxtRank.text = rank.ToString();
        TxtName.text = record.PlayerName;
        TxtRecord.text = record.Record.ToString("HH:mm:ss");
    }
}
