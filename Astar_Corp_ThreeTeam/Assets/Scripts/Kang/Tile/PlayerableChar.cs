using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerableChar : MonoBehaviour
{
    [Header("Tile")]
    public Vector3 tileIdx;
    public TileBase currentTile;

    [Header("Value")]
    public int moveDistance;
    public int sightDistance;
    public bool isMove;
    public bool isTurnOver;
    

    private Dictionary<TileBase, int> moveDics =
        new Dictionary<TileBase, int>();
    private MeshRenderer ren;
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

        ren = GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        if (!isTurnOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentTile.OpenDoor(true);
                // test
                BattleMgr.Instance.fogMgr.UpdateFog();
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.name);
                    if (isMove)
                    {
                        if (hit.collider.tag == "Tile")
                        {
                            var tileBase = hit.collider.GetComponent<TileBase>();
                            if (moveDics.ContainsKey(tileBase))
                            {
                                BattleMgr.Instance.aStar.InitAStar(currentTile.tileIdx, tileBase.tileIdx);
                                MoveMode();
                                StartCoroutine(CoMove());
                            }
                        }
                    }
                    if (hit.collider.gameObject == gameObject)
                    {
                        MoveMode();
                    }
                }
            }
        }
    }

    public void StartTurn()
    {
        isTurnOver = false;
        ren.material.color = Color.white;
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

            // test
            BattleMgr.Instance.fogMgr.UpdateFog();
            yield return new WaitForSeconds(0.1f);
        }
        isTurnOver = true;
        ren.material.color = Color.gray;
    }

    public void MoveMode()
    {
        isMove = !isMove;

        if (isMove)
        {
            ren.material.color = Color.red;
            MoveFloodFill();
        }
        else
        {
            ren.material.color = Color.white;
            foreach (var pair in moveDics)
            {
                var tile = pair.Key;
                var tileRen = tile.GetComponent<MeshRenderer>();
                if (tileRen == null)
                {
                    var obj = tile.transform.GetChild(0);
                    tileRen = obj.GetComponent<MeshRenderer>();
                }
                tileRen.material.color = Color.white;
            }
        }
    }

    private void MoveFloodFill()
    {
        moveDics.Clear();
        moveDics.Add(currentTile, 0);

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

        cnt++;
        if (!moveDics.ContainsKey(tile))
            moveDics.Add(tile, cnt);
        else if (moveDics[tile] > cnt)
            moveDics[tile] = cnt;
        else
            return;

        var tileRen = tile.GetComponent<MeshRenderer>();
        if (tileRen == null)
        {
            var obj = tile.transform.GetChild(0);
            tileRen = obj.GetComponent<MeshRenderer>();
        }
        tileRen.material.color = Color.blue;

        foreach (var adjNode in tile.adjNodes)
            CheckMoveRange(adjNode, cnt);
    }
}
