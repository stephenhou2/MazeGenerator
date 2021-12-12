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

        List<Room> allRooms = md.GetAllRoomData();

        foreach(Room room in allRooms)
        {
            mv.AddRoomView(room.Pos, room.Width_Half, room.Height_Half);
        }

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

    public void FloodFillMazeSingleStep()
    {
        md.FloodFillMazeSingleStep();
    }   
    
    public void GenerateMaze()
    {
        
    }

    public void CarveConnection()
    {

    }

    public void GenerateWholeMap()
    {
        // 1. 数据层，表现层重置
        md.Reset(WorkSpaceWidth,WorkSpaceHeight);
        mv.Reset(WorkSpaceWidth, WorkSpaceHeight);

        // 3. 生成地牢房间
        GenerateMapRooms();

        // 4.填充迷宫走廊
        GenerateMaze();

        // 5.连接处挖洞
        CarveConnection();

        // 6.更新表现层
        RefreshMapView();
    }
}
