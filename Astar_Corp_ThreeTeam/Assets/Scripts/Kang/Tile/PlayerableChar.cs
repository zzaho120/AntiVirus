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
        characterStats.weapon.mainWeapon = (Weapon)Instantiate(Resources.Load("Choi/Datas/Weapons/AssaultRifle_01"));
        characterStats.weapon.subWeapon = (Weapon)Instantiate(Resources.Load("Choi/Datas/Weapons/FireAxe_01"));
        characterStats.Init();
        direction = DirectionType.None;
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
                                    var battleInfo = BattleMgr.Instance.BattleWindowMgr.Open(2, false).GetComponent<BattleInfoWindow>();
                                    battleInfo.EnablePlayerInfo(true, this);

                                    var playerAction = BattleMgr.Instance.BattleWindowMgr.Open(1, false).GetComponent<PlayerActionWindow>();
                                    playerAction.curChar = this;

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

    public int GetVirusLevel(MonsterChar monster)
    {
        var newTile = currentTile.tileIdx - monster.currentTile.tileIdx;
        var virusTile = new Vector2(newTile.x, newTile.z);
        var monsterStats = monster.monsterStats;
        
        var resultLevel = monsterStats.virusLevel - (int)(Mathf.Abs(virusTile.x) + Mathf.Abs(virusTile.y));
        Debug.Log(resultLevel);

        return resultLevel;
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
            BattleMgr.Instance.sightMgr.UpdateFog(this);
            yield return new WaitForSeconds(0.1f);
        }
        var window = BattleMgr.Instance.BattleWindowMgr.Open(1).GetComponent<PlayerActionWindow>();
        window.OnActiveDirectionBtns(false, true);
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

        Debug.Log(weapon.MainWeaponBullet);
        if (weapon.MainWeaponBullet > 0)
        {
            var tileAccuracy = monster.currentTile.accuracy;
            var totalAccuracy = 0;
            switch (characterStats.weapon.Range)
            {
                case 1: // 근거리
                    if (0 < tileAccuracy && tileAccuracy < 5)
                        totalAccuracy = weapon.AccurRate_base;
                    else
                        totalAccuracy = weapon.AccurRate_base - 30 * (tileAccuracy - 4);
                    break;

                case 2: // 중거리
                    if (3 < tileAccuracy && tileAccuracy < 8)
                        totalAccuracy = weapon.AccurRate_base;
                    else if (tileAccuracy < 4)
                        totalAccuracy = weapon.AccurRate_base - 10 * (4 - tileAccuracy);
                    else if (tileAccuracy > 7)
                        totalAccuracy = weapon.AccurRate_base - 15 * (tileAccuracy - 8);
                    break;

                case 3: // 원거리
                    if (6 < tileAccuracy && tileAccuracy < 11)
                        totalAccuracy = weapon.AccurRate_base;
                    else if (tileAccuracy < 7)
                        totalAccuracy = weapon.AccurRate_base - 15 * (7 - tileAccuracy);
                    else if (tileAccuracy > 10)
                        totalAccuracy = weapon.AccurRate_base - 10 * (tileAccuracy - 10);
                    break;

                case 4: // 근접무기
                    if (1 == tileAccuracy)
                        totalAccuracy = weapon.AccurRate_base;
                    else
                        totalAccuracy = 0;
                    break;
            }

            totalAccuracy = Mathf.Clamp(totalAccuracy, 0, 100);

            var randomAccuracy = Random.Range(0, 100) < totalAccuracy;

            Debug.Log(randomAccuracy);
            if (randomAccuracy)
                monster.GetDamage(weapon.Damage);

            weapon.MainWeaponBullet--;
            monster.currentTile.EnableDisplay(true);

            if (!isFirstAtk)
            {
                isFirstAtk = true;
                AP -= weapon.FirstShotAp;
            }
            else
                AP -= weapon.AimShotAp;
            EndPlayer();
        }
    }

    public void AlertMode()
    {
        status = PlayerStatus.Alert;

        var weapon = characterStats.weapon;
        AP -= weapon.FirstShotAp;
        actionAP = AP;
        AP = 0;
    }

    public void EndPlayer()
    {
        status = PlayerStatus.TurnEnd;
        ren.material.color = Color.gray;
        EventBusMgr.Publish(EventType.EndPlayer);
    }

    public void GetDamage(MonsterStats monsterStats)
    {
        var hp = characterStats.currentHp;
        var dmg = monsterStats.Damage;
        hp -= dmg;
        characterStats.currentHp = Mathf.Clamp(hp, 0, hp);

        if (monsterStats.virus != null)
        {
            var virusType = string.Empty;
            switch (monsterStats.virus.id)
            {
                case "1":
                    virusType = "E";
                    break;
                case "2":
                    virusType = "B";
                    break;
                case "3":
                    virusType = "P";
                    break;
                case "4":
                    virusType = "I";
                    break;
                case "5":
                    virusType = "T";
                    break;
            }
            characterStats.virusPanalty[virusType].Calculation(monsterStats.virusLevel);
        }


        Debug.Log($"{characterStats.gameObject.name}은 {dmg} 데미지를 입어 {characterStats.currentHp}가 되었다.");

        if (characterStats.currentHp == 0)
            Destroy(gameObject);
    }
}
