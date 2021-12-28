using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileMgr : MonoBehaviour
{
    public GameObject tiles;
    public GameObject walls;

    public Dictionary<Vector3, TileBase> tileDics = new Dictionary<Vector3, TileBase>();
    public Dictionary<Vector3, WallBase> wallDics = new Dictionary<Vector3, WallBase>();
    public Dictionary<Vector2, List<TileBase>> tileVec2Dics = new Dictionary<Vector2, List<TileBase>>();

    public static int MAX_X_IDX = 16;
    public static int MAX_Z_IDX = 16;
    ScriptableMgr scriptableMgr;
    // Start is called before the first frame update
    void Start()
    {
        scriptableMgr = ScriptableMgr.Instance;
        Debug.Log(scriptableMgr);
        Debug.Log($"Test : {scriptableMgr.characterList.Count}");
    }
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

        var wallCount = walls.transform.childCount;
        for (int idx = 0; idx < wallCount; ++idx)
        {
            var go = walls.transform.GetChild(idx);

            var wallBase = go.GetComponent<WallBase>();
            wallBase.Init();

            wallDics.Add(wallBase.tileIdx, wallBase);
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

    private void InitAdjTile()
    {
        foreach (var pair in tileDics)
        {
            var idx = pair.Key;
            var tile = pair.Value;

            if (idx.z < MAX_Z_IDX - 1) CheckAdjTile(tile, new Vector3(idx.x, idx.y, idx.z + 1), DirectionType.Top, DirectionType.Bot);
            if (idx.z > 0) CheckAdjTile(tile, new Vector3(idx.x, idx.y, idx.z - 1), DirectionType.Bot, DirectionType.Top);
            if (idx.x > 0) CheckAdjTile(tile, new Vector3(idx.x - 1, idx.y, idx.z), DirectionType.Left, DirectionType.Right);
            if (idx.x < MAX_X_IDX - 1) CheckAdjTile(tile, new Vector3(idx.x + 1, idx.y, idx.z), DirectionType.Right, DirectionType.Left);
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
        var thisIdx = thisTile.tileIdx;
        var otherIdx = otherTile.tileIdx;

        if (thisIdx.z == otherIdx.z)
        {
            var wallIdx = new Vector3((thisIdx.x + otherIdx.x) * 0.5f, thisIdx.y, thisIdx.z);
            if (!wallDics.ContainsKey(wallIdx))
                thisTile.adjNodes.Add(otherTile);
        }
        else if (thisIdx.x == otherIdx.x)
        {
            var wallIdx = new Vector3(thisIdx.x, thisIdx.y, (thisIdx.z + otherIdx.z) * 0.5f);
            if (!wallDics.ContainsKey(wallIdx))
                thisTile.adjNodes.Add(otherTile);
        }
    }
}