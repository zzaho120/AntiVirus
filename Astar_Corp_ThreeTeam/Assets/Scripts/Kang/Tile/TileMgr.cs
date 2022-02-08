using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileMgr : MonoBehaviour
{
    public GameObject tiles;
    public GameObject walls;
    public GameObject fogs;

    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public GameObject fogPrefab;

    public Dictionary<Vector3, TileBase> tileDics = new Dictionary<Vector3, TileBase>();
    public Dictionary<Vector3, TileBase> wallDics = new Dictionary<Vector3, TileBase>();

    public static int MAX_X_IDX = 60;
    public static int MAX_Z_IDX = 60;
 
    public void Init()
    {
        //GenerateTile();
        var parentCount = tiles.transform.childCount;
        for (int idx = 0; idx < parentCount; ++idx)
        {
            var parent = tiles.transform.GetChild(idx).GetChild(1);
            var tileCount = parent.childCount;
            for (int tileIdx = 0; tileIdx < tileCount; ++tileIdx)
            {
                var go = parent.GetChild(tileIdx);

                var tileBase = go.GetComponent<TileBase>();
                tileBase.Init(this);

                tileDics.Add(tileBase.tileIdx, tileBase);

                var fog = Instantiate(fogPrefab, fogs.transform);
                fog.transform.position = tileBase.transform.position + new Vector3(0, 1);
                tileBase.fogTile = fog.gameObject;
            }

        }

        parentCount = walls.transform.childCount;
        for (int idx = 0; idx < parentCount; ++idx)
        {
            Debug.Log(walls.transform.GetChild(idx).childCount);
            var parent = walls.transform.GetChild(idx).GetChild(1);
            var wallCount = parent.childCount;
            for (int wallIdx = 0; wallIdx < wallCount; ++wallIdx)
            {
                var go = parent.GetChild(wallIdx);

                var wallBase = go.GetComponent<TileBase>();
                wallBase.Init(this);

                wallDics.Add(wallBase.tileIdx, wallBase);
            }
        }

        foreach (var pair in tileDics)
        {
            var wallIdx = new Vector3(pair.Key.x, pair.Key.y + 1, pair.Key.z);
            if (wallDics.ContainsKey(wallIdx))
                pair.Value.wallTile = wallDics[wallIdx];
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