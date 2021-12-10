using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapDef
{
    public const int CELL_TYPE_FLOOR = 0;
    public const int CELL_TYPE_ROOM = 1;
    public const int CELL_TYPE_WALL = 2;
    public const int CELL_TYPE_CORRIDOR = 3;


    public const int DIRECTION_UP = 0;
    public const int DIRECTION_DOWN = 1;
    public const int DIRECTION_LEFT = 2;
    public const int DIRECTION_RIGHT = 3;

    private static int[] _neighbors_1 = new int[]
    {
            DIRECTION_UP,
            DIRECTION_DOWN,
            DIRECTION_LEFT,
            DIRECTION_RIGHT,
    };

    private static int[] _neighbors_2 = new int[]
    {
            DIRECTION_DOWN,
            DIRECTION_LEFT,
            DIRECTION_RIGHT,
            DIRECTION_UP,
    };

    private static int[] _neighbors_3 = new int[]
    {
            DIRECTION_LEFT,
            DIRECTION_RIGHT,
            DIRECTION_UP,
            DIRECTION_DOWN,
    };
    private static int[] _neighbors_4 = new int[]
    {
            DIRECTION_RIGHT,
            DIRECTION_UP,
            DIRECTION_DOWN,
            DIRECTION_LEFT,
    };

    public static List<int[]> NEIGHBORS_POOL = new List<int[]>
    {
        _neighbors_1,
        _neighbors_2,
        _neighbors_3,
        _neighbors_4
    };

    public static Vector2Int UP = new Vector2Int(0, 1);
    public static Vector2Int DOWN = new Vector2Int(0, -1);
    public static Vector2Int LEFT = new Vector2Int(-1, 0);
    public static Vector2Int RIGHT = new Vector2Int(1, 0);

    public static Vector2Int[] NEIGHBORS_MAP = new Vector2Int[]
    {
        UP, //up
        DOWN , //down
        LEFT, //left
        RIGHT, //right
    };

    public static Vector2Int[] OFFSET_MAP = new Vector2Int[]
    {
        UP, //up
        DOWN , //down
        LEFT, //left
        RIGHT, //right
    };
}
