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
    public Vector3 weaponRot = new Vector3(-20.986f, -281.047f, -89.688f);
    public Vector3 fireRot = new Vector3(16.112f, 54.533f, -82.484f);
    public GameObject mainWeapon;
    public GameObject subWeapon;
    public GameObject currentWeapon;

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

    public int audibleDistance = 8;
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

    [Header("Costume")]
    public List<GameObject> tankerCos;
    public List<GameObject> healerCos;
    public List<GameObject> scoutCos;
    public List<GameObject> bombardierCos;
    public List<GameObject> sniperCos;

    public Transform rightHandTr;
    private MonsterChar targetMonster;

    public List<GameObject> sightTileList;

    private bool isOnSightTile;

    public override void Init()
    {
        base.Init();
        characterStats.Init();
        direction = DirectionType.None;

        animator = GetComponent<Animator>();

        switch (characterStats.character.type)
        {
            case "Tanker":
                foreach (var cos in tankerCos)
                    cos.SetActive(true);
                characterStats.skillMgr.passiveSkills.Add(ScriptableMgr.Instance.GetPassiveSkill("PRO_0001"));
                break;
            case "Healer":
                foreach (var cos in healerCos)
                    cos.SetActive(true);
                characterStats.skillMgr.passiveSkills.Add(ScriptableMgr.Instance.GetPassiveSkill("MED_0002"));
                break;
            case "Scout":
                foreach (var cos in scoutCos)
                    cos.SetActive(true);
                characterStats.skillMgr.passiveSkills.Add(ScriptableMgr.Instance.GetPassiveSkill("SER_0001"));
                break;
            case "Bombardier":
                foreach (var cos in bombardierCos)
                    cos.SetActive(true);
                characterStats.skillMgr.activeSkills.Add(ScriptableMgr.Instance.GetActiveSkill("HMG_0001"));
                break;
            case "Sniper":
                foreach (var cos in sniperCos)
                    cos.SetActive(true);
                characterStats.skillMgr.passiveSkills.Add(ScriptableMgr.Instance.GetPassiveSkill("SNP_0003"));
                break;
        }

        characterStats.StartGame();
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
                                            ReturnSightTile();
                                            var window = BattleMgr.Instance.battleWindowMgr.Open(0) as BattleBasicWindow;
                                            window.cancelBtn.SetActive(false);
                                            window.moveBtn.SetActive(true);
                                        }
                                    }
                                }
                                break;
                            case CharacterState.Attack:
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
                    rotY = 270 + 37;
                else if (dir.x < 0)
                    rotY = 90 + 37;
                else if (dir.z > 0)
                    rotY = 180 + 37;
                else if (dir.z < 0)
                    rotY = 0 + 37;
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
        var poolMgr = BattleMgr.Instance.battlePoolMgr;

        var origin = transform.position;
        var timer = 0f;
        while (timer < 1)
        {
            timer += Time.deltaTime * 5;
            transform.position = Vector3.Lerp(origin, nextIdx, timer);

            yield return null;
        }

        var footStep = poolMgr.CreateFootStep();
        footStep.transform.position = transform.position;
        StartCoroutine(CoReturnParticle(footStep));
    }

    private IEnumerator CoReturnParticle(GameObject particle)
    {
        var particleSys = particle.GetComponent<ParticleSystem>();
        particleSys.Play();

        while (true)
        {
            if (particleSys.isStopped)
            {
                var returnToPool = particle.GetComponent<ReturnToPool>();
                returnToPool.Return();
                break;
            }
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
        ReturnSightTile();

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
            go.transform.position = tile.tileIdx + new Vector3(0, 0.55f);
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
    }

    public void AttackMode()
    {
        status = CharacterState.Attack;
    }

    public void PlayAttackAnim(MonsterChar monster)
    {
        var repeat = 1;
        if (isHMGA1Skill)
            repeat = characterStats.weapon.WeaponBullet;

        StartCoroutine(CoAttack(repeat, monster));
    }

    private Quaternion originRot;
    public void ActionAttack(MonsterChar monster)
    {
        var weapon = characterStats.weapon;
        var fullAPMoveList = characterStats.buffMgr.GetBuffList(Stat.FullApMove);
        var isFullApMove = fullAPMoveList.Count > 0;
        currentWeapon.transform.localRotation = Quaternion.Euler(weaponRot);

        if (weapon.CheckAvailBullet)
        {
            if (weapon.CheckAvailShot(AP, CharacterState.Attack) || isFullApMove || isHMGA1Skill)
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

                if (!isFullApMove || !isHMGA1Skill)
                    AP -= weapon.GetWeaponAP();

                var window = BattleMgr.Instance.battleWindowMgr.Open(0) as BattleBasicWindow;
                window.UpdateUI();

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
                    var go = BattleMgr.Instance.battlePoolMgr.CreateScrollingText();
                    var scrollingText = go.GetComponent<ScrollingText>();
                    go.transform.position = monster.transform.position;
                    scrollingText.SetMiss();
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
        }
    }

    public IEnumerator CoAttack(int repeat, MonsterChar monster)
    {
        targetMonster = monster;
        for (var idx = 0; idx < repeat; ++idx)
        {
            if (targetMonster.monsterStats.currentHp <= 0)
                yield break;
            animator.SetTrigger("Fire");
            currentWeapon.transform.localRotation = Quaternion.Euler(fireRot);

            yield return new WaitForSeconds(1.5f);
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

        animator.SetTrigger("Damaged"); 
        var go = BattleMgr.Instance.battlePoolMgr.CreateScrollingText();
        var scrollingText = go.GetComponent<ScrollingText>();
        go.transform.position = transform.position;
        scrollingText.SetDamage(dmg, false);

        var blood = BattleMgr.Instance.battlePoolMgr.CreateBloodSplat();
        blood.transform.position = transform.position;
        StartCoroutine(CoReturnParticle(blood));

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
            animator.SetTrigger("Death");
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            var time = 0f;
            for (var idx = 0; idx < ac.animationClips.Length; ++idx)
            {
                if (ac.animationClips[idx].name == "Death")
                    time = ac.animationClips[idx].length;
            }

            StartCoroutine(CoDeath(time));
            return true;
        }
        return false;
    }


    private IEnumerator CoDeath(float time)
    {
        yield return new WaitForSeconds(time);

        EventBusMgr.Publish(EventType.DestroyChar, new object[] { this, 0 });
    }

    public bool GetDamage(int dmg)
    {
        var hp = characterStats.currentHp;
        hp -= dmg;

        var go = BattleMgr.Instance.battlePoolMgr.CreateScrollingText();
        var scrollingText = go.GetComponent<ScrollingText>();
        go.transform.position = transform.position;
        scrollingText.SetDamage(dmg, false);

        characterStats.currentHp = Mathf.Clamp(hp, 0, hp);
        var window = BattleMgr.Instance.battleWindowMgr.Open(0) as BattleBasicWindow;
        window.UpdateUI();
        window.UpdateExtraInfo(this);
        if (characterStats.currentHp == 0)
        {
            animator.SetTrigger("Death");
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            var time = 0f;
            for (var idx = 0; idx < ac.animationClips.Length; ++idx)
            {
                if (ac.animationClips[idx].name == "Death")
                    time = ac.animationClips[idx].length;
            }
            StartCoroutine(CoDeath(time));
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
                nextRot = Quaternion.Euler(0, 37, 0);
                break;
            case DirectionType.Bot:
                nextRot = Quaternion.Euler(0, 217, 0);
                break;
            case DirectionType.Left:
                nextRot = Quaternion.Euler(0, 307, 0);
                break;
            case DirectionType.Right:
                nextRot = Quaternion.Euler(0, 127, 0);
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

    public void HMG_A1_SkillCancel()
    {
        isHMGA1Skill = false;
        status = CharacterState.Wait;
    }

    public void DisplaySightTile()
    {
        var frontSightList = BattleMgr.Instance.sightMgr.GetFrontSight(this);
        if (!isOnSightTile && frontSightList.Count > 0)
        {
            var poolMgr = BattleMgr.Instance.battlePoolMgr;
            foreach (var sight in frontSightList)
            {
                var sightTile = poolMgr.CreateSightTile();
                sightTile.transform.position = sight.tileBase.tileIdx + new Vector3(0f, 0.55f);
                sightTileList.Add(sightTile);
            }
            isOnSightTile = true;
        }
    }

    public void ReturnSightTile()
    {
        if (isOnSightTile && sightTileList.Count > 0)
        {
            foreach (var sight in sightTileList)
            {
                var returnToPool = sight.GetComponent<ReturnToPool>();
                returnToPool.Return();
            }
            sightTileList.Clear();
            isOnSightTile = false;
        }
    }


    public void FireAnimation()
    {
        var poolMgr = BattleMgr.Instance.battlePoolMgr;
        var gunOneShot = poolMgr.CreateGunOneShot().GetComponent<ParticleSystem>();
        var bulletEjection = poolMgr.CreateBulletEjection().GetComponent<ParticleSystem>();


        gunOneShot.transform.position = currentWeapon.GetComponent<BattleWeapon>().fireTr.position;
        bulletEjection.transform.position = currentWeapon.GetComponent<BattleWeapon>().fireTr.position;
        gunOneShot.Play();
        bulletEjection.Play();
        
        StartCoroutine(CoReturnParticle(gunOneShot.gameObject));
        StartCoroutine(CoReturnParticle(bulletEjection.gameObject));
        ActionAttack(targetMonster);
    }

    public void ChangeWeaponObject()
    {
        Destroy(currentWeapon);

        switch (characterStats.weapon.type)
        {
            case WeaponStats.WeaponType.Main:
                currentWeapon = Instantiate(mainWeapon, rightHandTr);
                break;
            case WeaponStats.WeaponType.Sub:
                currentWeapon = Instantiate(subWeapon, rightHandTr);
                break;
        }

        currentWeapon.transform.rotation = Quaternion.Euler(fireRot);
    }

    public void UseConsumeItemForHp(int recovery)
    {
        characterStats.
    }
    public void UseConsumeItemForVirus(int recovery)
    {

    }
}
