using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MPXFile;


public enum Direction
{
    LEFT = 0,
    RIGHT = 1,
    BACK = 2,
    FORWARD = 3,
    BLOCKED = 4
}


public class CreatorTile : MonoBehaviour
{
    //public Vector3 TileSize;
    private bool IsStart;
    private bool IsEnd;

    public DungeonTile[] PrefabTiles;
    public PlayerController PlayerCon;

    public DungeonTile[,] Tiles;

    private DungeonTile lastTile;

    public int MaxIndex;
    public int LastIndex;
    public int ShowCount;

    public const int BASIC_TILE_INDEX = 0;
    public const int START_TILE_INDEX = 1;
    public const int END_TILE_INDEX = 2;

    /// <summary>
    /// 분기 개수
    /// </summary>
    public int SwitchCount;

    [SerializeField]
    private Vector3 playerPos;
    public Vector3 PlayerPos { get { return playerPos; } }

    List<DungeonTile> loadedTiles;
    Dictionary<int, Queue<DungeonTile>> pools;
    TileCreator creator;
    public TileCreator Creator { get { return creator; } }


    Vector3 centerPos;

    /// <summary>
    /// 동적으로 보여질 타일 범위
    /// </summary>
    public int ShowRange = 40;

    private void Awake()
    {
    }

    private void SpawnPlayer()
    {
        PlayerCon.CurrentPlayer.OnMove.AddListener(MovedPlayer);
    }

    /// <summary>
    /// 타일생성
    /// </summary>
    /// <param name="length">타일 길이</param>
    /// <param name="switchCount">분기 개수</param>
    public void CreateTiles(int length, int switchCount)
    {
        MaxIndex = length;
        SwitchCount = switchCount;

        CreateNewMap();
        PlayerCon = PlayerController.Inst;
        PlayerCon.SpawnPlayer.AddListener(SpawnPlayer);
    }

    private void CreateTiles(DungeonFile file)
    {
        MaxIndex = file.WorldSize;
        CreateLoadMap(file);

        PlayerCon = PlayerController.Inst;
        PlayerCon.SpawnPlayer.AddListener(SpawnPlayer);
    }

    public void CreateTiles()
    {
        DungeonFile file = TileImporter.Inst.CurrentFile;

        if (file != null)
        {
            CreateTiles(file);
        }
        else
        {
            CreateTiles(100, 10);
        }
    }


    //public DungeonTile FindTile(Tile tile)
    //{
    //    if (loadedTiles != null)
    //    {
    //        return loadedTiles.Find(o => o.Index == tile.Indexes);
    //    }
    //    return null;
    //}

    public DungeonTile FindNextTile(Tile tile)
    {
        if (tile != null)
        {
            int index = tile.GetLastIndex() + 1;
            if (loadedTiles != null)
            {
                return loadedTiles.Find(o => o.Index == index);
            }
        }
        return null;
    }

    public DungeonTile FindPrevTile(Tile tile)
    {
        if (tile != null)
        {
            int index = tile.GetLastIndex() - 1;
            if (loadedTiles != null)
            {
                return loadedTiles.Find(o => o.Index == index);
            }
        }
        return null;
    }


    public DungeonTile FindTile(int index)
    {
        if (loadedTiles != null)
        {
            return loadedTiles.Find(o => o.Index == index);
        }
        return null;
    }

    public DungeonTile FindFirst()
    {
        return FindTile(0);
    }

    public DungeonTile FindLast()
    {
        return FindTile(loadedTiles.Count - 1);
    }


    private void MovedPlayer(Vector3 pos)
    {
        if (pos != playerPos)
        {
            ShowTiles(pos + centerPos);
            RemoveLoadedTileAll();

            playerPos = pos;
        }
    }

    void Enqueue(DungeonTile tile, int keyIdx = -1)
    {
        if (tile != null)
        {
            int key = keyIdx == -1 ? BASIC_TILE_INDEX : keyIdx;
            Tile tileData = tile.Tile;
            if (tileData.ID == 0)
            {
                if (tileData.StartTile)
                {
                    key = START_TILE_INDEX;
                }
                else if (tileData.EndTile)
                {
                    key = END_TILE_INDEX;
                }
            }

            if (pools == null)
                pools = new Dictionary<int, Queue<DungeonTile>>();

            if (!pools.ContainsKey(key))
                pools.Add(key, new Queue<DungeonTile>());

            tile.gameObject.SetActive(false);
            pools[key].Enqueue(tile);
        }
    }

    DungeonTile Dequeue(Tile tile)
    {
        DungeonTile newTile = null;
        if (tile != null)
        {
            int key = BASIC_TILE_INDEX;
            if (tile.ID == 0)
            {
                if (tile.StartTile)
                {
                    key = START_TILE_INDEX;
                    //Debug.LogFormat("dequeue {0},{1}", tile.Name, key);
                }
                else if (tile.EndTile)
                {
                    key = END_TILE_INDEX;
                    //Debug.LogFormat("dequeue {0},{1}", tile.Name, key);
                }
            }

            if (pools != null && pools.ContainsKey(key) && pools[key].Count > 0)
            {
                newTile = pools[key].Dequeue();

                newTile.gameObject.SetActive(true);
                newTile.Init(tile, centerPos);
            }
            else
            {
                newTile = Create(PrefabTiles[key], tile);
            }
        }

        return newTile;
    }

    void AddLoadTile(DungeonTile tile)
    {
        if (tile != null)
        {
            if (loadedTiles == null)
                loadedTiles = new List<DungeonTile>();

            loadedTiles.Add(tile);
        }
    }

    void RemoveLoadedTileAll()
    {
        if (loadedTiles != null)
        {
            for (int i = loadedTiles.Count - 1; i >= 0; i--)
            {
                if (loadedTiles[i].Tile.CenterPosition.x < tilesMinX || loadedTiles[i].Tile.CenterPosition.y < tilesMinY
                    || loadedTiles[i].Tile.CenterPosition.x > tilesMaxX || loadedTiles[i].Tile.CenterPosition.y > tilesMaxY)
                {
                    RemoveLoadTile(loadedTiles[i], i);
                }
            }
        }
    }

    void RemoveLoadTile(DungeonTile tile, int index)
    {
        if (loadedTiles != null)
        {
            Enqueue(tile);
            loadedTiles.RemoveAt(index);
            Tiles[(int)tile.Tile.CenterPosition.x, (int)tile.Tile.CenterPosition.y] = null;
        }
    }

    void ClearLoadedTiles()
    {
        if (loadedTiles != null)
        {
            if (Application.isPlaying)
            {
                for (int i = 0; i < loadedTiles.Count; i++)
                {
                    if (loadedTiles[i] != null)
                        Enqueue(loadedTiles[i]);
                }
            }
            loadedTiles.Clear();
        }
    }


    public int SelectID = 0;

    private void OnDrawGizmosSelected()
    {
        if (creator != null)
        {
            Vector3 size = new Vector3(TileGroup.MIN_SIZE, TileGroup.MIN_SIZE, TileGroup.MIN_SIZE);
            Tile[,] tiles = creator.Tiles;

            if (tiles != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(new Vector3(playAreaMinX, 0, playAreaMinY), new Vector3(playAreaMinX, 0, playAreaMaxY));
                Gizmos.DrawLine(new Vector3(playAreaMinX, 0, playAreaMaxY), new Vector3(playAreaMaxX, 0, playAreaMaxY));
                Gizmos.DrawLine(new Vector3(playAreaMaxX, 0, playAreaMaxY), new Vector3(playAreaMaxX, 0, playAreaMinY));
                Gizmos.DrawLine(new Vector3(playAreaMaxX, 0, playAreaMinY), new Vector3(playAreaMinX, 0, playAreaMinY));

                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    for (int k = 0; k < tiles.GetLength(1); k++)
                    {
                        if (tiles[i, k] != null)
                        {
                            if (tiles[i, k].ID == SelectID || SelectID == -1)
                            {
                                if (tiles[i, k].Overlap)
                                {
                                    Gizmos.color = Color.yellow;
                                }
                                else
                                {
                                    Gizmos.color = Color.white;
                                }

                                if (tiles[i, k].StartTile)
                                {
                                    if (tiles[i, k].ID == 0 && tiles[i, k].GetFirstIndex() == 0)
                                        Gizmos.color = Color.red;
                                    else
                                        Gizmos.color = Color.cyan;

                                    //Gizmos.DrawSphere(new Vector3(tiles[i, k].CenterPosition.x, 0, tiles[i, k].CenterPosition.y), 1f);
                                }
                                else if (tiles[i, k].EndTile)
                                {
                                    if (tiles[i, k].ID == 0)
                                    {
                                        Gizmos.color = Color.black;
                                    }
                                    else
                                    {
                                        Gizmos.color = Color.gray;
                                    }
                                    //Gizmos.DrawSphere(new Vector3(tiles[i, k].CenterPosition.x, 0, tiles[i, k].CenterPosition.y), 1f);
                                }

                                Gizmos.DrawCube(new Vector3(tiles[i, k].CenterPosition.x - centerPos.x, 0, tiles[i, k].CenterPosition.y - centerPos.z), size);
                            }
                        }
                    }

                }
            }
        }
    }




    public void ClearMap()
    {
        DungeonTile[] tiles = GetComponentsInChildren<DungeonTile>();
        for (int i = tiles.Length - 1; i >= 0; i--)
        {
            DestroyImmediate(tiles[i].gameObject);
        }
    }

    public void CreateLoadMap(DungeonFile file)
    {
        InitCreate();

        creator = new TileCreator(file);
        Tiles = new DungeonTile[creator.Tiles.GetLength(0), creator.Tiles.GetLength(1)];

        centerPos = new Vector3(creator.CenterPos.x, 0, creator.CenterPos.y);
        InitPlayerPos();
    }

    void InitCreate()
    {
        IsStart = false;
        IsEnd = false;
        LastIndex = 0;

        Vector2[] size = new Vector2[PrefabTiles.Length];
        for (int i = 0; i < size.Length; i++)
        {
            size[i] = new Vector2(PrefabTiles[i].Size.x, PrefabTiles[i].Size.y);
        }
    }

    public void CreateNewMap()
    {
        InitCreate();

        creator = new TileCreator(MaxIndex);
        creator.CreateNewMap(SwitchCount);

        Tiles = new DungeonTile[creator.Tiles.GetLength(0), creator.Tiles.GetLength(1)];

        //LastIndex = creator.Index;
        centerPos = new Vector3(creator.CenterPos.x, 0, creator.CenterPos.y);
        InitPlayerPos();
    }

    public void InitPlayerPos()
    {
        playerPos = Vector3.zero;
    }

    public void ShowTileAll()
    {
        ShowCount = 0;
        Tile[,] tiles = creator.Tiles;
        ClearLoadedTiles();
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int k = 0; k < tiles.GetLength(1); k++)
            {
                Tile tile = tiles[i, k];
                if (tile != null)
                {
                    //Debug.LogFormat("{0}, {1}", tile.MyTile.CenterPosition, tile.MyTile.TilePos);
                    lastTile = Dequeue(tile);
                    AddLoadTile(lastTile);
                    ShowCount++;
                    //Tiles[i, k] = dungeonTile;
                }
            }
        }

    }


    private int playAreaMinX;
    private int playAreaMinY;
    private int playAreaMaxX;
    private int playAreaMaxY;

    private int tilesMinX;
    private int tilesMinY;
    private int tilesMaxX;
    private int tilesMaxY;

    public void ShowTiles(Vector3 pos)
    {
        Tile[,] tiles = creator.Tiles;

        int half = ShowRange / 2;

        tilesMinX = (int)pos.x - half;
        tilesMinY = (int)pos.z - half;

        tilesMaxX = (int)pos.x + half;
        tilesMaxY = (int)pos.z + half;

        playAreaMinX = tilesMinX - (int)centerPos.x;
        playAreaMinY = tilesMinY - (int)centerPos.z;

        playAreaMaxX = tilesMaxX - (int)centerPos.x;
        playAreaMaxY = tilesMaxY - (int)centerPos.z;

        for (int i = 0; i < ShowRange; i++)
        {
            for (int k = 0; k < ShowRange; k++)
            {
                int x = tilesMinX + i;
                int y = tilesMinY + k;

                if (x >= 0 && y >= 0 && x < tiles.GetLength(0) && y < tiles.GetLength(1))
                {
                    Tile tile = tiles[x, y];
                    DungeonTile dTile = Tiles[x, y];

                    if (tile != null)
                    {
                        if (dTile == null)
                        {
                            dTile = Dequeue(tile);
                            Tiles[x, y] = dTile;
                            if (tile.StartTile != tile.EndTile)
                                AddLoadTile(dTile);
                        }
                    }
                }
            }
        }
    }

    int GetRandomPefab()
    {
        return UnityEngine.Random.Range(0, PrefabTiles.Length);
    }


    public DungeonTile Create(DungeonTile prefab, Tile tile)
    {
        DungeonTile newTile = Instantiate(prefab);
        newTile.transform.SetParent(transform);

        newTile.Init(tile, centerPos);

        return newTile;
    }

}
