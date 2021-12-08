using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapDef
{
    public const int CELL_TYPE_EMPTY = 0;
    public const int CELL_TYPE_ROOM = 1;
    public const int CELL_TYPE_WALL = 2;

    private static Vector2Int[] _neighbors_1 = new Vector2Int[]
{
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
            new Vector2Int(1,0),
            new Vector2Int(0,1),
};

    private static Vector2Int[] _neighbors_2 = new Vector2Int[]
    {
            new Vector2Int(0,-1),
            new Vector2Int(1,0),
            new Vector2Int(0,1),
            new Vector2Int(-1,0),
    };

    private static Vector2Int[] _neighbors_3 = new Vector2Int[]
    {
                new Vector2Int(1,0),
                new Vector2Int(0,1),
                new Vector2Int(-1,0),
                new Vector2Int(0,-1),
    };
    private static Vector2Int[] _neighbors_4 = new Vector2Int[]
    {
                new Vector2Int(0,1),
                new Vector2Int(-1,0),
                new Vector2Int(0,-1),
                new Vector2Int(1,0),
    };

    public static List<Vector2Int[]> NEIGHBORS_POOL = new List<Vector2Int[]>
    {
        _neighbors_1,
        _neighbors_2,
        _neighbors_3,
        _neighbors_4
    };
}
