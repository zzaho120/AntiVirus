using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallType
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
    public WallType type;

    [Header("References")]
    public TileBase parentTile;

    [Header("Lists")]
    public List<GameObject> wallList;
}
