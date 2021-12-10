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

        for (int i = room.Left-1; i <= room.Right+1; i++)
        {
            _mapCellType[i,room.Bottom-1] = MapDef.CELL_TYPE_WALL;
        }
        for (int i = room.Left - 1; i <= room.Right + 1; i++)
        {
            _mapCellType[i,room.Top+1] = MapDef.CELL_TYPE_WALL;
        }
        for (int i = room.Bottom - 1; i <= room.Top + 1; i++)
        {
            _mapCellType[room.Left-1, i] = MapDef.CELL_TYPE_WALL;
        }
        for (int i = room.Bottom - 1; i <= room.Top + 1; i++)
        {
            _mapCellType[room.Right+1, i] = MapDef.CELL_TYPE_WALL;
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

    public bool CheckIsWall (int x,int y)
    {
        // 超出工作区域
        if (x < 0 || x >= _workSpaceWidth) return false;
        if (y < 0 || y >= _workSpaceHeight) return false;

        if (_mapCellType == null) return false;

        return _mapCellType[x, y] == MapDef.CELL_TYPE_WALL;
    }

    private int[] RandomNeighbors()
    {
        int seed = Random.Range(0, 4);
        return MapDef.NEIGHBORS_POOL[seed];
    }

    private void TryChangeToWall(Vector2Int pos)
    {
        if(_mapCellType[pos.x,pos.y] == MapDef.CELL_TYPE_FLOOR)
        {
            _mapCellType[pos.x, pos.y] = MapDef.CELL_TYPE_WALL;
        }
    }

    private bool TryAddCorridor(Vector2Int from,int direction)
    {
        Vector2Int offset = MapDef.OFFSET_MAP[direction];
        Vector2Int newPos1 = from + offset;
        Vector2Int newPos2 = from + 2 * offset;
        if(_mapCellType[newPos1.x, newPos1.y] == MapDef.CELL_TYPE_FLOOR
             && _mapCellType[newPos2.x, newPos2.y] == MapDef.CELL_TYPE_FLOOR)
        {
            _mapCellType[newPos1.x, newPos1.y] = MapDef.CELL_TYPE_CORRIDOR;
            _mapCellType[newPos2.x, newPos2.y] = MapDef.CELL_TYPE_CORRIDOR;

            switch(direction)
            {
                case MapDef.DIRECTION_UP:
                case MapDef.DIRECTION_DOWN:
                    TryChangeToWall(from + MapDef.LEFT);
                    TryChangeToWall(from + MapDef.RIGHT);
                    TryChangeToWall(newPos1 + MapDef.LEFT);
                    TryChangeToWall(newPos1 + MapDef.RIGHT);
                    break;
                case MapDef.DIRECTION_LEFT:
                case MapDef.DIRECTION_RIGHT:
                    TryChangeToWall(from + MapDef.UP);
                    TryChangeToWall(from + MapDef.DOWN);
                    TryChangeToWall(newPos1 + MapDef.UP);
                    TryChangeToWall(newPos1 + MapDef.DOWN);
                    break;
                default:
                    break;
            }
            return true;
        }
        return false;
    }


    /// <summary>
    /// 洪水填充算法生成迷宫走廊
    /// </summary>
    /// <param name="start">算法起点</param>
    private void FillMaze(Vector2Int start)
    {
        _toHandle.Clear();
        _toHandle.Enqueue(start);
        _mapCellType[start.x, start.y] = MapDef.CELL_TYPE_CORRIDOR;

        while (_toHandle.Count > 0)
        {
            Vector2Int pos = _toHandle.Dequeue();
            int[] neighbors = RandomNeighbors();
            for(int i=0;i< neighbors.Length;i++)
            {
                int direction = neighbors[i];
                if (TryAddCorridor(pos, direction))
                {
                    _toHandle.Enqueue(pos+ 2*MapDef.OFFSET_MAP[direction]);
                    break;
                }
            }
        }
    }

    public void FloodFillMaze()
    {
        if(_toHandle.Count > 0)
        {
            Vector2Int pos = _toHandle.Dequeue();
            int[] neighbors = RandomNeighbors();
            for (int i = 0; i < neighbors.Length; i++)
            {
                int direction = neighbors[i];
                if (TryAddCorridor(pos, direction))
                {
                    _toHandle.Enqueue(pos + 2 * MapDef.OFFSET_MAP[direction]);
                    break;
                }
            }
        }
    }

    private void FindMazeStart()
    {
        for (int i = 0; i < _workSpaceWidth; i++)
        {
            for (int j = 0; j < _workSpaceHeight; j++)
            {
                if (_mapCellType[i, j] == MapDef.CELL_TYPE_WALL)
                {
                    _toHandle.Enqueue(new Vector2Int(i, j));
                }
            }
        }
    }

    public void FloodFillMazeSingleStep()
    {
        if(_toHandle.Count == 0)
        {
            FindMazeStart();
        }

        if (_toHandle.Count == 0) return;

        for(int i =0;i<MapDef.NEIGHBORS_MAP.Length;i++)
        {

        }
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
    }

    public void Reset(int mapWidth,int mapHeight)
    {
        _workSpaceWidth = mapWidth;
        _workSpaceHeight = mapHeight;
        _allRooms.Clear();

        InitializeMapData(mapWidth, mapHeight);
    }
}
