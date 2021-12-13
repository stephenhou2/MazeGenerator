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

    private bool IsValidStart(Vector2Int pos)
    {
        if (_mapCellType[pos.x, pos.y] != MapDef.CELL_TYPE_WALL) return false;
        for (int m = 0; m < MapDef.FULL_NEIGHBORS.Length; m++)
        {
            int type = GetCellType(pos.x + MapDef.FULL_NEIGHBORS[m].x, pos.y + MapDef.FULL_NEIGHBORS[m].y);
            if (type != MapDef.CELL_TYPE_WALL && type != MapDef.CELL_TYPE_SOLID_WALL)
            {
                return false;
            }
        }

        return true;
    }

    public bool FindMazeStart()
    {
        for (int i = 0; i < _workSpaceWidth; i++)
        {
            for (int j = 0; j < _workSpaceHeight; j++)
            {
                if(IsValidStart(new Vector2Int(i,j)))
                {
                    _toHandle.Enqueue(new Vector2Int(i, j));
                    return true;
                }
            }
        }

        return false;
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
    }

    public void FullMapToWall()
    {
        // 先全部初始化为墙
        for (int i = 0; i < _workSpaceWidth; i++)
        {
            for (int j = 0; j < _workSpaceHeight; j++)
            {
                _mapCellType[i, j] = MapDef.CELL_TYPE_WALL;
            }
        }
    }

    public void MapBorderToWall()
    {
        // 最外围初始化为实体墙
        for (int i = 0; i < _workSpaceWidth; i++)
        {
            _mapCellType[i, 0] = MapDef.CELL_TYPE_SOLID_WALL;
            _mapCellType[i, _workSpaceHeight - 1] = MapDef.CELL_TYPE_SOLID_WALL;
        }

        for (int i = 0; i < _workSpaceHeight; i++)
        {
            _mapCellType[0, i] = MapDef.CELL_TYPE_SOLID_WALL;
            _mapCellType[_workSpaceWidth - 1, i] = MapDef.CELL_TYPE_SOLID_WALL;
        }
    }

    public void RoomBorderToWall()
    {
        foreach(Room room in _allRooms)
        {
            // 房间外围初始化为实体墙
            for (int i = room.Left - 1; i <= room.Right + 1; i++)
            {
                _mapCellType[i, room.Top + 1] = MapDef.CELL_TYPE_SOLID_WALL;
                _mapCellType[i, room.Bottom - 1] = MapDef.CELL_TYPE_SOLID_WALL;
            }
            for (int i = room.Bottom - 1; i <= room.Top + 1; i++)
            {
                _mapCellType[room.Left - 1, i] = MapDef.CELL_TYPE_SOLID_WALL;
                _mapCellType[room.Right + 1, i] = MapDef.CELL_TYPE_SOLID_WALL;
            }
        }
    }

    private bool Rool(float chance)
    {
        float v = Random.Range(0, 1);
        return v < chance;
    }

    private bool CheckCanChangeToDoor(int x,int y)
    {
        int curCellType = GetCellType(x, y);
        int upCellType = GetCellType(x, y+1);
        int downCellType = GetCellType(x, y-1);
        int leftCellType = GetCellType(x-1, y);
        int rightCellType = GetCellType(x+1, y);

        if (curCellType != MapDef.CELL_TYPE_WALL && curCellType != MapDef.CELL_TYPE_SOLID_WALL)
            return false;

        if (upCellType != MapDef.CELL_TYPE_WALL && upCellType != MapDef.CELL_TYPE_SOLID_WALL
             && downCellType != MapDef.CELL_TYPE_WALL && downCellType != MapDef.CELL_TYPE_SOLID_WALL)
            return Rool(MapDef.DOOR_CHANCE); ;
        
        if (leftCellType != MapDef.CELL_TYPE_WALL && leftCellType != MapDef.CELL_TYPE_SOLID_WALL
             && rightCellType != MapDef.CELL_TYPE_WALL && rightCellType != MapDef.CELL_TYPE_SOLID_WALL)
            return Rool(MapDef.DOOR_CHANCE); ;

        return false;
    }

    private void GenerateRoomDoor(Room room)
    {
        // 房间外围初始化为实体墙
        for (int i = room.Left - 1; i <= room.Right + 1; i++)
        {
            if (CheckCanChangeToDoor(i, room.Top + 1))
            {
                _mapCellType[i, room.Top + 1] = MapDef.CELL_TYPE_DOOR;
                return;
            }
            if (CheckCanChangeToDoor(i, room.Bottom - 1))
            {
                _mapCellType[i, room.Bottom - 1] = MapDef.CELL_TYPE_DOOR;
                return;
            }
        }
        for (int i = room.Bottom - 1; i <= room.Top + 1; i++)
        {
            if (CheckCanChangeToDoor(room.Left - 1, i))
            {
                _mapCellType[room.Left - 1, i] = MapDef.CELL_TYPE_DOOR;
                return;
            }
            if (CheckCanChangeToDoor(room.Right + 1, i))
            {
                _mapCellType[room.Right + 1, i] = MapDef.CELL_TYPE_DOOR;
                return;
            }
        }
    }

    public void GenerateDoors()
    {
        // 生成门
        foreach(Room room in _allRooms)
        {
            GenerateRoomDoor(room);
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
