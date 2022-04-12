using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MPXObject
{
    [System.Serializable]
    public class DungeonTileData : Object
    {
        public int LineID;
        public List<int> Indexes;

        public bool Overlap;
        public bool StartTile;
        public bool EndTile;
        public int TileType;
        public bool[] LinkList;
        public bool[] NearList;
        public Point2 Pos;
        public Point2 MyDirection;


        public DungeonTileData() : base() { }

        public DungeonTileData(Tile tile) : this()
        {
            LineID = tile.ID;
            Name = tile.Name;
            Vector2 pos = tile.CenterPosition;
            Pos = new Point2(pos.x, pos.y);
            MyDirection = new Point2(tile.MyDirection.x, tile.MyDirection.y);
            Overlap = tile.Overlap;
            StartTile = tile.StartTile;
            EndTile = tile.EndTile;
            ID = string.Format("{0}", Pos);

            LinkList = new bool[tile.LinkList.Length];
            NearList = new bool[tile.NearList.Length];

            for (int i = 0; i < tile.LinkList.Length; i++)
            {
                LinkList[i] = tile.LinkList[i] != null;
                NearList[i] = tile.NearList[i] != null;
            }
        }
    }
}