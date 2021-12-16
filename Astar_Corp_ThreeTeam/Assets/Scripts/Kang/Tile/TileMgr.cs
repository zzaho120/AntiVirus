using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileDirection
{
    None = 0,
    Top,
    Bot,
    Left,
    Right,
}

public class TileMgr : MonoBehaviour
{
    public GameObject tiles;

    public List<TileBase> tileList = new List<TileBase>();

    private int MAX_X_IDX = 10;
    private int MAX_Y_IDX = 2;
    private int MAX_Z_IDX = 10;

    private int OFFSET_X = 1;
    private int OFFSET_Y = 1;
    private int OFFSET_Z = 1;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        var tileCount = tiles.transform.childCount;
        for (int idx = 0; idx < tileCount; ++idx)
        {
            var go = tiles.transform.GetChild(idx);
            
            var tileBase = go.GetComponent<TileBase>();
            tileBase.Init();

            tileList.Add(tileBase);
        }

        for (int idx = 0; idx < tileList.Count; ++idx)
        {
            var tile = tileList[idx];
            if (idx % MAX_X_IDX > 0) CheckAdjTile(tile, tileList[idx - 1], WallType.Left, WallType.Right);
            if (idx % MAX_X_IDX < 9) CheckAdjTile(tile, tileList[idx + 1], WallType.Right, WallType.Left);
            if (idx / MAX_Z_IDX > 0) CheckAdjTile(tile, tileList[idx - MAX_Z_IDX], WallType.Bot, WallType.Top);
            if (idx / MAX_X_IDX < 9) CheckAdjTile(tile, tileList[idx + MAX_Z_IDX], WallType.Top, WallType.Bot);
        }
    }

    private void CheckAdjTile(TileBase thisTile, TileBase otherTile, WallType thisType, WallType otherType)
    {
        if (thisTile.tileIdx.y != otherTile.tileIdx.y)
            return;
        if (CheckWall(thisTile, thisType))
            return;
        if (CheckWall(otherTile, otherType))
            return;

        thisTile.adjNodes.Add(otherTile);
    }

    private bool CheckWall(TileBase tile, WallType type)
    {
        foreach (var wall in tile.wallList)
        {
            if (type == wall.type)
                return true;
        }
        return false;
    }
}