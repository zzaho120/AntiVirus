using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileMgr : MonoBehaviour
{
    public GameObject tiles;
    public GameObject walls;

    public GameObject tilePrefab;

    public Dictionary<Vector3, TileBase> tileDics = new Dictionary<Vector3, TileBase>();
    public Dictionary<Vector3, TileBase> wallDics = new Dictionary<Vector3, TileBase>();
    public Dictionary<Vector2, List<TileBase>> tileVec2Dics = new Dictionary<Vector2, List<TileBase>>();

    public static int MAX_X_IDX = 24;
    public static int MAX_Z_IDX = 24;
 
    public void Init()
    {
        GenerateTile();
        var tileCount = tiles.transform.childCount;
        for (int idx = 0; idx < tileCount; ++idx)
        {
            var go = tiles.transform.GetChild(idx);
            
            var tileBase = go.GetComponent<TileBase>();
            tileBase.Init(this);

            tileDics.Add(tileBase.tileIdx, tileBase);
        }

        var wallCount = walls.transform.childCount;
        for (int idx = 0; idx < wallCount; ++idx)
        {
            var go = walls.transform.GetChild(idx);

            var wallBase = go.GetComponent<TileBase>();
            wallBase.Init(this);

            wallDics.Add(wallBase.tileIdx, wallBase);
        }

        foreach (var pair in tileDics)
        {
            var wallIdx = new Vector3(pair.Key.x, pair.Key.y + 1, pair.Key.z);
            if (wallDics.ContainsKey(wallIdx))
                pair.Value.wallTile = wallDics[wallIdx];
        }

        foreach (var pair in tileDics)
        {
            var tileIdx = new Vector2(pair.Key.x, pair.Key.z);
            if (!tileVec2Dics.ContainsKey(tileIdx))
                tileVec2Dics.Add(tileIdx, new List<TileBase>());
            tileVec2Dics[tileIdx].Add(pair.Value);
        }

        InitAdjTile();
    }

    private void GenerateTile()
    {
        for (var i = 0; i < MAX_Z_IDX; ++i)
        {
            for (var j = 0; j < MAX_X_IDX; ++j)
            {
                var go = Instantiate(tilePrefab, new Vector3(j, 0, i), Quaternion.identity);
                go.transform.SetParent(tiles.transform);
            }
        }
    }

    private void InitAdjTile()
    {
        foreach (var pair in tileDics)
        {
            var idx = pair.Key;
            var tile = pair.Value;

            if (idx.z < MAX_Z_IDX - 1) 
                CheckAdjTile(tile, new Vector3(idx.x, idx.y, idx.z + 1));
            if (idx.z > 0) 
                CheckAdjTile(tile, new Vector3(idx.x, idx.y, idx.z - 1));
            if (idx.x > 0) 
                CheckAdjTile(tile, new Vector3(idx.x - 1, idx.y, idx.z));
            if (idx.x < MAX_X_IDX - 1) 
                CheckAdjTile(tile, new Vector3(idx.x + 1, idx.y, idx.z));
        }
    }

    public void CheckAdjTile(TileBase thisTile, Vector3 otherIdx)
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
                CheckObj(thisTile, otherTile);
            }
            else
            {
                // 상향 계단이 있을 경우
                var stairTile = tileDics[upStairIdx];
                CheckObj(thisTile, stairTile);
            }
        }
        else
        {
            var downStairIdx = new Vector3(otherIdx.x, otherIdx.y - 1, otherIdx.z);
            if (tileDics.ContainsKey(downStairIdx))
            {
                // 하향 계단이 있을 경우
                var stairTile = tileDics[downStairIdx];
                CheckObj(thisTile, stairTile);
            }
        }
    }

    private void CheckObj(TileBase thisTile, TileBase otherTile)
    {
        if (thisTile.wallTile != null || otherTile.wallTile)
            return;

        thisTile.adjNodes.Add(otherTile);
    }
}