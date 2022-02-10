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

public class PlayerableChar : BattleTile
{
    [Header("Character")]
    public CharacterStats characterStats;
    public Animator animator;

    [Header("Value")]
    public int AP;
    public int attackCount;
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
            return characterStats.sightDistance + (int)result;
        }
    }

    public int audibleDistance = 4;
    public CharacterState status;
    public bool isMoved;
    public bool isTankB1Skill;
    public bool isHMGA1Skill;

    public DirectionType direction;

    private Dictionary<TileBase, int> moveDics =
        new Dictionary<TileBase, int>();
    private List<MoveTile> moveList = new List<MoveTile>();

    [Header("Alert")]
    public List<MonsterChar> alertList;

    public override void Init()
    {
        base.Init();
        characterStats = new CharacterStats();
        characterStats.character = (Character)Instantiate(Resources.Load("Choi/Datas/Characters/Sniper"));
        characterStats.weapon = new WeaponStats();
        characterStats.weapon.mainWeapon = (Weapon)Instantiate(ScriptableMgr.Instance.GetEquippable("WEP_0013"));
        characterStats.weapon.subWeapon = (Weapon)Instantiate(ScriptableMgr.Instance.GetEquippable("WEP_0023"));
        
        characterStats.Init();  // --> characterStats.weapon.Init();
        direction = DirectionType.None;
        characterStats.StartGame();

        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (status != CharacterState.TurnEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (BattleMgr.Instance.playerMgr.selectChar == null || BattleMgr.Instance.playerMgr.selectChar == this)
                {
                    RaycastHit hit;
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        switch (status)
                        {
                            case CharacterState.Wait:
                                break;
                            case CharacterState.Move:
                                if (hit.collider.tag == "MoveTile")
                                {
                                    var tileBase = hit.collider.GetComponent<MoveTile>().parent;
                                    if (moveDics.ContainsKey(tileBase))
                                    {
                                        if (tileBase.charObj == null)
                                        {
                                            ActionMove(tileBase);
                                            ReturnMoveTile();
                                            var window = BattleMgr.Instance.battleWindowMgr.Open(0) as BattleBasicWindow;
                                            window.cancelBtn.SetActive(false);
                                            window.moveBtn.SetActive(true);
                                        }
                                    }
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
    }

    public void StartTurn()
    {
        status = CharacterState.Wait;
        isMoved = false;
        isTankB1Skill = false;
        isHMGA1Skill = false;
        AP = 6; 
        attackCount = 0;
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
            if (path.Count == 2 || path.Count == 1)
            {
                if (!animator.GetBool("Run Stop"))
                {
                    animator.SetBool("Run Stop", true);
                    animator.SetBool("Run", false);
                }
            }

            var aStarTile = path.Pop();

            
            if (aStarTile.tileBase.charObj != null && !aStarTile.tileBase.charObj.CompareTag("BattlePlayer"))
            {
                animator.SetBool("Run Stop", true);
                animator.SetBool("Run", false);
                yield return null;
                break;
            }

            yield return MoveTile(aStarTile.tileBase.tileIdx);
            BattleMgr.Instance.sightMgr.UpdateFog(this);
        }
        AP -= currentTile.moveAP;
        var window = BattleMgr.Instance.battleWindowMgr.Open(0) as BattleBasicWindow;
        window.UpdateUI();
        window.OnClickDirection();
        animator.SetBool("Run Stop", false);
        animator.SetBool("Idle", true);
        if (currentTile.moveAP == 6)
        {
            var skillList = characterStats.skillMgr.GetPassiveSkills(PassiveCase.FullApMove);
            foreach (var skill in skillList)
            {
                skill.Invoke(characterStats.buffMgr);
            }
        }
    }

    private IEnumerator MoveTile(Vector3 nextIdx)
    {
        foreach (var tile in currentTile.adjNodes)
        {
            if (tile.tileIdx.x == nextIdx.x && tile.tileIdx.z == nextIdx.z)
            {
                var dir = (currentTile.tileIdx - nextIdx).normalized;

                var rotY = 0;
                if (dir.x > 0)
                    rotY = 270;
                else if (dir.x < 0)
                    rotY = 90;
                else if (dir.z > 0)
                    rotY = 180;
                else if (dir.z < 0)
                    rotY = 0;
                transform.rotation = Quaternion.Euler(0f, rotY, 0f);

                currentTile.charObj = null;
                tileIdx = nextIdx;
                currentTile = tile;
                currentTile.charObj = gameObject;
                if (!animator.GetBool("Run") && !animator.GetBool("Run Stop"))
                    animator.SetBool("Run", true);
                yield return StartCoroutine(CoMoveChar(nextIdx + new Vector3(0f, 0.5f, 0f)));
            }
        }
    }

    private IEnumerator CoMoveChar(Vector3 nextIdx)
    {
        var origin = transform.position;
        var timer = 0f;
        while (timer < 1)
        {
            timer += Time.deltaTime * 5;
            transform.position = Vector3.Lerp(origin, nextIdx, timer);
            yield return null;
        }
    }

    public void MoveMode()
    {
        if (status == CharacterState.Wait)
            status = CharacterState.Move;


        if (status == CharacterState.Move)
        {
            FloodFillMove();
        }
        else
            ReturnMoveTile();
    }

    private void FloodFillMove()
    {
        moveDics.Clear();
        moveDics.Add(currentTile, 0);

        var cnt = 0; 
        var mpPerAp = characterStats.weapon.MpPerAp - characterStats.movePoint;
        var buffMgr = characterStats.buffMgr;

        var mpList = buffMgr.GetBuffList(Stat.Mp);
        foreach (var buff in mpList)
        {
            mpPerAp += (int)buff.GetAmount();
        }

        var maxMpList = buffMgr.GetBuffList(Stat.MaxMPLimit);
        var isBuff = maxMpList.Count > 0;
        foreach (var adjNode in currentTile.adjNodes)
        {
            CheckMoveRange(adjNode, cnt, mpPerAp, isBuff);
        }
    }

    private void CheckMoveRange(TileBase tile, int cnt, int mpPerAp, bool isBuff)
    {
        if (mpPerAp == 0)
            return;

        var totalMp = AP * mpPerAp;
        var moveAP = (cnt + mpPerAp) / mpPerAp;
        if (moveAP > totalMp || moveAP > AP)
            return;

        if (isBuff)
        {
            if (tile.movePoint >= 2)
                cnt += 2;
            else
                cnt += tile.movePoint;
        }
        else
            cnt += tile.movePoint;

        if (!moveDics.ContainsKey(tile))
        {
            moveDics.Add(tile, moveAP);
            tile.moveAP = moveAP;
        }
        else if (moveDics[tile] > moveAP)
        {
            moveDics[tile] = moveAP;
            tile.moveAP = moveAP;
        }
        else
            return;

        if (tile.charObj == null)
        {
            var go = BattleMgr.Instance.battlePoolMgr.CreateMoveTile();
            go.transform.position = tile.tileIdx + new Vector3(0, 0.5f);
            var moveTile = go.GetComponent<MoveTile>();
            moveTile.parent = tile;
            moveList.Add(moveTile);
        }

        foreach (var adjNode in tile.adjNodes)
        {
            CheckMoveRange(adjNode, cnt, mpPerAp, isBuff);
        }
    }

    private void ActionMove(TileBase tileBase)
    {
        isMoved = true;
        BattleMgr.Instance.pathMgr.InitAStar(currentTile.tileIdx, tileBase.tileIdx);
        MoveMode();
        StartCoroutine(CoMove());

        Debug.Log(characterStats.Weight);
    }

    public void AttackMode()
    {
        status = CharacterState.Attack;
    }


    public void ActionAttack(MonsterChar monster)
    {
        var weapon = characterStats.weapon;

        var fullAPMoveList = characterStats.buffMgr.GetBuffList(Stat.FullApMove);
        var isFullApMove = fullAPMoveList.Count > 0;
        var repeat = 1;
        if (isHMGA1Skill)
            repeat = AP / weapon.GetWeaponAP();

        for (var idx = 0; idx < repeat; ++idx)
        {
            if (weapon.CheckAvailBullet)
            {
                if (weapon.CheckAvailShot(AP, CharacterState.Attack) || isFullApMove)
                {
                    var buffValue = 0;
                    if (weapon.curWeapon.kind == "6")
                    {
                        var moveSRList = characterStats.buffMgr.GetBuffList(Stat.MoveSRAccuracy);
                        foreach (var buff in moveSRList)
                        {
                            buffValue += (int)buff.GetAmount();
                        }
                    }

                    var isHit = Random.Range(0, 100) < weapon.GetAttackAccuracy(monster.currentTile.accuracy) + characterStats.accuracy + buffValue;

                    if (!isFullApMove)
                        AP -= weapon.GetWeaponAP();

                    if (monster.IsNullTarget)
                        monster.SetTarget(this);

                    weapon.fireCount++;
                    weapon.WeaponBullet--;

                    var time = 0f;
                    if (isHit)
                    {
                        var isCrit = Random.Range(0, 100) < weapon.CritRate + characterStats.critRate - monster.monsterStats.critResist;
                        time = monster.GetDamage(this, isCrit);

                        var buffMgr = characterStats.buffMgr;
                        var skillList = characterStats.skillMgr.GetPassiveSkills(PassiveCase.Hit);
                        foreach (var skill in skillList)
                        {
                            skill.Invoke(buffMgr);
                        }

                        var buffList = buffMgr.GetBuffList(Stat.Aggro);
                        if (buffList.Count > 0)
                            monster.SetTarget(this);
                    }
                    else
                    {
                        // 빗맞음
                    }

                    characterStats.buffMgr.RemoveBuff(fullAPMoveList);
                    monster.currentTile.EnableDisplay(true);

                    if (AP <= 0 && time != 0)
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

        if (isHMGA1Skill)
        {
            AP = 0;
            EndPlayer();
            isHMGA1Skill = false;
        }
    }

    public void AlertMode()
    {
        status = CharacterState.Alert;
        attackCount = AP / characterStats.weapon.curWeapon.firstShotAp;
        AP = 0;
    }

    public void EndPlayer()
    {
        if (status != CharacterState.Alert)
            status = CharacterState.TurnEnd;

        BattleMgr.Instance.playerMgr.selectChar = null;
        EventBusMgr.Publish(EventType.EndPlayer);
        CameraController.instance.SetFollowObject(null);

        if (!isMoved)
        {
            var skillList = characterStats.skillMgr.GetPassiveSkills(PassiveCase.isMoved);
            var buffMgr = characterStats.buffMgr;
            foreach (var skill in skillList)
            {
                skill.Invoke(buffMgr);
            }
        }
    }

    public void WaitPlayer()
    {
        BattleMgr.Instance.playerMgr.selectChar = null;
        status = CharacterState.Wait;
    }

    public bool GetDamage(MonsterStats monsterStats, bool isCrit)
    {
        var hp = characterStats.currentHp;
        var dmg = 0;

        if (!isCrit)
            dmg = monsterStats.Damage;
        else
            dmg = (int)(monsterStats.Damage * (monsterStats.CritDmg / 100f));

        hp -= dmg;
        characterStats.currentHp = Mathf.Clamp(hp, 0, hp);
        var window = BattleMgr.Instance.battleWindowMgr.Open(0) as BattleBasicWindow;
        window.UpdateUI();


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
            var buffList = characterStats.buffMgr.GetBuffList(Stat.Virus);
            float result = monsterStats.monster.virusGauge;
            foreach (var buff in buffList)
            {
                result *= buff.GetAmount();
            }

            characterStats.virusPenalty[virusType].Calculation(monsterStats.virusLevel, result);
        }

        // 맞음 메세지

        if (characterStats.currentHp == 0)
        {
            EventBusMgr.Publish(EventType.DestroyChar, new object[] { this, 0 });
            return true;
        }

        return false;
    }

    public bool GetDamage(int dmg)
    {
        var hp = characterStats.currentHp;
        hp -= dmg;
        characterStats.currentHp = Mathf.Clamp(hp, 0, hp);
        var window = BattleMgr.Instance.battleWindowMgr.Open(0) as BattleBasicWindow;
        window.UpdateUI();
        window.UpdateExtraInfo(this);
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
            // Ap부족
        }
    }

    public void SetNonSelected()
    {
        BattleMgr.Instance.playerMgr.selectChar = null;
        isHMGA1Skill = false;
        isTankB1Skill = false;
        status = CharacterState.Wait;
    }

    public void SetDirection(DirectionType direction)
    {
        this.direction = direction;
        var nextRot = Quaternion.identity; 
        switch (direction)
        {
            case DirectionType.None:
                break;
            case DirectionType.Top:
                nextRot = Quaternion.Euler(0, 0, 0);
                break;
            case DirectionType.Bot:
                nextRot = Quaternion.Euler(0, 180, 0);
                break;
            case DirectionType.Left:
                nextRot = Quaternion.Euler(0, 270, 0);
                break;
            case DirectionType.Right:
                nextRot = Quaternion.Euler(0, 90, 0);
                break;
        }
        StartCoroutine(CoRotateChar(nextRot));
    }

    private IEnumerator CoRotateChar(Quaternion nextRot)
    {
        var timer = 0f;
        var origin = transform.rotation;
        var angle = Mathf.Abs(transform.rotation.eulerAngles.y) - nextRot.eulerAngles.y;
        Debug.Log(angle);
        animator.SetBool("Idle", false);
        if (angle == -90)
            animator.SetBool("Turn Right", true);
        else if (angle == 90)
            animator.SetBool("Turn Left", true);

        while (timer < 1)
        {
            timer += Time.deltaTime * 3;
            transform.rotation = Quaternion.Lerp(origin, nextRot, timer);
            yield return null;
        }

        animator.SetBool("Idle", true);
        if (angle == -90)
            animator.SetBool("Turn Right", false);
        else if (angle == 90)
            animator.SetBool("Turn Left", false);
    }

    public void ReturnMoveTile()
    {
        foreach (var moveTile in moveList)
        {
            var returnToPool = moveTile.GetComponent<ReturnToPool>();
            returnToPool.Return();
        }
        moveList.Clear();
    }

    public void HMG_A1_Skill()
    {
        isHMGA1Skill = true;
        status = CharacterState.Attack;
    }
}
