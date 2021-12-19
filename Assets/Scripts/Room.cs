using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private RectInt _rect;

    public Room(Vector2Int pos,int width,int height)
    {
        this._rect = new RectInt(pos, new Vector2Int(width, height));
    }

    public bool Overlaps(RectInt rect)
    {
        return _rect.Overlaps(rect);
    }

    public Vector2Int Pos
    {
        get
        {
            return _rect.position;
        }
    }

    public int Width
    {
        get
        {
            return _rect.size.x;
        }
    }

    public int Height
    {
        get
        {
            return _rect.size.y;
        }
    }

    public int Left
    {
        get
        {
            return _rect.xMin;
        }
    }

    public int Right
    {
        get
        {
            return _rect.xMax-1;
        }
    }
    
    public int Bottom
    {
        get
        {
            return _rect.yMin;
        }
    }

    public int Top
    {
        get
        {
            return _rect.yMax-1;
        }
    }
}
