using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum DirectionType
{
    None = 0,
    Top = 1 << 0,
    Bot = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3
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

    [Header("List")]
    public List<GameObject> wallList;

    public void Init()
    {
        InitIdx();
    }

    private void InitIdx()
    {
        tileIdx = transform.position;
        gameObject.name = $"Wall ({tileIdx.x}, {tileIdx.y}, {tileIdx.z} / {type.ToString()})";
    }

    public void EnableDisplay(bool isEnabled)
    {
        foreach (var wall in wallList)
        {
            var ren = wall.GetComponent<MeshRenderer>();
            ren.enabled = isEnabled;
        }
    }
}
