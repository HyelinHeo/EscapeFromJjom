using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPXFile;

public class TileImporter : Singleton<TileImporter>
{
    public DungeonFile CurrentFile;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
