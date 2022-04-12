using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileGroup
{
    public int ID;

    private Tile tile;
    public Tile Tile { get { return tile; } }

    private Vector2 centerPosition;
    public Vector2 CenterPosition;

    private Vector2 centerPos;

    public const int MIN_SIZE = 4;

    public static int MaxID = 0;

    public TileGroup(Vector2 defaultPos, Tile prevTile)
    {
        centerPos = defaultPos;
        ID = MaxID;
        MaxID += 1;
        tile = prevTile;
    }

    Vector2 GetLeft(Vector2 forward)
    {
        if (forward == Vector2.up)
        {
            return Vector2.left;
        }
        else if (forward == Vector2.right)
        {
            return Vector2.up;
        }
        else if (forward == Vector2.left)
        {
            return Vector2.down;
        }
        else
        {
            return Vector2.right;
        }
    }

    Vector2 GetRight(Vector2 forward)
    {
        if (forward == Vector2.up)
        {
            return Vector2.right;
        }
        else if (forward == Vector2.right)
        {
            return Vector2.down;
        }
        else if (forward == Vector2.left)
        {
            return Vector2.up;
        }
        else
        {
            return Vector2.left;
        }
    }

    public TileGroup(Vector2 defaultPos, Tile prevTile, int id, int index, int branchNo, Vector2 dir) : this(defaultPos, prevTile)
    {
        Vector2 pos = defaultPos;
        if (prevTile != null)
        {
            pos = prevTile.CenterPosition;
            pos += dir * MIN_SIZE;
        }

        tile = Create(dir, id, index + 1, branchNo, pos);
    }

    public bool SetTileMap(ref Tile[,] map)
    {
        int x = (int)tile.CenterPosition.x;
        int y = (int)tile.CenterPosition.y;

        if (map[x, y] == null)
            map[x, y] = tile;
        else
        {
            Debug.LogErrorFormat("{0}", map[x, y].GetFirstIndex());
            Debug.LogErrorFormat("{0},{1},{2}", tile.GetFirstIndex(), x, y);
        }
        return true;
    }

    public Direction ConvetTo(Vector2 dir)
    {
        if (dir == Vector2.up)
        {
            return Direction.FORWARD;
        }
        else if (dir == Vector2.right)
        {
            return Direction.RIGHT;
        }
        else if (dir == Vector2.left)
        {
            return Direction.LEFT;
        }
        else
        {
            return Direction.BACK;
        }
    }


    public Tile Create(Vector2 direction, int id, int index, int branchNo, Vector2 tilePos)
    {
        Tile newTile = new Tile(index);
        newTile.ID = id;
        newTile.MyDirection = direction;
        newTile.CenterPosition = tilePos;
        newTile.Group = this;
        newTile.Name = string.Format("{0}_{1}_{2}", id, index, branchNo);

        //Debug.LogFormat("Create: {0},{1},{2}", index, tilePos, direction);

        return newTile;
    }
}
