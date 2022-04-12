using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MPXObject;

public class Tile
{
    public int ID;
    public List<int> Indexes;

    public TileGroup Group;

    public bool Overlap;

    public Tile[] LinkList;

    public Tile[] NearList;

    public int UnLinkCount
    {
        get
        {
            int count = 0;

            if (LinkList != null && LinkList.Length > 0)
            {
                for (int i = 0; i < LinkList.Length; i++)
                {
                    if (LinkList[i] == null)
                        count++;
                }
            }
            return count;

        }
    }

    public bool StartTile;
    public bool EndTile;

    public string Name;

    public Vector2 CenterPosition;
    public Vector2 MyDirection;

    public Tile()
    {
        Indexes = new List<int>();
    }

    public Tile(int index) : this()
    {
        AddIndex(index);
        LinkList = new Tile[4];
        NearList = new Tile[4];
        Overlap = false;
    }

    public Tile(DungeonTileData data)
    {
        ID = data.LineID;
        Name = data.Name;
        Indexes = data.Indexes;

        Overlap = data.Overlap;
        StartTile = data.StartTile;
        EndTile = data.EndTile;

        LinkList = new Tile[4];
        NearList = new Tile[4];

        CenterPosition = new Vector2(data.Pos.X, data.Pos.Y);
        MyDirection = new Vector2(data.MyDirection.X, data.MyDirection.Y);
    }

    public int GetLastIndex()
    {
        if (Indexes != null && Indexes.Count > 0)
        {
            return Indexes[Indexes.Count - 1];
        }
        return -1;
    }

    public int GetFirstIndex()
    {
        if (Indexes != null && Indexes.Count > 0)
        {
            return Indexes[0];
        }
        return -1;
    }

    public int GetIndex(int index)
    {
        if (Indexes != null)
        {
            return Indexes.Find(o => o == index);
        }

        return -1;
    }

    void AddIndex(int index)
    {
        if (Indexes != null)
        {
            Indexes.Add(index);
        }
    }

    //public Tile GetNext()
    //{
    //    for (int i = 0; i < LinkList.Length; i++)
    //    {
    //        if (LinkList[i] != null && LinkList[i].Indexes > Indexes)
    //        {
    //            return LinkList[i];
    //        }
    //    }
    //    return null;
    //}

    public void SetOverlap(bool overlap, Tile prevTile, int index)
    {
        if (overlap)
        {
            AddIndex(index);

            Name += "_" + index.ToString();

            prevTile.Connect(this, prevTile.MyDirection);
        }
        else
        {
            Name = Indexes.ToString();
        }

        Overlap = overlap;

    }

    //public Tile GetPrev()
    //{
    //    for (int i = 0; i < LinkList.Length; i++)
    //    {
    //        if (LinkList[i] != null && LinkList[i].Indexes < Indexes)
    //        {
    //            return LinkList[i];
    //        }
    //    }
    //    return null;
    //}


    /// <summary>
    /// 해당 방향이 연결되어 있는지 여부
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public bool IsConnected(Direction dir)
    {
        return IsConnected((int)dir);
    }

    public bool IsConnected(int dir)
    {
        if (LinkList.Length > dir)
        {
            return LinkList[dir] != null;
        }
        return false;
    }

    public List<int> GetUnLinkList()
    {
        List<int> directions = new List<int>();
        if (LinkList != null)
        {
            for (int i = 0; i < LinkList.Length; i++)
            {
                if (LinkList[i] == null)
                    directions.Add(i);
            }
        }

        return directions;
    }

    public int GetLink()
    {
        if (LinkList != null)
        {
            for (int i = 0; i < LinkList.Length; i++)
            {
                if (LinkList[i] != null)
                    return i;
            }
        }
        return -1;
    }


    public void Connect(Tile linkTile, Vector2 direction)
    {
        int idx = ConvertTo(direction);
        int linkIdx = ConvertTo(direction * -1);

        LinkList[idx] = linkTile;
        linkTile.LinkList[linkIdx] = this;

        SetNear(linkTile, direction);
    }

    public void SetNear(Tile nearTile, Vector2 direction)
    {
        int idx = ConvertTo(direction);
        int linkIdx = ConvertTo(direction * -1);

        NearList[idx] = nearTile;
        nearTile.NearList[linkIdx] = this;
    }

    int ConvertTo(Vector2 direction)
    {
        if (direction == Vector2.right)
        {
            return (int)Direction.RIGHT;
        }
        else if (direction == Vector2.left)
        {
            return (int)Direction.LEFT;

        }
        else if (direction == Vector2.down)
        {
            return (int)Direction.BACK;
        }
        else
        {
            return (int)Direction.FORWARD;
        }
    }
}
