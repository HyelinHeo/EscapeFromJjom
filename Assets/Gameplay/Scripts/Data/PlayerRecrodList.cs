using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerRecrodList : GameData
{
    public List<PlayerRecord> Records;

    public PlayerRecrodList() : base()
    {
        Records = new List<PlayerRecord>();
    }

    public void Add(PlayerRecord record)
    {
        if (Records == null)
            Records = new List<PlayerRecord>();

        Records.Add(record);
    }

    public void Sort()
    {
        if (Records != null)
        {
            Records.Sort((a, b) => a.Record.CompareTo(b.Record));
        }
    }

    public void Add(string playerName, DateTime time)
    {
        PlayerRecord record = new PlayerRecord();
        record.PlayerName = playerName;
        record.RecordTime = DateTime.Now;
        record.Record = time;

        Add(record);
    }
}
