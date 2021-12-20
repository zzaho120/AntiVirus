using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Vector3 tileIdx;
    public TileBase currentTile;

    public int moveDistance;
    public bool isMove;
    private Dictionary<Vector3, TileBase> moveDics =
        new Dictionary<Vector3, TileBase>();
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
            BattleMgr.Instance.aStar.InitAStar(currentTile.tileIdx, new Vector3(5, 6, 6));
            StartCoroutine(CoMove());
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    MoveMode();
                }
            }
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
    }

    private IEnumerator CoMove()
    {
        var path = BattleMgr.Instance.aStar.pathList;
        while (path.Count > 0)
        {
            var aStarTile = path.Pop();

            MoveTile(aStarTile.tileBase.tileIdx);
            yield return new WaitForSeconds(1f);
        }
    }

    public void MoveMode()
    {
        isMove = !isMove;
        var ren = GetComponent<MeshRenderer>();

        if (isMove)
        {
            ren.material.color = Color.red;
            MoveFloodFill();
        }
        else
        {
            ren.material.color = Color.white;
            foreach (var tile in moveDics)
            {
                var tileRen = tile.Value.GetComponent<MeshRenderer>();
                if (tileRen == null)
                {
                    var obj = tile.Value.transform.GetChild(0);
                    tileRen = obj.GetComponent<MeshRenderer>();
                }
                tileRen.material.color = Color.white;
            }
        }
    }

    private void MoveFloodFill()
    {
        moveDics.Clear();
        moveDics.Add(tileIdx, currentTile);

        var cnt = 0;
        foreach (var adjNode in currentTile.adjNodes)
        {
            CheckMoveRange(adjNode, cnt);
        }
    }

    private void CheckMoveRange(TileBase tile, int cnt)
    {
        if (cnt >= moveDistance)
            return;

        if (!moveDics.ContainsKey(tile.tileIdx))
            moveDics.Add(tile.tileIdx, tile);
        else
            return;

        var ren = tile.GetComponent<MeshRenderer>();
        if (ren == null)
        {
            var obj = tile.transform.GetChild(0);
            ren = obj.GetComponent<MeshRenderer>();
        }
        ren.material.color = Color.blue;

        cnt++;
        Debug.Log($"{tile.tileIdx}, {cnt}");
        foreach (var adjNode in tile.adjNodes)
            CheckMoveRange(adjNode, cnt);
    }
}
