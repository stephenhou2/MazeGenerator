using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int Pos;
    public int Width_Half;
    public int Height_Half;

    public Room(Vector2Int pos,int halfWidth,int halfHeight)
    {
        this.Pos = pos;
        this.Width_Half = halfWidth;
        this.Height_Half = halfHeight;
    }

    public int Left
    {
        get
        {
            return Pos.x - Width_Half;
        }
    }

    public int Right
    {
        get
        {
            return Pos.x + Width_Half;
        }
    }
    
    public int Bottom
    {
        get
        {
            return Pos.y - Height_Half;
        }
    }

    public int Top
    {
        get
        {
            return Pos.y + Height_Half;
        }
    }
}
