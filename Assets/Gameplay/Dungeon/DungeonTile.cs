using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTile : MonoBehaviour
{
    public int Index;
    public int SelectType;

    public DungeonTileWall[] Walls;
    private Tile tileData;
    public Tile Tile
    {
        get { return tileData; }
    }

    public bool IsGroup;

    public bool Loaded;

    //public DungeonTile[] LinkList;

    public Vector2 Size;

    void Awake()
    {
        //LinkList = new DungeonTile[Walls.Length];
    }

    void ShowWall(int index)
    {
        Walls[index].Show();
    }

    public void HideWall(int index)
    {
        Walls[index].Hide();
    }

    public void ShowWallAll()
    {
        for (int i = 0; i < Walls.Length; i++)
        {
            Walls[i].Show();
        }
    }

    public void HideWallAll()
    {
        for (int i = 0; i < Walls.Length; i++)
        {
            Walls[i].Hide();
        }
    }

    private void OnDrawGizmos()
    {
        if (tileData != null && tileData.Overlap)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 1f);
        }

    }

    public void Init(Tile tile, Vector3 offset)
    {
        tileData = tile;
        //SelectType = tile.SelectType;
        SelectType = 0;
        for (int i = 0; i < Walls.Length; i++)
        {
            if (tileData.LinkList[i] != null)
            {
                Walls[i].Hide();
            }
            else
            {
                Walls[i].Show();
            }
        }

        transform.localEulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(tile.CenterPosition.x - offset.x, 0, tile.CenterPosition.y - offset.z);
        Index = tile.GetFirstIndex();
        name = tile.Name;
        //ClearLinkList();
    }

    //public DungeonTile GetLinkTile()
    //{
    //    int index = tileData.GetLink();
    //}

    //void ClearLinkList()
    //{
    //    if (LinkList != null)
    //    {
    //        LinkList = new DungeonTile[Walls.Length];
    //    }
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
        return tileData.IsConnected(dir);
    }

 

    //public void Connect(DungeonTile linkTile, Direction direction)
    //{
    //    int dir = (int)direction;
    //    int linkDirection = -1;

    //    LinkList[dir] = linkTile;

    //    switch (direction)
    //    {
    //        case Direction.LEFT:
    //            linkDirection = (int)Direction.RIGHT;
    //            break;
    //        case Direction.RIGHT:
    //            linkDirection = (int)Direction.LEFT;
    //            break;
    //        case Direction.BACK:
    //            linkDirection = (int)Direction.FORWARD;
    //            break;
    //        case Direction.FORWARD:
    //            linkDirection = (int)Direction.BACK;
    //            break;
    //        default:
    //            break;
    //    }
    //    linkTile.LinkList[linkDirection] = this;

    //    //Debug.LogFormat("connect: [{0}] {1},{2}", name, dir, linkDirection);

    //    HideWall(dir);
    //    linkTile.HideWall(linkDirection);
    //}
}
