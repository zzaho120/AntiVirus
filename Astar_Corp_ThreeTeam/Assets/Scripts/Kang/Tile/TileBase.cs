using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    [Header("Value")]
    public Vector3 tileIdx;

    [Header("List")]
    public List<TileBase> adjNodes;
    public List<WallBase> wallList;
    public List<GameObject> objList;

    public void Init()
    {
        InitIdx();
        InitWall();
    }

    private void InitIdx()
    {
        tileIdx = transform.position;
        gameObject.name = $"Tile ({tileIdx.x}, {tileIdx.y}, {tileIdx.z})";
    }

    private void InitWall()
    {
        if (wallList.Count != 0)
        {
            foreach (var wall in wallList)
            {
                wall.Init();

                if (wall.tileIdx.y == tileIdx.y) // °°Àº Ãþ
                    wall.parentFloor = this;
                else if (wall.tileIdx.y == tileIdx.y - 1) // À­ Ãþ
                    wall.parentSeiling = this;
            }
        }
    }
}