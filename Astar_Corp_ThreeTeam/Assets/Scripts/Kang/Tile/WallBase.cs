using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionType
{
    None = 0,
    Top = 1,
    Bot,
    Left,
    Right,
}

public class WallBase : MonoBehaviour
{
    [Header("Values")]
    public Vector3 tileIdx;
    public DirectionType type;
    public bool isPassable;

    [Header("References")]
    public TileBase parentFloor;
    public TileBase parentSeiling;

    public void Init()
    {
        InitIdx();
    }

    private void InitIdx()
    {
        tileIdx = transform.position;
        gameObject.name = $"Wall ({tileIdx.x}, {tileIdx.y}, {tileIdx.z} / {type.ToString()})";
    }
}
