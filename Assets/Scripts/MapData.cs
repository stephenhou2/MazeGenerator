using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    private int[,] _mapCellType;
    private int _workSpaceWidth;
    private int _workSpaceHeight;

    private List<Room> _allRooms = new List<Room>();

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

    private Vector2Int[] _neighbors = new Vector2Int[]
    {
        new Vector2Int(-1,0),
        new Vector2Int(0,-1),
        new Vector2Int(1,0),
        new Vector2Int(0,1),
    };

    private Queue<Vector2Int> _toHandle = new Queue<Vector2Int>();

    /// <summary>
    /// 洪水填充算法生成迷宫走廊
    /// </summary>
    /// <param name="start">算法起点</param>
    private void FillMaze(Vector2Int start)
    {
        _toHandle.Clear();
        _toHandle.Enqueue(start);

        while(_toHandle.Count > 0)
        {
            Vector2Int pos = _toHandle.Dequeue();


        }
    }

    public void FloodFillMaze()
    {
        for (int i=1;i<_workSpaceWidth-1;i++)
        { 
            for(int  j=1;j<_workSpaceHeight-1;j++)
            {
                if(_mapCellType[i,j] == MapDef.CELL_TYPE_WALL)
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
