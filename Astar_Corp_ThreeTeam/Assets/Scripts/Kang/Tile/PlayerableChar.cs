using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerableChar : BattleChar
{
    [Header("Character")]
    public CharacterStats characterStats;

    [Header("Value")]
    public int moveDistance;
    public int sightDistance;
    public bool isMove;
    public bool isAttack;
    public bool isSelected;
    public bool isTurnOver;

    private Dictionary<TileBase, int> moveDics =
        new Dictionary<TileBase, int>();

    private Dictionary<Vector2, List<TileBase>> attackDics =
        new Dictionary<Vector2, List<TileBase>>();

    private Dictionary<Vector2, List<TileBase>> tileVec2Dics;

    private Dictionary<Vector3, WallBase> wallDics;

    private MeshRenderer ren;

    public override void Init()
    {
        base.Init();
        ren = GetComponent<MeshRenderer>();
        characterStats.character = (Character)Instantiate(Resources.Load("Choi/Datas/Characters/Sniper"));
        characterStats.Init();

        tileVec2Dics = BattleMgr.Instance.tileMgr.tileVec2Dics;
        wallDics = BattleMgr.Instance.tileMgr.wallDics;
    }

    public void Update()
    {
        if (!isTurnOver)
        {
            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit))
                {
                    if (isMove && hit.collider.tag == "Tile")
                    {
                        var tileBase = hit.collider.GetComponent<TileBase>();
                        if (moveDics.ContainsKey(tileBase))
                            ActionMove(tileBase);
                    }
                    else if (isAttack && hit.collider.tag == "BattleMonster")
                    {
                        Debug.Log(hit.collider.name);
                        var monster = hit.collider.GetComponent<MonsterChar>();
                        var tileIdx = new Vector2(monster.tileIdx.x, monster.tileIdx.z);
                        if (attackDics.ContainsKey(tileIdx))
                            ActionAttack(monster);
                    }
                    else if (hit.collider.gameObject == gameObject)
                    {
                        isSelected = !isSelected;
                        if (isSelected)
                        {
                            var playerAction = BattleMgr.Instance.BattleWindowMgr.Open(0).GetComponent<PlayerActionWindow>();
                            playerAction.curChar = this;
                            ren.material.color = Color.red;
                        }
                        else
                        {
                            var window = BattleMgr.Instance.BattleWindowMgr.GetWindow(0);
                            var playerAction = window.GetComponent<PlayerActionWindow>();
                            playerAction.curChar = null;
                            window.Close();
                            ren.material.color = Color.white;
                        }
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

    private IEnumerator CoMove()
    {
        var path = BattleMgr.Instance.aStar.pathList;
        while (path.Count > 0)
        {
            var aStarTile = path.Pop();

            if (aStarTile.tileBase.charObj != null)
                break;

            MoveTile(aStarTile.tileBase.tileIdx);

            // test
            BattleMgr.Instance.fogMgr.UpdateFog();
            yield return new WaitForSeconds(0.1f);
        }
        TurnOver();
    }

    private void MoveTile(Vector3 nextIdx)
    {
        foreach (var tile in currentTile.adjNodes)
        {
            if (tile.tileIdx.x == nextIdx.x && tile.tileIdx.z == nextIdx.z)
            {
                currentTile.charObj = null;
                tileIdx = nextIdx;
                currentTile = tile;
                currentTile.charObj = gameObject;
                transform.position = new Vector3(tile.tileIdx.x, tile.tileIdx.y + 1, tile.tileIdx.z);
            }
        }
    }

    public void MoveMode()
    {
        isMove = !isMove;

        if (isMove && isSelected)
        {
            FloodFillMove();
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

    private void FloodFillMove()
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
        {
            CheckMoveRange(adjNode, cnt);
        }
    }

    private void ActionMove(TileBase tileBase)
    {
        BattleMgr.Instance.aStar.InitAStar(currentTile.tileIdx, tileBase.tileIdx);
        MoveMode();
        StartCoroutine(CoMove());
    }

    public void AttackMode()
    {
        isAttack = !isAttack;

        if (isAttack)
        {
            FloodFillAttack();
            foreach (var pair in attackDics)
            {
                foreach (var tile in pair.Value)
                {
                    var tileRen = tile.transform.GetChild(0).GetComponent<MeshRenderer>();
                    tileRen.material.color = Color.red;
                }
            }
        }
        else
        {
            foreach (var pair in attackDics)
            {
                foreach (var tile in pair.Value)
                {
                    var tileRen = tile.transform.GetChild(0).GetComponent<MeshRenderer>();
                    tileRen.material.color = Color.white;
                }
            }
        }
    }

    private void FloodFillAttack()
    {
        attackDics.Clear();
        var tileIdx = new Vector2(currentTile.tileIdx.x, currentTile.tileIdx.z);
        attackDics.Add(tileIdx, tileVec2Dics[tileIdx]);

        var cnt = 0;

        if (!wallDics.ContainsKey(new Vector3(tileIdx.x, currentTile.tileIdx.y, tileIdx.y + 0.5f)))
            CheckAttackRange(new Vector2(tileIdx.x, tileIdx.y + 1), cnt);

        if (!wallDics.ContainsKey(new Vector3(tileIdx.x, currentTile.tileIdx.y, tileIdx.y - 0.5f)))
            CheckAttackRange(new Vector2(tileIdx.x, tileIdx.y - 1), cnt);

        if (!wallDics.ContainsKey(new Vector3(tileIdx.x - 0.5f, currentTile.tileIdx.y, tileIdx.y)))
            CheckAttackRange(new Vector2(tileIdx.x - 1, tileIdx.y), cnt);

        if (!wallDics.ContainsKey(new Vector3(tileIdx.x + 0.5f, currentTile.tileIdx.y, tileIdx.y)))
            CheckAttackRange(new Vector2(tileIdx.x + 1, tileIdx.y), cnt);
    }

    private void CheckAttackRange(Vector2 tileIdx, int cnt)
    {
        if (cnt >= characterStats.Range)
            return;

        cnt++;
        if (tileVec2Dics.ContainsKey(tileIdx))
        {
            if (!attackDics.ContainsKey(tileIdx))
                attackDics.Add(tileIdx, tileVec2Dics[tileIdx]);
            else
                return;
        }

        if (!wallDics.ContainsKey(new Vector3(tileIdx.x, currentTile.tileIdx.y, tileIdx.y + 0.5f)))
            CheckAttackRange(new Vector2(tileIdx.x, tileIdx.y + 1), cnt);

        if (!wallDics.ContainsKey(new Vector3(tileIdx.x, currentTile.tileIdx.y, tileIdx.y - 0.5f)))
            CheckAttackRange(new Vector2(tileIdx.x, tileIdx.y - 1), cnt);

        if (!wallDics.ContainsKey(new Vector3(tileIdx.x - 0.5f, currentTile.tileIdx.y, tileIdx.y)))
            CheckAttackRange(new Vector2(tileIdx.x - 1, tileIdx.y), cnt);

        if (!wallDics.ContainsKey(new Vector3(tileIdx.x + 0.5f, currentTile.tileIdx.y, tileIdx.y)))
            CheckAttackRange(new Vector2(tileIdx.x + 1, tileIdx.y), cnt);
    }

    private void ActionAttack(MonsterChar monster)
    {
        AttackMode();
        TurnOver();
    }
    
    private void TurnOver()
    {
        isTurnOver = true;
        ren.material.color = Color.gray;
    }
}
