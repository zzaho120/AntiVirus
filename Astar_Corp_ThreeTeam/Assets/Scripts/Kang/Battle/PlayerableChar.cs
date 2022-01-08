using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
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
    public int sightDistance;
    public PlayerState status;
    public bool isSelected;

    public DirectionType direction;

    private Dictionary<TileBase, int> moveDics =
        new Dictionary<TileBase, int>();

    private MeshRenderer ren;

    [Header("Alert")]
    public List<MonsterChar> alertList;

    public override void Init()
    {
        base.Init();
        ren = GetComponent<MeshRenderer>();
        //characterStats.character = (Character)Instantiate(Resources.Load("Choi/Datas/Characters/Sniper"));
        characterStats.weapon.mainWeapon = (Weapon)Instantiate(Resources.Load("Choi/Datas/Weapons/AssaultRifle_01"));
        characterStats.weapon.subWeapon = (Weapon)Instantiate(Resources.Load("Choi/Datas/Weapons/FireAxe_01"));
        //¼öÁ¤
        characterStats.Init();  // --> characterStats.weapon.Init();
        direction = DirectionType.None;
        StartTurn();
    }

    public void Update()
    {
        if (status != PlayerState.TurnEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit))
                {
                    switch (status)
                    {
                        case PlayerState.Wait:
                            if (hit.collider.gameObject == gameObject)
                            {
                                isSelected = !isSelected;
                                if (isSelected)
                                {
                                    var battleInfo = BattleMgr.Instance.battleWindowMgr.Open(2, false).GetComponent<BattleInfoWindow>();
                                    battleInfo.EnablePlayerInfo(true, this);

                                    var playerAction = BattleMgr.Instance.battleWindowMgr.Open(1, false).GetComponent<PlayerActionWindow>();
                                    playerAction.curChar = this;
                                    playerAction.EnableReloadBtn();

                                    ren.material.color = Color.green;
                                }
                                else
                                    SetNonSelected();
                            }
                            break;
                        case PlayerState.Move:
                            if (hit.collider.tag == "Tile")
                            {
                                var tileBase = hit.collider.GetComponent<TileBase>();
                                if (moveDics.ContainsKey(tileBase))
                                    ActionMove(tileBase);
                            }
                            break;
                        case PlayerState.Attack:
                            if (hit.collider.tag == "BattleMonster")
                            {
                                var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.BattleInfo - 1).GetComponent<BattleInfoWindow>();
                                window.EnableMonsterInfo(true, hit.collider.GetComponent<MonsterChar>(), characterStats.weapon);
                            }
                            break;
                        case PlayerState.Alert:
                            break;
                    }
                }
            }
        }
    }

    public void StartTurn()
    {
        status = PlayerState.Wait;
        isSelected = false;
        AP = 6;
        ren.material.color = Color.white;
        alertList.Clear();
        characterStats.StartTurn();
    }

    public int GetVirusLevel(MonsterChar monster)
    {
        var newTile = currentTile.tileIdx - monster.currentTile.tileIdx;
        var virusTile = new Vector2(newTile.x, newTile.z);
        var monsterStats = monster.monsterStats;
        
        var resultLevel = monsterStats.virusLevel - (int)(Mathf.Abs(virusTile.x) + Mathf.Abs(virusTile.y));
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
        AP -= currentTile.moveAP;

        var window = BattleMgr.Instance.battleWindowMgr.Open(1).GetComponent<PlayerActionWindow>();
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
        if (status == PlayerState.Move)
            status = PlayerState.Wait;
        else
            status = PlayerState.Move;

        if (status == PlayerState.Move && isSelected)
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
        var moveAP = (int)((cnt + 1.5 - characterStats.weapon.Weight) / 1.5f);
        if (moveAP > AP)
            return;

        cnt++;
        if (!moveDics.ContainsKey(tile))
        {
            moveDics.Add(tile, cnt);
            tile.moveAP = moveAP;
        }
        else if (moveDics[tile] > cnt)
        {
            moveDics[tile] = cnt;
            tile.moveAP = moveAP;
        }
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
        status = PlayerState.Attack;
    }


    public void ActionAttack(MonsterChar monster)
    {
        var weapon = characterStats.weapon;

        if (weapon.CheckAvailBullet)
        {
            if (weapon.CheckAvailShot(AP, PlayerState.Attack))
            {
                var isHit = weapon.CheckAttackAccuracy(monster.currentTile.accuracy);
                AP -= weapon.GetWeaponAP(PlayerState.Attack);

                if (isHit)
                    monster.GetDamage(weapon.Damage);
                else
                {
                    var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
                    window.SetMsgText($"You missed {monster.name}");
                }

                monster.currentTile.EnableDisplay(true);

                if (AP <= 0)
                    EndPlayer();
                else
                {
                    WaitPlayer();
                    SetNonSelected();
                }

            }
            else
            {
                var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
                window.SetMsgText($"Not enough Action Point for Attack");
                SetNonSelected();
            }
        }
        else
        {
            var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
            window.SetMsgText($"Not enough Bullet for Attack");
            SetNonSelected();
        }
        
    }

    public void AlertMode()
    {
        status = PlayerState.Alert;
    }

    public void EndPlayer()
    {
        if (status != PlayerState.Alert)
            status = PlayerState.TurnEnd;
        ren.material.color = Color.gray;
        EventBusMgr.Publish(EventType.EndPlayer);
    }

    public void WaitPlayer()
    {
        status = PlayerState.Wait;
        ren.material.color = Color.white;
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

        var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
        window.SetMsgText($"Player is damaged {dmg} Point - HP : {characterStats.currentHp}");

        if (characterStats.currentHp == 0)
            EventBusMgr.Publish(EventType.DestroyChar, new object[] { this, 0 });
    }

    public void ReloadWeapon()
    {
        var weapon = characterStats.weapon;

        if (weapon.CheckReloadAP(AP))
        {
            AP -= weapon.Reload();
        }
        else
        {
            var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
            window.SetMsgText($"Not Enough Action Point");
        }
    }

    public void SetNonSelected()
    {
        var window = BattleMgr.Instance.battleWindowMgr.GetWindow(1);
        var playerAction = window.GetComponent<PlayerActionWindow>();
        playerAction.curChar = null;
        window.Close();
        window = BattleMgr.Instance.battleWindowMgr.GetWindow(2);
        window.Close();
        ren.material.color = Color.white;
        isSelected = false;
        status = PlayerState.Wait;
    }
}
