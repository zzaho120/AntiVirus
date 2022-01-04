using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatus
{
    Wait,
    Move,
    Attack,
    Alert,
    TurnEnd
}

public class PlayerableChar : BattleChar
{
    [Header("Character")]
    public CharacterStats characterStats;

    [Header("Value")]
    public int AP;
    public int actionAP;
    public int sightDistance;
    public PlayerStatus status;
    public bool isSelected;
    public bool isFirstAtk;

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
        characterStats.Init();
        AP = 6;
        status = PlayerStatus.Wait;
    }

    public void Update()
    {
        if (status != PlayerStatus.TurnEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit))
                {
                    switch (status)
                    {
                        case PlayerStatus.Wait:
                            if (hit.collider.gameObject == gameObject)
                            {
                                isSelected = !isSelected;
                                if (isSelected)
                                {
                                    var playerAction = BattleMgr.Instance.BattleWindowMgr.Open(1, false).GetComponent<PlayerActionWindow>();
                                    playerAction.curChar = this;

                                    var battleInfo = BattleMgr.Instance.BattleWindowMgr.Open(2, false).GetComponent<BattleInfoWindow>();
                                    battleInfo.EnablePlayerInfo(true, this);
                                    ren.material.color = Color.red;
                                }
                                else
                                {
                                    var window = BattleMgr.Instance.BattleWindowMgr.GetWindow(1);
                                    var playerAction = window.GetComponent<PlayerActionWindow>();
                                    playerAction.curChar = null;
                                    window.Close();
                                    ren.material.color = Color.white;
                                }
                            }
                            break;
                        case PlayerStatus.Move:
                            if (hit.collider.tag == "Tile")
                            {
                                var tileBase = hit.collider.GetComponent<TileBase>();
                                if (moveDics.ContainsKey(tileBase))
                                    ActionMove(tileBase);
                            }
                            break;
                        case PlayerStatus.Attack:
                            if (hit.collider.tag == "BattleMonster")
                            {
                                Debug.Log(hit.collider.name);
                                var monster = hit.collider.GetComponent<MonsterChar>();
                                var tileIdx = new Vector2(monster.tileIdx.x, monster.tileIdx.z);
                                if (BattleMgr.Instance.sightMgr.GetFrontSight(this).Exists(x => x.tileBase == monster.currentTile))
                                    ActionAttack(monster);
                            }
                            break;
                        case PlayerStatus.Alert:
                            break;
                    }
                }
            }
        }
    }

    public void StartTurn()
    {
        status = PlayerStatus.Wait;
        isSelected = false;
        isFirstAtk = false;

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
        EndPlayer();
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
        if (status == PlayerStatus.Move)
            status = PlayerStatus.Wait;
        else
            status = PlayerStatus.Move;

        if (status == PlayerStatus.Move && isSelected)
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

    private void ActionMove(TileBase tileBase)
    {
        BattleMgr.Instance.sightMgr.UpdateFog();
        BattleMgr.Instance.aStar.InitAStar(currentTile.tileIdx, tileBase.tileIdx);
        MoveMode();
        StartCoroutine(CoMove());
    }

    public void AttackMode()
    {
        status = PlayerStatus.Attack;
    }


    private void ActionAttack(MonsterChar monster)
    {
        var weapon = characterStats.weapon;

        Debug.Log(weapon.mainWeaponBullet);
        if (weapon.mainWeaponBullet > 0)
        {
            var tileAccuracy = monster.currentTile.accuracy;
            var totalAccuracy = 0;
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

            Debug.Log(randomAccuracy);
            if (randomAccuracy)
                monster.GetDamage(weapon.damage);

            weapon.mainWeaponBullet--;
            monster.currentTile.EnableDisplay(true);

            if (!isFirstAtk)
            {
                isFirstAtk = true;
                AP -= weapon.firstShotAp;
            }
            else
                AP -= weapon.aimShotAp;
            EndPlayer();
        }
    }

    public void AlertMode()
    {
        status = PlayerStatus.Alert;

        var weapon = characterStats.weapon;
        AP -= weapon.firstShotAp;
        actionAP = AP;
        AP = 0;
    }

    public void EndPlayer()
    {
        status = PlayerStatus.TurnEnd;
        ren.material.color = Color.gray;
        EventBusMgr.Publish(EventType.EndPlayer);
    }
}
