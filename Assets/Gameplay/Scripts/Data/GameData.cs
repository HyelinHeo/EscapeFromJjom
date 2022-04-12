using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public string ID;

    public GameData()
    {
        ID = NewID();
    }

    public virtual string NewID()
    {
        return Guid.NewGuid().ToString();
    }
}
