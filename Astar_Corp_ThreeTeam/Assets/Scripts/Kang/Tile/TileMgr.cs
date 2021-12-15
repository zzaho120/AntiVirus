using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OtherTileDirection
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
    public GameObject walls;

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
            tileBase.tileIdx = new Vector3(idx % MAX_X_IDX, 0, idx / MAX_Z_IDX);

            var tileIdx = tileBase.tileIdx;
            go.name = $"Tile ({tileIdx.x}, {tileIdx.y}, {tileIdx.z})";

            tileList.Add(tileBase);
        }

        var wallCount = walls.transform.childCount;
        for (int idx = 0; idx < wallCount; ++idx)
        {
            var go = walls.transform.GetChild(idx).gameObject;
            var wallBase = go.GetComponent<WallBase>();

            foreach (var tile in tileList)
            {
                if (tile.tileIdx == wallBase.tileIdx)
                    tile.objList.Add(go);
            }
        }

        for (int idx = 0; idx < tileList.Count; ++idx)
        {
            var tile = tileList[idx];
            tile.Init();

            if (idx % MAX_X_IDX > 0) CheckAdjTile(idx - 1, tile, OtherTileDirection.Left);
            if (idx % MAX_X_IDX < 9) CheckAdjTile(idx + 1, tile, OtherTileDirection.Right);
            if (idx / MAX_Z_IDX > 0) CheckAdjTile(idx - MAX_Z_IDX, tile, OtherTileDirection.Top);
            if (idx / MAX_X_IDX < 9) CheckAdjTile(idx + MAX_Z_IDX, tile, OtherTileDirection.Bot);
        }
    }

    private void CheckAdjTile(int idx, TileBase tile, OtherTileDirection type)
    {
        //bool isExistWall = false;
        //var otherTile = tileList[idx];
        //foreach(var obj in otherTile.objList)
        //{
        //    var wallBase = obj.GetComponent<WallBase>();

        //    if (wallBase == null)
        //        continue;

        //    switch (type)
        //    {
        //        case OtherTileDirection.Top:
        //            if (wallBase.type == WallType.Bot)
        //                isExistWall = true;
        //            break;
        //        case OtherTileDirection.Bot:
        //            if (wallBase.type == WallType.Top)
        //                isExistWall = true;
        //            break;
        //        case OtherTileDirection.Left:
        //            if (wallBase.type == WallType.Right)
        //                isExistWall = true;
        //            break;
        //        case OtherTileDirection.Right:
        //            if (wallBase.type == WallType.Left)
        //                isExistWall = true;
        //            break;
        //    }

        //    if (isExistWall)
        //        return;
        //}

        //switch (type)
        //{
        //    case OtherTileDirection.Top:
        //        if (tile.type == WallType.Bot)
        //            isExistWall = true;
        //        break;
        //    case OtherTileDirection.Bot:
        //        if (tile.type == WallType.Bot)
        //            isExistWall = true;
        //        break;
        //    case OtherTileDirection.Left:
        //        if (wallBase.type == WallType.Bot)
        //            isExistWall = true;
        //        break;
        //    case OtherTileDirection.Right:
        //        if (wallBase.type == WallType.Bot)
        //            isExistWall = true;
        //        break;
        //}
    }
}