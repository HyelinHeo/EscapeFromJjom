using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerRecord : GameData
{
    public string PlayerName;

    /// <summary>
    /// 기록한 시간
    /// </summary>
    public DateTime RecordTime;

    /// <summary>
    /// 기록
    /// </summary>
    public DateTime Record;

    public PlayerRecord() : base()
    {
    }

    public override string ToString()
    {
        return string.Format("{0} {1} {2}", PlayerName, RecordTime, Record);
    }
}
