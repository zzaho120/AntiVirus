using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTile 
{
    public bool listOn;
    public int F, G, H;
    public AStarTile parent;
    public TileBase tileBase;

    public AStarTile(TileBase tile)
    {
        tileBase = tile;
    }
}


public class PathMgr : MonoBehaviour
{
    private Vector3 startIdx;
    private Vector3 endIdx;
    private Vector3 closeIdx;
    private int lastIdx;
    private int Cg;

    private bool isArrive;
    private bool isNotFound;
    private bool inited;

    private List<AStarTile> closeList = new List<AStarTile>();
    private List<AStarTile> openList = new List<AStarTile>();
    private Dictionary<Vector3, AStarTile> aStarTiles = new Dictionary<Vector3, AStarTile>();

    private readonly int StraightWeight = 10;
    private readonly int DiagonalWeight = 14;

    [HideInInspector]
    public Stack<AStarTile> pathList = new Stack<AStarTile>();

    public void Init()
    {
        InitAStarTile();
    }
    private void InitAStarTile()
    {
        if (!inited)
        {
            inited = true;

            var tileDics = BattleMgr.Instance.tileMgr.tileDics;
            foreach (var pair in tileDics)
            {
                var tile = pair.Value;
                aStarTiles.Add(tile.tileIdx, new AStarTile(tile));
            }
        }
    }
    public void InitAStar(Vector3 start, Vector3 end)
    {
        closeList.Clear();
        openList.Clear();
        
        startIdx = start;
        endIdx = end;
        isArrive = false;
        isNotFound = false;

        closeList.Add(aStarTiles[start]);
        foreach (var aStarTile in aStarTiles)
        {
            aStarTile.Value.listOn = false;
            aStarTile.Value.F = 0;
            aStarTile.Value.G = 0;
            aStarTile.Value.H = 0;
            aStarTile.Value.parent = null;
        }

        while (!isArrive && !isNotFound)
        {
            AddOpenList();
            CalculateH();
            CalculateF();
            AddCloseList();
            CheckArrive();
        }
    }

    private void AddOpenList()
    {
        var aStarTile = closeList[closeList.Count - 1];
        var tile = aStarTile.tileBase;
        Cg = aStarTile.G;

        foreach (var adjTile in tile.adjNodes)
        {
            CheckGValue(tile.tileIdx, adjTile.tileIdx);
        }
    }

    private void CheckGValue(Vector3 thisIdx, Vector3 otherIdx)
    {
        var aStarTile = aStarTiles[otherIdx];
        if (!aStarTile.listOn)
        {
            aStarTile.listOn = true;
            aStarTile.G = Cg + StraightWeight;
            aStarTile.parent = aStarTiles[thisIdx];
            openList.Add(aStarTile);
        }
        else if(Cg + StraightWeight < aStarTile.G)
        {
            aStarTile.G = Cg + StraightWeight;
            aStarTile.parent = aStarTiles[thisIdx];
        }
    }

    private void CalculateH()
    {
        foreach (var aStarTile in openList)
        {
            var tile = aStarTile.tileBase;
            var xValue = Mathf.Abs(Mathf.FloorToInt(endIdx.x - tile.tileIdx.x)) * StraightWeight;
            var yValue = Mathf.Abs(Mathf.FloorToInt(endIdx.y - tile.tileIdx.y)) * StraightWeight;
            var zValue = Mathf.Abs(Mathf.FloorToInt(endIdx.z - tile.tileIdx.z)) * StraightWeight;

            aStarTile.H = xValue + yValue + zValue;
        }
    }

    private void CalculateF()
    {
        foreach (var aStarTile in openList)
        {
            aStarTile.F = aStarTile.G + aStarTile.H;
        }
    }

    private void AddCloseList()
    {
        if (openList.Count == 0)
        {
            isNotFound = true;
            Debug.Log("길을 찾지 못했습니다.");
            return;
        }

        var minF = int.MinValue;
        int openIdx = 0;

        for (int idx = 0; idx < openList.Count; ++idx)
        {
            if (openList[idx].F < minF)
            {
                minF = openList[idx].F;
                openIdx = idx;
            }
        }

        closeList.Add(openList[openIdx]);
        openList.RemoveAt(openIdx);
    }

    private void CheckArrive()
    {
        if (closeList[closeList.Count - 1].tileBase.tileIdx == endIdx)
        {
            isArrive = true;

            SetPath(closeList[closeList.Count - 1]);
            pathList.Pop();
        }
    }

    private void SetPath(AStarTile tile)
    {
        pathList.Push(tile);

        if (tile.tileBase.tileIdx == startIdx)
            return;

        tile = tile.parent;

        if (tile == null)
            return;
        else
            SetPath(tile);
    }
}