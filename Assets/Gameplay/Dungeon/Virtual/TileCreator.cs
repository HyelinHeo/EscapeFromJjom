using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPXFile;
using MPXObject;

public enum UseState
{
    USED,
    UNUSED,
    OUTOFWOLRD
}


public class TileCreator
{
    private int worldSize = 100;
    private DungeonFile dungeonfile;
    public int WorldSize { get { return worldSize; } }

    public Tile[,] Tiles;

    private Dictionary<int, List<TileGroup>> groups;
    public Dictionary<int, List<TileGroup>> Groups;

    //public delegate void EventTile(Tile tile);
    //public event EventTile CreatedTile;

    //int index;
    //public int Index { get { return index; } }

    Vector2 centerPos;
    public Vector2 CenterPos { get { return centerPos; } }

    int ForwardPercent = 40;
    int LeftPercent = 30;
    int RightPercent = 30;

    int createdCount;
    const int START_TILE_COUNT = 2;

    public TileCreator(int world)
    {
        worldSize = world;
        Tiles = new Tile[worldSize * TileGroup.MIN_SIZE * 2, worldSize * TileGroup.MIN_SIZE * 2];
        ClearMap();

        centerPos = new Vector2(Tiles.GetLength(0) / 2, Tiles.GetLength(1) / 2);

        Debug.LogFormat("TileWorldSize {0}, {1}", Tiles.GetLength(0), Tiles.GetLength(1));
    }

    public TileCreator(DungeonFile file)
    {
        if (file != null)
        {
            dungeonfile = file;
            worldSize = file.WorldSize;
            Tiles = new Tile[worldSize * TileGroup.MIN_SIZE * 2, worldSize * TileGroup.MIN_SIZE * 2];
            ClearMap();
            centerPos = new Vector2(Tiles.GetLength(0) / 2, Tiles.GetLength(1) / 2);
            CreateLoadMap(file);
        }
    }

    private void CreateLoadMap(DungeonFile file)
    {
        List<DungeonTileData> tiles = file.Datas;
        if (tiles != null && tiles.Count > 0)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                DungeonTileData data = tiles[i];
                int x = (int)data.Pos.X;
                int y = (int)data.Pos.Y;
                Tiles[x, y] = new Tile(data);
            }

            // 연결관계 찾기
            for (int i = 0; i < tiles.Count; i++)
            {
                DungeonTileData data = tiles[i];
                int x = (int)data.Pos.X;
                int y = (int)data.Pos.Y;
                if (Tiles[x, y] != null)
                {
                    bool[] linkList = data.LinkList;
                    bool[] nearList = data.NearList;
                    for (int k = 0; k < linkList.Length; k++)
                    {
                        if (linkList[k])
                        {
                            Tiles[x, y].LinkList[k] = FindTile(Tiles[x, y], k);
                        }

                        if (nearList[k])
                        {
                            Tiles[x, y].NearList[k] = FindTile(Tiles[x, y], k);
                        }
                    }
                }
            }
        }
    }

    Tile FindTile(Tile tile, int dir)
    {
        if (tile != null)
        {
            Vector2 dirVec = ConvertToDirection(dir);
            Vector2 newPos = tile.CenterPosition + dirVec * TileGroup.MIN_SIZE;
            int x = (int)newPos.x;
            int y = (int)newPos.y;

            return Tiles[x, y];
        }
        return null;
    }

    void AddGroups(TileGroup group)
    {
        if (group != null)
        {
            if (groups == null)
                groups = new Dictionary<int, List<TileGroup>>();

            int key = group.Tile.ID;

            if (!groups.ContainsKey(key))
                groups[key] = new List<TileGroup>();

            groups[key].Add(group);
        }
    }


    /// <summary>
    /// 해당 타일이 막혀있는지 여부
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    bool IsBlockedTile(Tile tile)
    {
        Tile[] linkList = tile.LinkList;
        if (linkList != null && linkList.Length > 0)
        {
            for (int i = 0; i < linkList.Length; i++)
            {
                if (linkList[i] == null)
                {
                    if (UsedTile(tile.CenterPosition, ConvertToDirection(i)) == UseState.UNUSED)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }


    TileGroup GetRandomGroup(int key)
    {
        if (groups != null && groups.ContainsKey(key) && groups[key].Count > 0)
        {
            List<TileGroup> findGroups = groups[key].FindAll(o => !o.Tile.StartTile && !IsBlockedTile(o.Tile));
            if (findGroups != null)
            {
                int idx = Random.Range(0, findGroups.Count);
                return findGroups[idx];
            }
            else
            {
                Debug.LogErrorFormat("GetRandomGroup error. {0}, {1}", groups[key].Count, key);
            }
        }

        return null;
    }

    Vector2 ConvertToDirection(int dirIdx)
    {
        if (dirIdx == 0)
        {
            return Vector2.left;
        }
        else if (dirIdx == 1)
        {
            return Vector2.right;
        }
        else if (dirIdx == 2)
        {
            return Vector2.down;
        }
        else
        {
            return Vector2.up;
        }
    }

    Vector2 ConvertToDirection(Direction dir)
    {
        return ConvertToDirection((int)dir);
    }

    Tile GetRandomTile(int lineId)
    {
        if (groups != null && groups.Count > 0)
        {
            TileGroup grp = GetRandomGroup(lineId);
            if (grp != null)
            {
                Tile tile = grp.Tile;
                List<Vector2> dirs = CanGoDirections(tile.CenterPosition, tile.MyDirection);
                if (dirs.Count > 0)
                {
                    return tile;
                }
                else
                {
                    Debug.LogErrorFormat("get random tile error. {0}, {1}", tile.CenterPosition, tile.MyDirection);
                }
            }
        }
        return null;
    }

    void RemoveGroups()
    {
        if (groups != null)
        {
            groups.Clear();
        }
    }

    public void ClearMap()
    {
        if (Tiles != null)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    Tiles[i, k] = null;
                }
            }
        }

        RemoveGroups();
    }

    public bool CreateNewLine(Tile startTile, int id, int wayLength)
    {
        int index = -1;
        int createdCount = 0;
        Tile prevTile = startTile;
        Vector2 overlapDir = Vector2.zero;

        int branchNo = 0;

        while (index < wayLength)
        {
            List<Vector2> dirList = CanGoDirections(prevTile == null ? centerPos : prevTile.CenterPosition, prevTile == null ? Vector2.up : prevTile.MyDirection);
            Vector2 dir = Vector2.up;

            if (createdCount > START_TILE_COUNT || id > 0)
            {
                if (dirList.Count == 0)
                {
                    dir = Vector2.zero;
                }
                else
                {
                    int idx = Random.Range(0, dirList.Count);
                    dir = dirList[idx];
                }
            }

            if (dir != Vector2.zero)
            {
                TileGroup group = new TileGroup(centerPos, prevTile, id, index, branchNo, dir);
                if (!group.SetTileMap(ref Tiles))
                {
                    //Debug.LogErrorFormat("Tile error: {0}", prevTile.Indexes);
                    continue;
                }
                Tile newTile = group.Tile;
                if (prevTile != null)
                {
                    prevTile.Connect(newTile, dir);
                }

                if (prevTile == null || prevTile.ID != newTile.ID)
                {
                    newTile.StartTile = true;
                    //Debug.LogFormat("start tile {0}", newTile.Name);
                }

                overlapDir = Vector2.zero;
                index = group.Tile.GetFirstIndex();
                //Debug.LogFormat("created: {0}", index);
                createdCount++;
                prevTile = newTile;
                AddGroups(group);
            }
            else
            {
                if (prevTile != null)
                {
                    // 사방이 다 막힌 경우
                    Vector2 pos = MovePosition(overlapDir != Vector2.zero ? overlapDir : prevTile.MyDirection, prevTile.CenterPosition);
                    Tile tile = Tiles[(int)pos.x, (int)pos.y];
                    if (tile != null && id == tile.ID)
                    {
                        if (overlapDir == Vector2.zero)
                        {
                            overlapDir = prevTile.MyDirection;
                            tile.SetOverlap(true, prevTile, index + 1);

                            branchNo++;
                        }
                        tile.MyDirection = prevTile.MyDirection;
                        prevTile = tile;
                        index = prevTile.GetFirstIndex();

                        //Debug.LogWarningFormat("link tile. {0}, {1}, {2}", tile.ID, tile.Name, tile.CenterPosition);
                    }
                    else
                    {
                        Debug.LogFormat("limit Create Map. {0}, {1}, {2}", id, prevTile.CenterPosition, pos);
                        return false;
                    }
                }
            }
        }

        prevTile.EndTile = true;
        //Debug.LogFormat("EndTile {0}", prevTile.Name);

        return true;
    }


    public void CreateNewMap(int subWayCount)
    {
        if (!CreateNewLine(null, 0, worldSize))
        {
            Debug.LogError("CreateNewLine error");
        }
        else
        {
            if (subWayCount > 0)
            {
                for (int i = 0; i < subWayCount; i++)
                {
                    Tile startTile = GetRandomTile(0);
                    if (startTile != null)
                    {
                        CreateNewLine(startTile, i + 1, worldSize / 2);
                    }
                    else
                    {
                        Debug.LogErrorFormat("Create Sub way error. {0}", i + 1);
                    }
                }
            }

            Debug.Log("CreateNewMap Complete");
        }
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

    int GetPercent(Vector2 dir)
    {
        Direction direction = Direction.BLOCKED;
        if (dir == Vector2.up)
        {
            direction = Direction.FORWARD;
        }
        else if (dir == Vector2.right)
        {
            direction = Direction.RIGHT;
        }
        else if (dir == Vector2.left)
        {
            direction = Direction.LEFT;
        }
        else
        {
            direction = Direction.BACK;
        }

        return GetPercent(direction);
    }

    int GetPercent(Direction dir)
    {
        switch (dir)
        {
            case Direction.LEFT:
                return LeftPercent;
            case Direction.RIGHT:
                return RightPercent;
            case Direction.FORWARD:
                return ForwardPercent;
            default:
                break;
        }
        return 0;
    }

    int GetRandomTileCount(Tile prevTile)
    {
        return 1;
    }

    int GetRandomSwitchCount()
    {
        int rand = Random.Range(0, 100);
        if (rand < 80)
        {
            return 1;
        }
        else if (rand < 95)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }


    /// <summary>
    /// 해당 위치에서 충돌하지 않는 방향 가져오기
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="forward"></param>
    /// <returns></returns>
    List<Vector2> CanGoDirections(Vector2 pos, Vector2 forward)
    {
        List<Vector2> list = new List<Vector2>();

        Vector2 dir = forward;
        if (UsedTile(pos, dir) == UseState.UNUSED)
        {
            list.Add(dir);
        }

        dir = GetLeft(forward);
        if (UsedTile(pos, dir) == UseState.UNUSED)
        {
            list.Add(dir);
        }

        dir = GetRight(forward);
        if (UsedTile(pos, dir) == UseState.UNUSED)
        {
            list.Add(dir);
        }

        return list;
    }


    /// <summary>
    /// 방향 목록에서 랜덤으로 방향 가져오기
    /// </summary>
    /// <param name="directions"></param>
    /// <returns></returns>
    List<Vector2> GetRandomDirections(List<Vector2> directions)
    {
        List<Vector2> list = new List<Vector2>();

        if (directions != null && directions.Count > 0)
        {
            int count = GetRandomSwitchCount();
            for (int i = 0; i < count; i++)
            {
                AddDirectionList(ref list, directions[Random.Range(0, directions.Count)]);
            }
        }

        return list;
    }

    void AddDirectionList(ref List<Vector2> list, Vector2 dir)
    {
        if (!list.Contains(dir))
        {
            list.Add(dir);
        }
    }

    public Vector2 GetRandomDirection(Tile prevTile)
    {
        Vector2 dir = Vector2.zero;
        // 시작 타일 처리
        Vector2 forward = Vector2.up;
        if (prevTile != null)
        {
            forward = prevTile.MyDirection;
        }

        int rand = Random.Range(0, 100);

        Vector2 right = GetRight(forward);
        Vector2 left = GetLeft(forward);
        int fowardPercent = GetPercent(forward);
        int leftPercent = fowardPercent + GetPercent(left);

        if (rand < fowardPercent)
        {
            dir = forward;
        }
        else if (rand < leftPercent)
        {
            dir = left;
        }
        else
        {
            dir = right;
        }

        if (prevTile != null)
        {
            Vector2 dirforward = dir;
            if (UsedTile(prevTile.CenterPosition, dir) == UseState.USED)
            {
                dir = GetRight(dirforward);
                if (UsedTile(prevTile.CenterPosition, dir) == UseState.USED)
                {
                    dir = GetLeft(dirforward);
                    if (UsedTile(prevTile.CenterPosition, dir) == UseState.USED)
                    {
                        return Vector2.zero;
                    }
                }
            }
        }

        return dir;
    }

    UseState UsedTile(Vector2 pos, Vector2 dir)
    {
        Vector2 movePos = MovePosition(dir, pos);
        return UsedTile((int)movePos.x, (int)movePos.y);
    }

    /// <summary>
    /// 해당 좌표에 타일이 있는지 여부 확인
    /// //todo : 전체 충돌 체크 필요
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public UseState UsedTile(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Tiles.GetLength(0) && y < Tiles.GetLength(1))
        {
            return Tiles[x, y] != null ? UseState.USED : UseState.UNUSED;
        }
        return UseState.OUTOFWOLRD;
    }

    Vector2 MovePosition(Vector2 direction, Vector2 pos)
    {
        pos += direction * TileGroup.MIN_SIZE;

        return pos;
    }
}
;
