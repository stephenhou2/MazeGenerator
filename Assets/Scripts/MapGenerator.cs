using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator:MonoBehaviour
{
    [HideInInspector]
    public int WorkSpaceWidth = 100;

    [HideInInspector]
    public int WorkSpaceHeight = 80;

    [HideInInspector]
    public int RoomMaxTry = 100;

    [HideInInspector]
    public int RoomMinSize_Half = 5;

    [HideInInspector]
    public int RoomMaxSize_Half = 7;

    private MapView mv;
    private MapData md;

    public void InitializeMapGenerator()
    {
        var viewNode = transform.Find("_MAP_VIEW");
        if(viewNode != null)
        {
            mv = viewNode.GetComponent<MapView>();
        }

        md = new MapData();
    }

    public void RefreshMapView()
    {
        mv.Reset(WorkSpaceWidth, WorkSpaceHeight);

        // 房间
        List<Room> allRooms = md.GetAllRoomData();
        foreach(Room room in allRooms)
        {
            mv.AddRoomView(room.Pos, room.Width_Half, room.Height_Half);
        }

        // 墙
        for(int i=0;i<WorkSpaceWidth;i++)
        {
            for(int j=0;j<WorkSpaceHeight;j++)
            {
                if(md.GetCellType(i,j) == MapDef.CELL_TYPE_WALL)
                {
                    mv.AddWallView(new Vector2Int(i,j));
                }
                else if(md.GetCellType(i,j) == MapDef.CELL_TYPE_SOLID_WALL)
                {
                    mv.AddSolidWallView(new Vector2Int(i, j));
                }
            }
        }

        // 门
        for (int i = 0; i < WorkSpaceWidth; i++)
        {
            for (int j = 0; j < WorkSpaceHeight; j++)
            {
                if (md.GetCellType(i, j) == MapDef.CELL_TYPE_DOOR)
                {
                    mv.AddDoorView(new Vector2Int(i, j));
                }
            }
        }
    }

    public void ResetMap(int mapWidth,int mapHeight,int roomMinSizeHalf,int roomMaxSizeHalf,int roomMaxTry)
    {
        WorkSpaceWidth = mapWidth;
        WorkSpaceHeight = mapHeight;
        RoomMinSize_Half = roomMinSizeHalf;
        RoomMaxSize_Half = roomMaxSizeHalf;
        RoomMaxTry = roomMaxTry;

        // 1. 数据层，表现层重置
        md.Reset(mapWidth,mapHeight);
        mv.Reset(mapWidth, mapHeight);
    }

    public void GenerateMapRooms()
    {
        md.GenerateMapRooms(RoomMinSize_Half, RoomMaxSize_Half, RoomMaxTry);
    }

    public void FullMapToWall()
    {
        md.FullMapToWall();
    }

    public void MapBorderToWall()
    {
        md.MapBorderToWall();
    }

    public void RoomBorderToWall()
    {
        md.RoomBorderToWall();
    }

    public void FloodFillMazeSingleStep()
    {
        //if(md.FindMazeStart())
        //{
        //    md.FloodFillMazeSingleStep();
        //}

        md.GenerateFullMaze();
    }   
    
    public void GenerateFullMaze()
    {
        while(md.FindMazeStart())
        {
            md.FloodFillMazeSingleStep();
        }
    }

    public void GenerateDoors()
    {
        md.GenerateDoors();
    }

    public void GenerateWholeMap()
    {
        // 1. 数据层，表现层重置
        md.Reset(WorkSpaceWidth,WorkSpaceHeight);
        mv.Reset(WorkSpaceWidth, WorkSpaceHeight);

        // 2.地图全部填充为墙体
        FullMapToWall();

        // 3. 地图边界填充为实体墙
        MapBorderToWall();

        // 4. 生成地牢房间
        GenerateMapRooms();

        // 5.房间边界填充为实体墙
        RoomBorderToWall();

        // 4.填充迷宫走廊
        GenerateFullMaze();

        // 5.生成所有门
        GenerateDoors();

        // 6.更新表现层
        RefreshMapView();
    }
}
