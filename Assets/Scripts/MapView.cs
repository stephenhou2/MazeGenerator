using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    public GameObject RoomNode;
    public GameObject WallNode;
    public GameObject DoorNode;

    public Material roomMat;
    public Material wallMat;
    public Material doorMat;
    public Material agentMat;

    private int WorkSpaceWidth;
    private int WorkSpaceHeight;

    public  void AddRoomView(Vector2Int pos, int width, int height)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = RoomNode.transform;

        quad.transform.position = new Vector3(pos.x+width/2 , pos.y+height/2 , 0);
        quad.transform.localScale = new Vector3(width, height, 1);
        quad.transform.rotation = Quaternion.identity;
        quad.name = "_ROOM_";
        quad.GetComponent<MeshRenderer>().material = roomMat;
        //quad.hideFlags = HideFlags.HideInHierarchy;
    }

    public void AddWallView(Vector2Int pos)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = WallNode.transform;

        quad.transform.position = new Vector3(pos.x, pos.y, 0);
        quad.transform.localScale = Vector3.one;
        quad.transform.rotation = Quaternion.identity;
        quad.name = "_WALL_";
        quad.GetComponent<MeshRenderer>().material = wallMat;
        //quad.hideFlags = HideFlags.HideInHierarchy;
    }


    public void AddDoorView(Vector2Int pos)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = DoorNode.transform;

        quad.transform.position = new Vector3(pos.x, pos.y, 0);
        quad.transform.localScale = Vector3.one;
        quad.transform.rotation = Quaternion.identity;
        quad.name = "_DOOR_";
        quad.GetComponent<MeshRenderer>().material = doorMat;
        //quad.hideFlags = HideFlags.HideInHierarchy;
    }

    private void InitializeMapView(int mapWidth,int mapHeight)
    {
        WorkSpaceWidth = mapWidth;
        WorkSpaceHeight = mapHeight;
    }

    public void Reset(int mapWidth, int mapHeight)
    {
        for (int i = RoomNode.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(RoomNode.transform.GetChild(i).gameObject);
        }        
        
        for (int i = WallNode.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(WallNode.transform.GetChild(i).gameObject);
        }   
        
        for (int i = DoorNode.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(DoorNode.transform.GetChild(i).gameObject);
        } 

        InitializeMapView(mapWidth, mapHeight);
    }


#if UNITY_EDITOR

    public bool ShowGridLine;

    private void OnDrawGizmos()
    {
        if(ShowGridLine)
        {
            Gizmos.color = Color.grey;

            for (int i = 1; i < WorkSpaceHeight; i++)
            {
                Gizmos.DrawLine(new Vector3(-0.5f, i-0.5f, 0), new Vector3(WorkSpaceWidth-0.5f, i - 0.5f, 0));
            }
            for (int i = 1; i < WorkSpaceWidth; i++)
            {
                Gizmos.DrawLine(new Vector3(i - 0.5f, -0.5f, 0), new Vector3(i - 0.5f, WorkSpaceHeight - 0.5f, 0));
            }
        }
    }

#endif
}
