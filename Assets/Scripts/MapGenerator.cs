using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator:MonoBehaviour
{
    public int WorkSpaceWidth = 100;
    public int WorkSpaceHeight = 80;
    public int RoomMaxTry = 100;

    public GameObject RoomNode;

    private List<Room> _allRooms = new List<Room>();

    private int[,] _mapCellType;

    private bool CheckRoomCanCreate(Vector2Int pos, int width, int height)
    {
        if (    pos.x - width < 0 
            || pos.x + width >= WorkSpaceWidth
            || pos.y - height < 0 
            || pos.y + height >= WorkSpaceHeight)
        {
            return false;
        }

        foreach (Room room in _allRooms)
        {
            if (Mathf.Abs(room.Pos.x - pos.x) + 1 < room.Width + width
                && Mathf.Abs(room.Pos.y - pos.y) + 1 < room.Height + height)
            {
                return false;
            }
        }
        return true;
    }

    private void AddRoom(Vector2Int pos, int width, int height)
    {
        Room room = new Room(pos, width, height);
         _allRooms.Add(room);

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = RoomNode.transform;

        quad.transform.position = new Vector3(pos.x+ width / 2.0f,pos.y+ height / 2.0f,0);
        quad.transform.localScale = new Vector3(width, height, 1);
        quad.transform.rotation = Quaternion.identity;

    }

    [ContextMenu("Test")]
    private void Test()
    {
        for(int i = RoomNode.transform.childCount-1; i>=0;i--)
        {
            DestroyImmediate(RoomNode.transform.GetChild(i).gameObject);
        }

        GenerateMapRooms(3, 7, RoomMaxTry);
    }

    public void GenerateMapRooms(int minSize,int maxSize,int maxTry)
    {
        _allRooms.Clear();
        int cnt = 0;
        while(cnt < maxTry)
        {
            // 房间的宽高
            int width = Random.Range(minSize, maxSize);
            int height = Random.Range(minSize, maxSize);

            int offset = Mathf.RoundToInt(minSize / 2);
            int randomPosX = Random.Range(offset, WorkSpaceWidth - offset);
            int randomPosY = Random.Range(offset, WorkSpaceHeight - offset);

            Vector2Int pos = new Vector2Int(randomPosX, randomPosY);
            //Vector2Int size = new Vector2Int(width, height);
            if (CheckRoomCanCreate(pos, width, height))
            {
                AddRoom(pos, width, height);
            }

            cnt++;
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#if UNITY_EDITOR
    public bool ShowGridLine = true;

    private void OnDrawGizmos()
    {
        if (ShowGridLine)
        {
            for (int i = 0; i < WorkSpaceHeight; i++)
            {
                Gizmos.DrawLine(new Vector3(0, i, 0), new Vector3(WorkSpaceWidth, i, 0));
            }
            for (int i = 0; i < WorkSpaceWidth; i++)
            {
                Gizmos.DrawLine(new Vector3(i, 0, 0), new Vector3(i, WorkSpaceHeight, 0));
            }
        }
    }
#endif


}
