using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Wall
{
    None = 0,
    Top = 0001,
    Bot = 0010,
    Left = 0100,
    Right = 1000,
}

public class TileBase : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject wallPrefab;

    [Header("Value")]
    public Vector3 tileIdx;
    public Wall wallValue;

    public void Generate()
    {
        if ((wallValue & Wall.Top) != 0)
            GenerateWall(0.1f, 1, 0.45f, 0);
        if((wallValue & Wall.Bot) != 0)
            GenerateWall(0.1f, 1, -0.45f, 0);
        if ((wallValue & Wall.Left) != 0)
            GenerateWall(1, 0.1f, 0, -0.45f);
        if ((wallValue & Wall.Right) != 0)
            GenerateWall(1, 0.1f, 0, 0.45f);
    }

    public void GenerateWall(float xScale, float zScale, float xPos, float zPos)
    {
        var go = Instantiate(wallPrefab, gameObject.transform);
        go.transform.localScale = new Vector3(xScale, 1, zScale);
        go.transform.localPosition = new Vector3(xPos, tileIdx.y + 1, zPos);
    }
}