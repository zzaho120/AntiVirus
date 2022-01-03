using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerableChar : BattleChar
{
    [Header("Character")]
    public CharacterStats characterStats;

    [Header("Value")]
    public int AP;
    public int moveDistance;
    public int sightDistance;
    public bool isMove;
    public bool isAttack;
    public bool isSelected;
    public bool isTurnOver;
    public DirectionType direction;

    private Dictionary<TileBase, int> moveDics =
        new Dictionary<TileBase, int>();

    private MeshRenderer ren;

    public override void Init()
    {
        base.Init();
        ren = GetComponent<MeshRenderer>();
        characterStats.character = (Character)Instantiate(Resources.Load("Choi/Datas/Characters/Sniper"));
        characterStats.Init();
        direction = DirectionType.Top;
        AP = 6;
        characterStats.Init();
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
                        if (BattleMgr.Instance.sightMgr.GetFrontSight(this).Exists(x => x.tileBase == monster.currentTile))
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
        AP = 6;
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
            BattleMgr.Instance.sightMgr.UpdateFog();
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
        if (cnt >= AP)
            return;

        if (cnt % 3 == 0)
        {
            tile.moveAP = ((cnt + 3) / 3);
            Debug.Log(tile.moveAP);
        }

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

    public void AttackMode()
    {
        isAttack = true;
    }

    private void ActionMove(TileBase tileBase)
    {
        BattleMgr.Instance.sightMgr.UpdateFog();
        BattleMgr.Instance.aStar.InitAStar(currentTile.tileIdx, tileBase.tileIdx);
        MoveMode();
        StartCoroutine(CoMove());
    }

    private void ActionAttack(MonsterChar monster)
    {
        TurnOver();

        var tileAccuracy = monster.currentTile.accuracy; ;
        var totalAccuracy = 0;
        var weapon = characterStats.weapon;
        switch (characterStats.weapon.range)
        {
            case 1: // 근거리
                if (0 < tileAccuracy && tileAccuracy < 5)
                    totalAccuracy = weapon.accurRate_base;
                else
                    totalAccuracy = weapon.accurRate_base - 30 * (tileAccuracy - 4);
                break;

            case 2: // 중거리
                if (3 < tileAccuracy && tileAccuracy < 8)
                    totalAccuracy = weapon.accurRate_base;
                else if (tileAccuracy < 4)
                    totalAccuracy = weapon.accurRate_base - 10 * (4 - tileAccuracy);
                else if (tileAccuracy > 7)
                    totalAccuracy = weapon.accurRate_base - 15 * (tileAccuracy - 8);
                break;

            case 3: // 원거리
                if (6 < tileAccuracy && tileAccuracy < 11)
                    totalAccuracy = weapon.accurRate_base;
                else if (tileAccuracy < 7)
                    totalAccuracy = weapon.accurRate_base - 15 * (7 - tileAccuracy);
                else if (tileAccuracy > 10)
                    totalAccuracy = weapon.accurRate_base - 10 * (tileAccuracy - 10);
                break;

            case 4: // 근접무기
                if (1 == tileAccuracy)
                    totalAccuracy = weapon.accurRate_base;
                else
                    totalAccuracy = 0;
                break;
        }

        totalAccuracy = Mathf.Clamp(totalAccuracy, 0, 100);

        var randomAccuracy = Random.Range(0, 100) < totalAccuracy;

        if (randomAccuracy)
            monster.GetDamage(weapon.damage);
        Debug.Log(totalAccuracy);
        Debug.Log(randomAccuracy);

        monster.currentTile.EnableDisplay(true);
    }

    private void ActionBoundary()
    {

    }

    private void TurnOver()
    {
        isTurnOver = true;
        ren.material.color = Color.gray;
    }
}
