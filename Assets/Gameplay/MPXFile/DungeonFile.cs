using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPXObject;
using System;

namespace MPXFile
{
    [Serializable]
    public class DungeonFile : CustomFile
    {
        public int WorldSize;
        public int SwitchCount;
        public List<DungeonTileData> Datas;

        public DungeonFile() : base() { }

        public DungeonFile(TileCreator tileCreator)
        {
            WorldSize = tileCreator.WorldSize;
            Tile[,] tiles = tileCreator.Tiles;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int k = 0; k < tiles.GetLength(1); k++)
                {
                    if (tiles[i, k] != null)
                    {
                        AddTile(tiles[i, k]);
                    }
                }
            }
        }

        public DungeonFile Clone()
        {
            return (DungeonFile)this.MemberwiseClone();
        }

        void AddTile(Tile tile)
        {
            if (Datas == null)
                Datas = new List<DungeonTileData>();

            Datas.Add(new DungeonTileData(tile));
        }
    }
}
