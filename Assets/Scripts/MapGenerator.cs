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


    public Agent agent;
    private MapView mv;
    private MapData md;

    private int lastDirection;
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
            mv.AddRoomView(room.Pos, room.Width, room.Height);
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

    public void GenerateFullMaze()
    {
        md.GenerateFullMaze();
    }   

    public void GenerateDoors()
    {
        md.GenerateDoors();
    } 
    
    public void CarveDeadEnds()
    {
        md.CarveDeadEnds();
    }

    public void CreateAgent()
    {
        if(agent == null)
        {
            return;
        }
        var rooms = md.GetAllRoomData();
        var roomIndex = Random.Range(0, rooms.Count);
        var room = rooms[roomIndex];
        int randomX = Random.Range(room.Left, room.Right + 1);
        int randomY = Random.Range(room.Bottom, room.Top + 1);
        agent.InitializedAgent(new Vector2Int(randomX, randomY));
    }

    private void AgentMove(int direction)
    {
        if (agent == null) return;

        if(direction == MapDef.DIR_UP && md.GetCellType(agent.pos+MapDef.UP) != MapDef.CELL_TYPE_WALL)
        {
            agent.MoveTo(agent.pos + MapDef.UP);
            lastDirection = MapDef.DIR_UP;
        }else if(direction == MapDef.DIR_DOWN && md.GetCellType(agent.pos+MapDef.DOWN) != MapDef.CELL_TYPE_WALL)
        {
            agent.MoveTo(agent.pos + MapDef.DOWN);
            lastDirection = MapDef.DIR_DOWN;
        }
        else if(direction == MapDef.DIR_LEFT && md.GetCellType(agent.pos+MapDef.LEFT) != MapDef.CELL_TYPE_WALL)
        {
            agent.MoveTo(agent.pos + MapDef.LEFT);
            lastDirection = MapDef.DIR_LEFT;
        }
        else if(direction == MapDef.DIR_RIGHT && md.GetCellType(agent.pos+MapDef.RIGHT) != MapDef.CELL_TYPE_WALL)
        {
            agent.MoveTo(agent.pos + MapDef.RIGHT);
            lastDirection = MapDef.DIR_RIGHT;
        }
    }


    public void GenerateWholeMap()
    {
        // 1. 数据层，表现层重置
        md.Reset(WorkSpaceWidth,WorkSpaceHeight);
        mv.Reset(WorkSpaceWidth, WorkSpaceHeight);

        // 2.地图全部填充为墙体
        FullMapToWall();

        // 3. 生成地牢房间
        GenerateMapRooms();

        // 4.填充迷宫走廊
        GenerateFullMaze();

        // 5.生成所有门
        GenerateDoors();

        // 6.反雕刻
        CarveDeadEnds();

        // 7.更新表现层
        RefreshMapView();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AgentMove(MapDef.DIR_UP);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AgentMove(MapDef.DIR_DOWN);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AgentMove(MapDef.DIR_LEFT);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AgentMove(MapDef.DIR_RIGHT);
        }
    }
}
