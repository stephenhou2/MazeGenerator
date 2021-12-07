using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator:MonoBehaviour
{
    #region  Def
    public int WorkSpaceWidth = 100;
    public int WorkSpaceHeight = 80;
    public int RoomMaxTry = 100;

    public int RoomMinSize_Half = 5;
    public int RoomMaxSize_Half = 7;
    #endregion

    public MapView mv;
    public MapData md = new MapData();

    /// <summary>
    /// 创建地牢房间
    /// </summary>
    /// <param name="minSize"></param>
    /// <param name="maxSize"></param>
    /// <param name="maxTry"></param>
    public void GenerateMapRooms(int minSize,int maxSize,int maxTry)
    {
        int cnt = 0;
        while(cnt < maxTry)
        {
            // 房间的宽高
            int width_half = Random.Range(minSize, maxSize);
            int height_half = Random.Range(minSize, maxSize);

            int randomPosX = Random.Range(1 + width_half + 1, WorkSpaceWidth - 1 - width_half - 1);
            int randomPosY = Random.Range(1 + height_half + 1, WorkSpaceHeight - 1 - height_half - 1);

            Vector2Int pos = new Vector2Int(randomPosX, randomPosY);
            if (md.TryAddRoom(pos, width_half, height_half))
            {
                mv.AddRoomView(pos, width_half, height_half);
            }

            cnt++;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test")]
    private void Test()
    {
        // 1. 数据层，表现层重置
        md.Reset();
        mv.Reset();

        // 2. 数据层重新初始化
        md.InitializeMapData(WorkSpaceWidth, WorkSpaceHeight);
        mv.InitializeMapView(WorkSpaceWidth, WorkSpaceHeight);

        // 3. 生成地牢房间
        GenerateMapRooms(RoomMinSize_Half,RoomMaxSize_Half, RoomMaxTry);

        // 4.填充迷宫走廊



        // 5.连接处挖洞
    }
#endif
}
