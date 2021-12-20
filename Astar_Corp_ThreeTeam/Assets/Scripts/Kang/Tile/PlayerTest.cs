using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Vector3 tileIdx;
    public TileBase currentTile;

    public void Init()
    {
        var dics = BattleMgr.Instance.tileMgr.tileDics;
        var pos = transform.position;
        tileIdx = new Vector3(pos.x, pos.y - 1, pos.z);
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            var nextIdx = new Vector3(tileIdx.x, tileIdx.y, tileIdx.z + 1);
            MoveTile(nextIdx);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            var nextIdx = new Vector3(tileIdx.x - 1, tileIdx.y, tileIdx.z);
            MoveTile(nextIdx);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var nextIdx = new Vector3(tileIdx.x, tileIdx.y, tileIdx.z - 1);
            MoveTile(nextIdx);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var nextIdx = new Vector3(tileIdx.x + 1, tileIdx.y, tileIdx.z);
            MoveTile(nextIdx);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentTile.OpenDoor(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            BattleMgr.Instance.aStar.InitAStar(currentTile.tileIdx, new Vector3(3, 0, 1));
        }
    }
    
    private void MoveTile(Vector3 nextIdx)
    {
        foreach (var tile in currentTile.adjNodes)
        {
            if (tile.tileIdx.x == nextIdx.x && tile.tileIdx.z == nextIdx.z)
            {
                tileIdx = nextIdx;
                currentTile = tile;
                transform.position = new Vector3(tile.tileIdx.x, tile.tileIdx.y + 1, tile.tileIdx.z);
            }
        }
        Debug.Log(currentTile.tileIdx);
    }
}
