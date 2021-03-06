using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MonsterStats))]
public class MonsterChar : BattleTile
{
    [Header("Character")]
    public int monsterIdx;
    public MonsterStats monsterStats;
    public BattleMonsterFSM fsm;
    public bool turnState;
    public PlayerableChar target;

    public bool IsNullTarget { get => target == null; }
    public bool isMoved;
    public bool isSelect;
    public List<GameObject> renList;

    public Animator animator;

    private int cumulativeDmg;
    private PlayerableChar lastAttacker;

    public List<VirusBase> virusList = new List<VirusBase>();
    private List<GameObject> sightTileList = new List<GameObject>();

    public string ownerName;
    public BattleFloatingInfo info;
    public bool IsfatalDmg
    {
        get
        {
            var monster = monsterStats.monster;
            return cumulativeDmg > monster.maxHp * (monster.escapeHpDec / 100f);
        }
    }
    public override void Init()
    {
        base.Init();
        monsterStats.Init();
        fsm = new BattleMonsterFSM();
        fsm.Init(this);
        animator = GetComponent<Animator>();
        SetOwnerName();
    }

    public void SetOwnerName()
    {
        switch (monsterStats.monster.name)
        {
            case "Bear":
                ownerName = "??";
                break;
            case "Boar":
                ownerName = "??????";
                break;
            case "Fox":
                ownerName = "????";
                break;
            case "Wolf":
                ownerName = "????";
                break;
            case "Jaguar":
                ownerName = "???Ծ?";
                break;
            case "Spider":
                ownerName = "?Ź?";
                break;
            case "Tiger":
                ownerName = "ȣ????";
                break;
        }
    }
    public void StartTurn()
    {
        monsterStats.StartTurn();
        isMoved = false;
        cumulativeDmg = 0;
    }

    public float GetDamage(PlayerableChar player, bool isCrit)
    {
        lastAttacker = player;
        var hp = monsterStats.currentHp;
        var dmg = 0;
        if (!isCrit)
            dmg = player.characterStats.weapon.Damage;
        else
            dmg = (int)(player.characterStats.weapon.Damage * player.characterStats.weapon.CritDmg / 100f);

        hp -= dmg;
        cumulativeDmg += dmg;
        monsterStats.currentHp = Mathf.Clamp(hp, 0, hp);

        var go = BattleMgr.Instance.battlePoolMgr.CreateScrollingText();
        var scrollingText = go.GetComponent<ScrollingText>();
        go.transform.position = transform.position;
        scrollingText.SetDamage(dmg, false);

        

       

        var blood = BattleMgr.Instance.battlePoolMgr.CreateBloodSplat();
        blood.transform.position = transform.position;
        StartCoroutine(CoReturnParticle(blood));

        if (monsterStats.currentHp == 0)
        {
            animator.SetTrigger("Death");
            var dieSoundObj = BattleMgr.Instance.battlePoolMgr.CreateMonsterDieSound();
            var dieSound = dieSoundObj.GetComponent<AudioSource>();
            dieSound.Play();
            var returnToPool = dieSoundObj.GetComponent<ReturnToPool>();
            returnToPool.Return(3f);
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            var time = 0f;
            for (var idx = 0; idx < ac.animationClips.Length; ++idx)
            {
                if (ac.animationClips[idx].name == "Death")
                    time = ac.animationClips[idx].length;
            }
            StartCoroutine(CoDeath(player, time));
            return time;
        }
        else
        {
            animator.SetTrigger("Damaged");
            var hitSoundObj = BattleMgr.Instance.battlePoolMgr.CreateMonsterHitSound();
            var hitSound = hitSoundObj.GetComponent<AudioSource>();
            hitSound.Play(); 
            var returnToPool = hitSoundObj.GetComponent<ReturnToPool>();
            returnToPool.Return(3f);
        }

        return 0;
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

    private IEnumerator CoDeath(PlayerableChar player, float time)
    {
        player.characterStats.GetExp(monsterStats.monster.exp);
        if (time <= 0f)
            time = 1.5f;
        yield return new WaitForSeconds(time);

        EventBusMgr.Publish(EventType.DestroyChar, new object[] { this, 1 });

        if (player.AP <= 0)
            player.EndPlayer();
    }

    public void SetTarget(PlayerableChar player)
    {
        if (player != null)
        {
            var particle = BattleMgr.Instance.battlePoolMgr.CreateDetect();
            particle.transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
            particle.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
            var returnToPool = particle.GetComponent<ReturnToPool>();
            returnToPool.Return(1f);

            var isInSight = BattleMgr.Instance.sightMgr.GetMonsterInPlayerSight(this.gameObject);

            if (isInSight)
            {
                var detectObj = BattleMgr.Instance.battlePoolMgr.CreateDetectSound();
                var detectSound = detectObj.GetComponent<AudioSource>();
                detectSound.Play();
                var detectReturnToPool = detectSound.GetComponent<ReturnToPool>();
                detectReturnToPool.Return(1f);
            }
        }
        target = player;
    }

    public void MonsterUpdate()
    {
        fsm.Update();
    }

    public void Move(PlayerableChar target)
    {
        var Ap1ByMp = monsterStats.Mp;
        var mp = monsterStats.currentAp * Ap1ByMp;

        if (target == null)
            MoveRandomTile(mp, Ap1ByMp);
        else
            MoveTarget(mp, Ap1ByMp);
    }

    public void MoveEscape()
    {
        var Ap1ByMp = monsterStats.Mp;
        var mp = monsterStats.currentAp * Ap1ByMp;

        var tileDics = BattleMgr.Instance.tileMgr.tileDics;
        var destTile = Vector3.zero;

        while (true)
        {
            var playerIdx = lastAttacker.tileIdx;
            var dir = tileIdx - playerIdx;

            var newX = 0;
            var newZ = 0;

            if (dir.x > 0)
                newX = 1;
            else if (dir.x < 0)
                newX = -1;

            if (dir.z > 0)
                newZ = 1;
            else if (dir.z < 0)
                newZ = -1;

            var maxTile = mp / 2;
            var randomX = Random.Range(0, maxTile + 1);
            var randomZ = maxTile - randomX;

            var escapeIdx = new Vector3(randomX * newX, 0, randomZ * newZ);

            destTile = tileIdx + escapeIdx;
            Debug.Log(destTile);
            if (tileDics.ContainsKey(destTile))
            {
                BattleMgr.Instance.pathMgr.InitAStar(tileIdx, destTile);
                break;
            }
        }

        StartCoroutine(CoMove(Ap1ByMp, destTile));
    }


    public void MoveRandomTile(int mp, int Ap1ByMp)
    {
        var tileDics = BattleMgr.Instance.tileMgr.tileDics;
        var destTile = Vector3.zero;
        while (true)
        {
            var maxTile = mp / 2;
            var randomX = Random.Range(-maxTile, maxTile + 1);
            var randomZ = 0;
            if (randomX > 0)
                randomZ = maxTile - randomX;
            else
                randomZ = maxTile + randomX;

            if (Random.Range(0, 2) == 0)
                randomZ = -randomZ;

            destTile = tileIdx + new Vector3(randomX, 0, randomZ);
            if (tileDics.ContainsKey(destTile))
            {
                if (destTile == currentTile.tileIdx)
                    continue;

                BattleMgr.Instance.pathMgr.InitAStar(tileIdx, destTile);
                break;
            }
        }

        StartCoroutine(CoMove(Ap1ByMp, destTile));
    }

    public void MoveTarget(int mp, int Ap1ByMp)
    {
        var tileDics = BattleMgr.Instance.tileMgr.tileDics;
        var destTile = Vector3.zero;
        BattleMgr.Instance.pathMgr.InitAStar(tileIdx, target.tileIdx);
        StartCoroutine(CoMove(Ap1ByMp, target.tileIdx));
    }

    public IEnumerator CoMove(int Ap1ByMp, Vector3 destTile)
    {
        animator.SetBool("Run", true);
        var pathMgr = BattleMgr.Instance.pathMgr;
        var printIdx = 0;

        if (Ap1ByMp < 2)
            printIdx = 1;
        else if (Ap1ByMp < 4)
            printIdx = 2;
        else if (Ap1ByMp >= 4)
            printIdx = 3;

        var idx = 1;
        var moveIdx = 0;
        var compareIdx = Mathf.Abs(destTile.x) + Mathf.Abs(destTile.z);
        while (Mathf.Abs(tileIdx.x) + Mathf.Abs(tileIdx.z) + monsterStats.AtkRange != compareIdx)
        {
            AStarTile nextTile = null;
            if (pathMgr.pathList.Count > 0)
                nextTile = pathMgr.pathList.Pop();
            else
                break;

            idx++;
            if (idx >= printIdx)
            {
                idx = 0;
                CreateHint(HintType.Footprint, nextTile.tileBase.tileIdx);
                var footPrintIsInSight = BattleMgr.Instance.sightMgr.GetSightDisplay(currentTile);
                currentTile.EnableDisplay(footPrintIsInSight);
            }

            if (moveIdx == 0)
                monsterStats.currentAp--;
            moveIdx += 2;
            if (moveIdx >= Ap1ByMp)
                moveIdx = 0;
            if (monsterStats.currentAp == 0)
                break;

            var sightMgr = BattleMgr.Instance.sightMgr;
            sightMgr.InitMonsterSight(this);
            if (target == null)
                SetTarget(sightMgr.GetPlayerInMonsterSight(this));

            yield return MoveTile(nextTile.tileBase.tileIdx);
        }

        if (moveIdx > 0)
            monsterStats.currentAp--;

        isMoved = true;

        animator.SetBool("Run", false);
        animator.SetTrigger("Idle");

        if (monsterStats.originMaxHp > monsterStats.currentHp)
            CreateHint(HintType.Bloodprint, currentTile.tileIdx);

        var bloodIsInSight = BattleMgr.Instance.sightMgr.GetSightDisplay(currentTile);
        currentTile.EnableDisplay(bloodIsInSight);
    }

    public IEnumerator MoveTile(Vector3 nextIdx)
    {

        foreach (var tile in currentTile.adjNodes)
        {
            if (tile.tileIdx.x == nextIdx.x && tile.tileIdx.z == nextIdx.z)
            {
                if (tile.charObj != null && tile.charObj.CompareTag("BattlePlayer"))
                {
                    EventBusMgr.Publish(EventType.EndEnemy);
                    yield break;
                }
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
                yield return StartCoroutine(CoMoveChar(nextIdx + new Vector3(0f, 0.5f, 0f)));
            }
        }
        var isInSight = BattleMgr.Instance.sightMgr.GetSightDisplay(currentTile);
        currentTile.EnableDisplay(isInSight);
        info.gameObject.SetActive(isInSight);

        var boolList = CheckFrontSight();
        foreach (var elem in boolList)
        {
            if (elem)
            {
                EventBusMgr.Publish(EventType.DetectAlert, new object[] { boolList, this });
                break;
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
    private void CreateHint(HintType hintType, Vector3 nextIdx)
    {
        var directionIdx =  currentTile.tileIdx - nextIdx;
        var hintMgr = BattleMgr.Instance.hintMgr;
        var directionType = DirectionType.None;
        if (directionIdx.x != 0)
        {
            switch (directionIdx.x)
            {
                case -1:
                    directionType = DirectionType.Right;
                    break;
                case 1:
                    directionType = DirectionType.Left;
                    break;
            }
        }
        else if (directionIdx.z != 0)
        {
            switch (directionIdx.z)
            {
                case -1:
                    directionType = DirectionType.Top;
                    break;
                case 1:
                    directionType = DirectionType.Bot;
                    break;
            }
        }
        hintMgr.AddPrint(hintType, directionType, currentTile.tileIdx, this);
    }

    public bool[] CheckFrontSight()
    {
        var frontSights = BattleMgr.Instance.sightMgr.frontSightList;
        var playerChars = BattleMgr.Instance.playerMgr.playerableChars;

        var boolList = new bool[frontSights.Count];

        for (var playerIdx = 0; playerIdx < frontSights.Count; ++playerIdx)
        {
            if (playerChars[playerIdx].status != CharacterState.Alert)
                continue;

            if (frontSights[playerIdx].Exists(sightTile => sightTile.tileBase == currentTile))
                boolList[playerIdx] = true;
        }

        return boolList;
    }

    public PlayerableChar CheckAttackRange()
    {
        var adjTiles = currentTile.adjNodes;
        var rangeCnt = 0;

        var player = CheckAttackRangeByDirection(DirectionType.Top, rangeCnt, tileIdx);
        if (player != null)
            return player;

        player = CheckAttackRangeByDirection(DirectionType.Bot, rangeCnt, tileIdx);
        if (player != null)
            return player;

        player = CheckAttackRangeByDirection(DirectionType.Left, rangeCnt, tileIdx);
        if (player != null)
            return player;

        player = CheckAttackRangeByDirection(DirectionType.Right, rangeCnt, tileIdx);
        if (player != null)
            return player;

        return null;
    }

    public PlayerableChar CheckAttackRangeByDirection(DirectionType directionType, int rangeCnt, Vector3 tileIdx)
    {
        PlayerableChar result = null;
        var nextTile = Vector3.zero;
        switch (directionType)
        {
            case DirectionType.Top:
                nextTile = new Vector3(tileIdx.x, tileIdx.y, Mathf.Clamp(tileIdx.z + 1, 0, TileMgr.MAX_Z_IDX));
                break;
            case DirectionType.Bot:
                nextTile = new Vector3(tileIdx.x, tileIdx.y, Mathf.Clamp(tileIdx.z - 1, 0, TileMgr.MAX_Z_IDX));
                break;
            case DirectionType.Left:
                nextTile = new Vector3(Mathf.Clamp(tileIdx.x - 1, 0, TileMgr.MAX_X_IDX), tileIdx.y, tileIdx.z);
                break;
            case DirectionType.Right:
                nextTile = new Vector3(Mathf.Clamp(tileIdx.x + 1, 0, TileMgr.MAX_X_IDX), tileIdx.y, tileIdx.z);
                break;
        }
        if (rangeCnt >= monsterStats.AtkRange)
            return null;

        var tileDics = BattleMgr.Instance.tileMgr.tileDics;
        if (tileDics.ContainsKey(nextTile))
        {
            var charObj = tileDics[nextTile].charObj;
            if (charObj != null)
            {
                var player = charObj.GetComponent<PlayerableChar>();
                if (player != null && player == target)
                    return player;
            }
        }

        rangeCnt++;
        result = CheckAttackRangeByDirection(directionType, rangeCnt, nextTile);

        if (result != null)
            return result;
        else
            return null;
    }

    public void FloodFillVirus()
    {
        var cnt = 0;
        var adjTiles = currentTile.adjNodes;

        foreach (var adjTile in adjTiles)
        {
            CheckVirusArea(adjTile, currentTile, monsterStats.virusLevel, cnt);
        }
    }

    public void DisplaySightTile()
    {
        var frontSightList = BattleMgr.Instance.sightMgr.GetMonsterSight(this);
        var poolMgr = BattleMgr.Instance.battlePoolMgr;
        foreach (var sight in frontSightList)
        {
            var sightTile = poolMgr.CreateMonsterSightTile();
            sightTile.transform.position = sight.tileBase.tileIdx + new Vector3(0f, 0.55f);
            sightTileList.Add(sightTile);
        }
    }

    public void ReturnSightTile()
    {
        foreach (var sight in sightTileList)
        {
            var returnToPool = sight.GetComponent<ReturnToPool>();
            returnToPool.Return();
        }
        sightTileList.Clear();
    }

    private void CheckVirusArea(TileBase tile, TileBase origin, int virusLevel, int cnt)
    {
        cnt++;
        var difference = Mathf.Abs(origin.tileIdx.x - tile.tileIdx.x) + Mathf.Abs(origin.tileIdx.z - tile.tileIdx.z);

        if (cnt >= virusLevel)
            return;

        var level = virusLevel - (int)difference;

        if (BattleMgr.Instance.fieldVirusLevel > level)
            level = BattleMgr.Instance.fieldVirusLevel;

        if (tile.virusLevel > level)
            tile.virusLevel = level;
        var alpha = 1f - (float)level / virusLevel - 0.3f;

        var go = BattleMgr.Instance.battlePoolMgr.CreateVirusTile();
        go.transform.position = tile.tileIdx + new Vector3(0, 0.55f);
        var virusTile = go.GetComponent<VirusBase>();

        var virus = monsterStats.virus;
        virusTile.Init(tile, virus.name, level, virus.virusGauge);
        virusList.Add(virusTile);

        var adjTiles = tile.adjNodes;
        foreach (var adjTile in adjTiles)
        {
            CheckVirusArea(adjTile, currentTile, virusLevel, cnt);
        }
    }

    public void ReturnVirusTile()
    {
        foreach (var virusTile in virusList)
        {
            var returnToPool = virusTile.GetComponent<ReturnToPool>();
            returnToPool.Return();
        }
        virusList.Clear();
    }

    private bool isCoAttack;
    public void Attack(PlayerableChar player)
    {
        animator.SetTrigger("Attack");
        if (!isCoAttack)
            StartCoroutine(CoAttack(player));
    }

    private IEnumerator CoAttack(PlayerableChar player)
    {
        isCoAttack = true;
        var dir = (currentTile.tileIdx - player.currentTile.tileIdx).normalized;

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

        var isHit = Random.Range(0, 100) > player.characterStats.avoidRate;
        monsterStats.CalculateAttackAp();
        if (isHit)
        {
            var isCrit = Random.Range(0, 100) < monsterStats.Crit_Rate - player.characterStats.critResistRate;
            if (player.GetDamage(monsterStats, isCrit))
                SetTarget(null);
        }
        else
        {
            var go = BattleMgr.Instance.battlePoolMgr.CreateScrollingText();
            var scrollingText = go.GetComponent<ScrollingText>();
            go.transform.position = player.transform.position;
            scrollingText.SetMiss();
        }

        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        var time = 0f;
        for (var idx = 0; idx < ac.animationClips.Length; ++idx)
        {
            if (ac.animationClips[idx].name == "Attack")
                time = ac.animationClips[idx].length;
        }

        yield return new WaitForSeconds(time);
        isCoAttack = false;
    }
}