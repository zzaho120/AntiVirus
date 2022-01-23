using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Wait,
    Move,
    Attack,
    Alert,
    TurnEnd
}

[RequireComponent(typeof(CharacterStats))]
public class PlayerableChar : BattleTile
{
    [Header("Character")]
    public CharacterStats characterStats;

    [Header("Value")]
    public int AP;
    private int sightDistance = 3;
    public int SightDistance
    {
        get
        {
            var buffList = characterStats.buffMgr.GetBuffList(Stat.Sight);
            float result = 0;
            foreach (var buff in buffList)
            {
                result = buff.GetAmount();
            }
            return sightDistance + (int)result;
        }
    }

    public int audibleDistance = 4;
    public CharacterState status;
    public bool isSelected;
    
    public DirectionType direction;

    private Dictionary<TileBase, int> moveDics =
        new Dictionary<TileBase, int>();

    [Header("Alert")]
    public List<MonsterChar> alertList;

    public override void Init()
    {
        base.Init();
        characterStats = GetComponent<CharacterStats>();
        characterStats.character = (Character)Instantiate(Resources.Load("Choi/Datas/Characters/Sniper"));
        characterStats.weapon.mainWeapon = (Weapon)Instantiate(Resources.Load("Choi/Datas/Weapons/AssaultRifle_01"));
        characterStats.weapon.subWeapon = (Weapon)Instantiate(Resources.Load("Choi/Datas/Weapons/FireAxe_01"));
        //¼öÁ¤
        characterStats.Init();  // --> characterStats.weapon.Init();
        direction = DirectionType.None;
    }

    public void Update()
    {
        if (status != CharacterState.TurnEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit))
                {
                    switch (status)
                    {
                        case CharacterState.Wait:
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


                                    CameraController.instance.SetFollowObject(transform);
                                }
                                else
                                    SetNonSelected();
                            }
                            break;
                        case CharacterState.Move:
                            if (hit.collider.tag == "Tile")
                            {
                                var tileBase = hit.collider.GetComponent<TileBase>();
                                if (moveDics.ContainsKey(tileBase))
                                    ActionMove(tileBase);
                            }
                            break;
                        case CharacterState.Attack:
                            if (hit.collider.tag == "BattleMonster")
                            {
                                var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.BattleInfo - 1).GetComponent<BattleInfoWindow>();
                                window.EnableMonsterInfo(true, hit.collider.GetComponent<MonsterChar>(), characterStats.weapon);
                            }
                            break;
                        case CharacterState.Alert:
                            break;
                    }
                }
            }
        }
    }

    public void StartTurn()
    {
        status = CharacterState.Wait;
        isSelected = false;
        AP = 6;
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
        var path = BattleMgr.Instance.pathMgr.pathList;
        while (path.Count > 0)
        {
            var aStarTile = path.Pop();

            if (aStarTile.tileBase.charObj != null && !aStarTile.tileBase.charObj.CompareTag("BattlePlayer"))
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
                transform.position = new Vector3(tile.tileIdx.x, tile.tileIdx.y + 0.5f, tile.tileIdx.z);
            }
        }
    }

    public void MoveMode()
    {
        if (status == CharacterState.Move)
            status = CharacterState.Wait;
        else
            status = CharacterState.Move;

        if (status == CharacterState.Move && isSelected)
        {
            FloodFillMove();
        }
        else
        {
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
        var moveAP = (int)((cnt + 1.5 - characterStats.weapon.MpPerAp) / 1.5f);
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
        BattleMgr.Instance.pathMgr.InitAStar(currentTile.tileIdx, tileBase.tileIdx);
        MoveMode();
        StartCoroutine(CoMove());
    }

    public void AttackMode()
    {
        status = CharacterState.Attack;
    }


    public void ActionAttack(MonsterChar monster)
    {
        var weapon = characterStats.weapon;

        if (weapon.CheckAvailBullet)
        {
            if (weapon.CheckAvailShot(AP, CharacterState.Attack))
            {
                var isHit = weapon.CheckAttackAccuracy(monster.currentTile.accuracy);
                AP -= weapon.GetWeaponAP();

                if (isHit)
                {
                    monster.GetDamage(weapon.Damage);
                    monster.SetTarget(this);
                }
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
        status = CharacterState.Alert;
    }

    public void EndPlayer()
    {
        if (status != CharacterState.Alert)
            status = CharacterState.TurnEnd;
        CameraController.instance.SetFollowObject(null);
        EventBusMgr.Publish(EventType.EndPlayer);
    }

    public void WaitPlayer()
    {
        status = CharacterState.Wait;
    }

    public bool GetDamage(MonsterStats monsterStats)
    {
        var hp = characterStats.currentHp;
        var dmg = monsterStats.Damage;
        hp -= dmg;
        characterStats.currentHp = Mathf.Clamp(hp, 0, hp);

        monsterStats.CalculateAttackAp();

        if (monsterStats.virus != null)
        {
            var virusType = string.Empty;
            switch (monsterStats.virus.id)
            {
                case "VIR_0001":
                    virusType = "E";
                    break;
                case "VIR_0002":
                    virusType = "B";
                    break;
                case "VIR_0003":
                    virusType = "P";
                    break;
                case "VIR_0004":
                    virusType = "I";
                    break;
                case "VIR_0005":
                    virusType = "T";
                    break;
            }
            characterStats.virusPanalty[virusType].Calculation(monsterStats.virusLevel, monsterStats.monster.virusGauge);
        }

        var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
        window.SetMsgText($"Player is damaged {dmg} Point - HP : {characterStats.currentHp}");

        if (characterStats.currentHp == 0)
        {
            EventBusMgr.Publish(EventType.DestroyChar, new object[] { this, 0 });
            return true;
        }

        return false;
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
        isSelected = false;
        status = CharacterState.Wait;
        CameraController.instance.SetFollowObject(null);
    }

    public void SetDirection(DirectionType direction)
    {
        this.direction = direction;

        switch (direction)
        {
            case DirectionType.None:
                break;
            case DirectionType.Top:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case DirectionType.Bot:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case DirectionType.Left:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case DirectionType.Right:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
        }
    }
}
