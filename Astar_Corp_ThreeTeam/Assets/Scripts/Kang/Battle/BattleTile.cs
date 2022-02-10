using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTile : MonoBehaviour
{
    [Header("Tile")]
    public Vector3 tileIdx;
    public TileBase currentTile;

    public virtual void Init()
    {
        var dics = BattleMgr.Instance.tileMgr.tileDics;
        var pos = transform.position;
        tileIdx = new Vector3(pos.x, pos.y - 0.5f, (int)pos.z);
        foreach (var pair in dics)
        {
            var tile = pair.Value;
            if (tile.tileIdx == tileIdx)
            {
                currentTile = tile;
                break;
            }
        }

        if (currentTile != null)
            currentTile.charObj = gameObject;
    }

    public void InitHint()
    {
        var dics = BattleMgr.Instance.tileMgr.tileDics;
        var pos = transform.position;
        tileIdx = new Vector3(pos.x, pos.y - 0.5f, (int)pos.z);
        foreach (var pair in dics)
        {
            var tile = pair.Value;
            if (tile.tileIdx == tileIdx)
            {
                currentTile = tile;
                break;
            }
        }
    }
}
