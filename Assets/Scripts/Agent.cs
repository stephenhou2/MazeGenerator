using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent:MonoBehaviour
{
    public Vector2Int pos;

    public void InitializedAgent(Vector2Int pos)
    {
        this.pos = pos;
        MoveTo(pos);
    }

    public void MoveTo(Vector2Int pos)
    {
        this.pos = pos;
        this.transform.position = new Vector3(pos.x, pos.y, 0);
    }
}
