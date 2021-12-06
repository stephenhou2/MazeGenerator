using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int Pos;
    public int Width;
    public int Height;

    public Room(Vector2Int pos,int width,int height)
    {
        this.Pos = pos;
        this.Width = width;
        this.Height = height;
    }
}
