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

        return _mapCellType[x, y] == MapDef.CELL_TYPE_WALL;
    }

    private Vector2Int[] RandomNeighbors()
    {
        int seed = Random.Range(0, 4);
        return MapDef.NEIGHBORS_POOL[seed];
    }

    /// <summary>
    /// 洪水填充算法生成迷宫走廊
    /// </summary>
    /// <param name="start">算法起点</param>
    private void FillMaze(Vector2Int start)
    {
        _toHandle.Clear();
        _toHandle.Enqueue(start);
        _mapCellType[start.x, start.y] = MapDef.CELL_TYPE_EMPTY;

        while (_toHandle.Count > 0)
        {
            Vector2Int pos = _toHandle.Dequeue();
            Vector2Int[] neighbors = RandomNeighbors();
            for(int i=0;i< neighbors.Length;i++)
            {
                // 走廊旁边要留一个格子当作墙，所以要向周围扩展两步
                if (CheckIsWall(pos.x + neighbors[i].x,pos.y+ neighbors[i].y) 
                    && CheckIsWall(pos.x + 2* neighbors[i].x,pos.y + 2* neighbors[i].y))
                    {
                        Vector2Int newPos1 = pos + neighbors[i];
                        Vector2Int newPos2 = pos + 2*neighbors[i];
                        _mapCellType[newPos1.x, newPos1.y] = MapDef.CELL_TYPE_EMPTY;
                        _mapCellType[newPos2.x, newPos2.y] = MapDef.CELL_TYPE_EMPTY;
                        _toHandle.Enqueue(newPos2);
                        break;
                    }
            }
        }
    }

    public void FloodFillMaze()
    {
        for (int i = 1; i < _workSpaceWidth - 1; i++)
        {
            for (int j = 1; j < _workSpaceHeight - 1; j++)
            {
                if (_mapCellType[i, j] == MapDef.CELL_TYPE_WALL
                    && _mapCellType[i - 1, j] == MapDef.CELL_TYPE_WALL
                    && _mapCellType[i + 1, j] == MapDef.CELL_TYPE_WALL
                    && _mapCellType[i, j - 1] == MapDef.CELL_TYPE_WALL
                    && _mapCellType[i, j + 1] == MapDef.CELL_TYPE_WALL)
                {
                    FillMaze(new Vector2Int(i, j));
                }
            }
        }
    }

    public void InitializeMapData(int mapWidth,int mapHeight)
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

    public void Reset()
    {
        _mapCellType = null;
        _allRooms.Clear();
    }
}
