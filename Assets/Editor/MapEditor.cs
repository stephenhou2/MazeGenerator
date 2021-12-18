using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditor : EditorWindow
{
    [MenuItem("Tool/地图编辑器", false,10)]
    public static void OpenMapEditor()
    {
        MapEditor window =  GetWindow<MapEditor>("地图编辑器", false);

        if(window != null)
        {
            window.autoRepaintOnSceneChange = true;
            window.Show(true);
            FocusWindowIfItsOpen<MapEditor>();
            window.minSize =  new Vector2(Screen.width / 20, Screen.height / 3);
            window.Initialize();
        }
    }

    private GUILayoutOption[] mTextStyle = new GUILayoutOption[]
    {
        GUILayout.Width(200),
        GUILayout.Height(20),
    };

    private int mWorkSpaceWidth = 100;
    private int mWorkSpaceHeight = 80;
    private int mRoomMinSizeHalf = 5;
    private int mRoomMaxSizeHalf = 7;
    private int mRoomMaxTry = 100;

    private MapGenerator mapGen;

    private void Initialize()
    {
        var node = GameObject.Find("_MAP_GENERATOR");
        if(node != null)
        {
            mapGen = node.GetComponent<MapGenerator>();
        }

        if(mapGen != null)
        {
            mapGen.InitializeMapGenerator();
            mapGen.ResetMap(mWorkSpaceWidth, mWorkSpaceHeight, mRoomMinSizeHalf, mRoomMaxSizeHalf, mRoomMaxTry);
        }

    }

    private void OnGUI()
    {
        if(mapGen == null)
        {
            EditorGUILayout.LabelField("场景中没有找到地图生成器！！！！！！");
            return;
        }

        mWorkSpaceWidth = EditorGUILayout.IntField("地图宽度:", mWorkSpaceWidth, mTextStyle);
        mWorkSpaceHeight = EditorGUILayout.IntField("地图高度:", mWorkSpaceHeight, mTextStyle);
        mRoomMinSizeHalf = EditorGUILayout.IntField("最小房间size:", mRoomMinSizeHalf, mTextStyle);
        mRoomMaxSizeHalf = EditorGUILayout.IntField("最大房间size:", mRoomMaxSizeHalf, mTextStyle);
        mRoomMaxTry = EditorGUILayout.IntField("生成房间最大尝试次数:", mRoomMaxTry, mTextStyle);

        EditorGUILayout.Space();

        if(GUILayout.Button("重置地图"))
        {
            mapGen.ResetMap(mWorkSpaceWidth,mWorkSpaceHeight,mRoomMinSizeHalf,mRoomMaxSizeHalf,mRoomMaxTry);
            mapGen.RefreshMapView();
        }

        if(GUILayout.Button("全部填充为墙壁"))
        {
            mapGen.FullMapToWall();
            mapGen.RefreshMapView();
        }

        if(GUILayout.Button("地图边界填充为实体墙壁"))
        {
            mapGen.MapBorderToWall();
            mapGen.RefreshMapView();
        }

        if (GUILayout.Button("生成房间"))
        {
            mapGen.GenerateMapRooms();
            mapGen.RefreshMapView();
        }

        if(GUILayout.Button("房间周围填充为实体墙壁"))
        {
            mapGen.RoomBorderToWall();
            mapGen.RefreshMapView();
        }

        if (GUILayout.Button("填充迷宫"))
        {
            mapGen.FloodFillMazeSingleStep();
            mapGen.RefreshMapView();
        }

        if (GUILayout.Button("生成门"))
        {
            mapGen.GenerateDoors();
            mapGen.RefreshMapView();
        }
        
        if (GUILayout.Button("反雕刻"))
        {
            mapGen.CarveDeadEnds();
            mapGen.RefreshMapView();
        }


        if (GUILayout.Button("生成完整地图"))
        {
            mapGen.GenerateWholeMap();
            mapGen.RefreshMapView();
        }
    }
}
