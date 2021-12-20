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

    public Dictionary<Vector3, TileBase> tileDics = new Dictionary<Vector3, TileBase>();

    private int MAX_X_IDX = 10;
    private int MAX_Y_IDX = 2;
    private int MAX_Z_IDX = 10;

    private int OFFSET_X = 1;
    private int OFFSET_Y = 1;
    private int OFFSET_Z = 1;

    public void Init()
    {
        var tileCount = tiles.transform.childCount;
        for (int idx = 0; idx < tileCount; ++idx)
        {
            var go = tiles.transform.GetChild(idx);
            
            var tileBase = go.GetComponent<TileBase>();
            tileBase.Init(this);

            tileDics.Add(tileBase.tileIdx, tileBase);
        }

        InitAdjTile();
    }

    private void InitAdjTile()
    {
        foreach (var pair in tileDics)
        {
            var idx = pair.Key;
            var tile = pair.Value;

            if (idx.x > 0) CheckAdjTile(tile, new Vector3(idx.x - 1, idx.y, idx.z), DirectionType.Left, DirectionType.Right);
            if (idx.x < MAX_X_IDX - 1) CheckAdjTile(tile, new Vector3(idx.x + 1, idx.y, idx.z), DirectionType.Right, DirectionType.Left);
            if (idx.z > 0) CheckAdjTile(tile, new Vector3(idx.x, idx.y, idx.z - 1), DirectionType.Bot, DirectionType.Top);
            if (idx.z < MAX_Z_IDX - 1) CheckAdjTile(tile, new Vector3(idx.x, idx.y, idx.z + 1), DirectionType.Top, DirectionType.Bot);
        }
    }

    public void CheckAdjTile(TileBase thisTile, Vector3 otherIdx, DirectionType thisType, DirectionType otherType)
    {
        // 자기 위에 타일이 있을 경우
        var upTile = new Vector3(thisTile.tileIdx.x, thisTile.tileIdx.y + 1, thisTile.tileIdx.z);
        if (tileDics.ContainsKey(upTile))
            return;


        if (tileDics.ContainsKey(otherIdx))
        {
            var otherTile = tileDics[otherIdx];
            var upStairIdx = new Vector3(otherIdx.x, otherIdx.y + 1, otherIdx.z);
            if (!tileDics.ContainsKey(upStairIdx))
            {
                // 같은 층일 경우
                if (thisTile.CheckObj(thisType))
                    return;
                if (otherTile.CheckObj(otherType))
                    return;

                thisTile.adjNodes.Add(otherTile);
            }
            else
            {
                // 상향 계단이 있을 경우
                var stairTile = tileDics[upStairIdx];
                if (thisTile.CheckObj(thisType))
                    return;
                if (otherTile.CheckObj(otherType))
                    return;
                if (stairTile.CheckObj(otherType))
                    return;

                thisTile.adjNodes.Add(stairTile);
            }
        }
        else
        {
            var downStairIdx = new Vector3(otherIdx.x, otherIdx.y - 1, otherIdx.z);
            if (tileDics.ContainsKey(downStairIdx))
            {
                // 하향 계단이 있을 경우
                var stairTile = tileDics[downStairIdx];
                if (thisTile.CheckObj(thisType))
                    return;
                if (stairTile.CheckObj(otherType))
                    return;

                thisTile.adjNodes.Add(stairTile);
            }
        }
    }
}