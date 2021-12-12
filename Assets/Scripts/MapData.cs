using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    private int[,] _mapCellType;
    private int _workSpaceWidth;
    private int _workSpaceHeight;

    private List<Room> _allRooms = new List<Room>();

    public int[,] GetMapCellData()
    {
        return _mapCellType;
    }

    public List<Room> GetAllRoomData()
    {
        return _allRooms;
    }

    private bool CheckRoomCanCreate(Vector2Int pos, int halfWidth, int halfHeight)
    {
        if (pos.x - halfWidth <= 0
            || pos.x + halfWidth >= _workSpaceWidth
            || pos.y - halfHeight <= 0
            || pos.y + halfHeight >= _workSpaceHeight)
        {
            return false;
        }

        foreach (Room room in _allRooms)
        {
            if (Mathf.Abs(room.Pos.x - pos.x) < room.Width_Half + halfWidth + 2
                && Mathf.Abs(room.Pos.y - pos.y)  < room.Height_Half  + halfHeight + 2)
            {
                return false;
            }
        }
        return true;
    }

    public void AddNewRoom(Vector2Int pos, int halfWidth, int halfHeight)
    {
        Room room = new Room(pos, halfWidth, halfHeight);
        _allRooms.Add(room);

        // 房间所在的所有格子，设置为房间
        for(int i = room.Left;i<=room.Right;i++)
        {
            for (int j = room.Bottom; j <= room.Top; j++)
            {
                _mapCellType[i, j] = MapDef.CELL_TYPE_ROOM;
            }
        }

        // 房间外围初始化为实体墙
        for (int i = room.Left-1; i <= room.Right+1; i++)
        {
            _mapCellType[i, room.Top+1] = MapDef.CELL_TYPE_SOLID_WALL;
            _mapCellType[i, room.Bottom - 1] = MapDef.CELL_TYPE_SOLID_WALL;
        }
        for (int i = room.Bottom-1; i <= room.Top+1; i++)
        {
            _mapCellType[room.Left-1,i] = MapDef.CELL_TYPE_SOLID_WALL;
            _mapCellType[room.Right+1,i] = MapDef.CELL_TYPE_SOLID_WALL;
        }
    }

    public bool TryAddRoom(Vector2Int pos, int halfWidth, int halfHeight)
    {
        if(CheckRoomCanCreate(pos, halfWidth, halfHeight))
        {
            AddNewRoom(pos, halfWidth, halfHeight);
            return true;
        }

        return false;
    }

    private Queue<Vector2Int> _toHandle = new Queue<Vector2Int>();

    public int GetCellType (int x,int y)
    {
        // 超出工作区域
        if (x < 0 || x >= _workSpaceWidth) return MapDef.CELL_TYPE_INVALID;
        if (y < 0 || y >= _workSpaceHeight) return MapDef.CELL_TYPE_INVALID;

        if (_mapCellType == null) return MapDef.CELL_TYPE_INVALID;

        return _mapCellType[x, y];
    }

    private Vector2Int[] RandomNeighbors()
    {
        int seed = Random.Range(0, 4);
        return MapDef.NEIGHBORS_POOL[seed];
    }

    private void FindMazeStart()
    {
        for (int i = 0; i < _workSpaceWidth; i++)
        {
            for (int j = 0; j < _workSpaceHeight; j++)
            {
                if (_mapCellType[i, j] == MapDef.CELL_TYPE_WALL)
                {
                    for(int m = 0;m<MapDef.FULL_NEIGHBORS.Length;m++)
                    {
                        int type = GetCellType(i + MapDef.FULL_NEIGHBORS[m].x, j + MapDef.FULL_NEIGHBORS[m].y);
                        if (type != MapDef.CELL_TYPE_WALL && type != MapDef.CELL_TYPE_SOLID_WALL)
                            return;
                    }
                    _toHandle.Enqueue(new Vector2Int(i, j));
                    return;
                }
            }
        }
    }

    private void GenerateMaze()
    {
        if (_toHandle.Count == 0) return;

        Vector2Int cur = _toHandle.Dequeue();
        _mapCellType[cur.x, cur.y] = MapDef.CELL_TYPE_FLOOR;
        Vector2Int[] neighbors = RandomNeighbors();
        for (int i = 0; i < neighbors.Length; i++)
        {
            Vector2Int pos = cur + neighbors[i];
            Vector2Int pos2 = cur + 2 * neighbors[i];
            if (GetCellType(pos.x, pos.y) == MapDef.CELL_TYPE_WALL 
                && GetCellType(pos2.x, pos2.y) == MapDef.CELL_TYPE_WALL)
            {
                _mapCellType[pos.x, pos.y] = MapDef.CELL_TYPE_FLOOR;
                _toHandle.Enqueue(pos2);
                GenerateMaze();
            }
        }
    }

    public void FloodFillMazeSingleStep()
    {
        if(_toHandle.Count == 0)
        {
            FindMazeStart();
        }

        if (_toHandle.Count == 0)
        {
            Debug.Log("已经无法继续生成迷宫");
            return;
        }

        GenerateMaze();
    }

    /// <summary>
    /// 创建地牢房间
    /// </summary>
    /// <param name="minSize"></param>
    /// <param name="maxSize"></param>
    /// <param name="maxTry"></param>
    public void GenerateMapRooms(int minSize, int maxSize, int maxTry)
    {
        int cnt = 0;
        while (cnt < maxTry)
        {
            // 房间的宽高
            int width_half = Random.Range(minSize, maxSize);
            int height_half = Random.Range(minSize, maxSize);

            int randomPosX = Random.Range(1 + width_half + 1, _workSpaceWidth - 1 - width_half - 1);
            int randomPosY = Random.Range(1 + height_half + 1, _workSpaceHeight - 1 - height_half - 1);

            Vector2Int pos = new Vector2Int(randomPosX, randomPosY);
            TryAddRoom(pos, width_half, height_half);
            cnt++;
        }
    }

    private void InitializeMapData(int mapWidth,int mapHeight)
    {
        _workSpaceWidth = mapWidth;
        _workSpaceHeight = mapHeight;
        _mapCellType = new int[mapWidth, mapHeight];

        // 先全部初始化为墙
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                _mapCellType[i, j] = MapDef.CELL_TYPE_WALL;
            }
        }

        // 最外围初始化为实体墙
        for(int i = 0;i<mapWidth;i++)
        {
            _mapCellType[i, 0] = MapDef.CELL_TYPE_SOLID_WALL;
            _mapCellType[i, _workSpaceHeight-1] = MapDef.CELL_TYPE_SOLID_WALL;
        }

        for(int i = 0;i<mapHeight;i++)
        {
            _mapCellType[0, i] = MapDef.CELL_TYPE_SOLID_WALL;
            _mapCellType[_workSpaceWidth-1, i] = MapDef.CELL_TYPE_SOLID_WALL;
        }
    }

    public void Reset(int mapWidth,int mapHeight)
    {
        _workSpaceWidth = mapWidth;
        _workSpaceHeight = mapHeight;
        _allRooms.Clear();

        InitializeMapData(mapWidth, mapHeight);
    }
}
