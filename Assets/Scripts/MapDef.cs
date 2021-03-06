using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapDef
{
    public const int CELL_TYPE_INVALID = -1;
    public const int CELL_TYPE_FLOOR = 0;
    public const int CELL_TYPE_ROOM = 1;
    public const int CELL_TYPE_WALL = 2;
    public const int CELL_TYPE_DOOR = 3;

    public static Vector2Int UP = new Vector2Int(0, 1);
    public static Vector2Int DOWN = new Vector2Int(0, -1);
    public static Vector2Int LEFT = new Vector2Int(-1, 0);
    public static Vector2Int RIGHT = new Vector2Int(1, 0);

    public static float DOOR_CHANCE = 0.3f;

    public const int DIR_UP = 0;
    public const int DIR_LEFT = 1;
    public const int DIR_DOWN = 2;
    public const int DIR_RIGHT = 3;

    public static int[] DIRECTION_MAP = new int[]
    {
        DIR_UP,
        DIR_LEFT,
        DIR_DOWN,
        DIR_RIGHT
    };

    public static Vector2Int[] _neighbors_1 = new Vector2Int[]
    {
            UP,
            DOWN,
            LEFT,
            RIGHT,
    };

    public static Vector2Int[] _neighbors_2 = new Vector2Int[]
    {
            DOWN,
            LEFT,
            RIGHT,
            UP,
    };

    public static Vector2Int[] _neighbors_3 = new Vector2Int[]
    {
            LEFT,
            RIGHT,
            UP,
            DOWN,
    };
    public static Vector2Int[] _neighbors_4 = new Vector2Int[]
    {
            RIGHT,
            UP,
            DOWN,
            LEFT,
    };

    public static List<Vector2Int[]> NEIGHBORS_POOL = new List<Vector2Int[]>
    {
        _neighbors_1,
        _neighbors_2,
        _neighbors_3,
        _neighbors_4
    };

    public static Vector2Int[] FULL_NEIGHBORS = new Vector2Int[]
    {
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
    };
}
