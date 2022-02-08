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

    private List<MoveTile> virusList = new List<MoveTile>();
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
    }

    public void StartTurn()
    {
        monsterStats.StartTurn();
        isMoved = false;
        cumulativeDmg = 0;
    }

    public void GetDamage(PlayerableChar player, bool isCrit)
    {
        animator.SetTrigger("Damaged");
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

        var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
        window.SetMsgText($"{monsterStats.gameObject.name} is damaged {dmg}Point - HP : {monsterStats.currentHp}");

        if (monsterStats.currentHp == 0)
        {
            player.characterStats.GetExp(monsterStats.monster.exp);
            EventBusMgr.Publish(EventType.DestroyChar, new object[] { this, 1 });
        }
    }

    public void SetTarget(PlayerableChar player)
    {
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
            idx++;
            if (idx >= printIdx)
            {
                idx = 0;
                CreateHint(HintType.Footprint, tileIdx);
            }

            var isInSight = BattleMgr.Instance.sightMgr.GetSightDisplay(currentTile);
            currentTile.EnableDisplay(isInSight);

            AStarTile nextTile = null;
            if (pathMgr.pathList.Count > 0)
                nextTile = pathMgr.pathList.Pop();
            else
                break;

            //if (!MoveTile(nextTile.tileBase.tileIdx))
            //    break;

            if (moveIdx == 0)
                monsterStats.currentAp--;
            moveIdx += 2;
            if (moveIdx >= Ap1ByMp)
                moveIdx = 0;
            if (monsterStats.currentAp == 0)
                break;

            var sightMgr = BattleMgr.Instance.sightMgr;
            sightMgr.InitMonsterSight(monsterIdx);
            if (target == null)
                SetTarget(sightMgr.GetPlayerInMonsterSight(monsterIdx));

            //if (isInSight)
            //    yield return new WaitForSeconds(0.1f);
            //else
            //    yield return null;

            yield return MoveTile(nextTile.tileBase.tileIdx);
        }

        if (moveIdx > 0)
            monsterStats.currentAp--;

        isMoved = true;

        animator.SetBool("Run", false);
        animator.SetTrigger("Idle");

        if (monsterStats.originMaxHp > monsterStats.currentHp)
            CreateHint(HintType.Bloodprint, currentTile.tileIdx);
    }

    public IEnumerator MoveTile(Vector3 nextIdx)
    {
        foreach (var tile in currentTile.adjNodes)
        {
            if (tile.tileIdx.x == nextIdx.x && tile.tileIdx.z == nextIdx.z)
            {
                //if (tile.charObj != null && tile.charObj.CompareTag("BattlePlayer"))
                //    return false;
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

        var boolList = CheckFrontSight();
        foreach (var elem in boolList)
        {
            if (elem)
            {
                EventBusMgr.Publish(EventType.DetectAlert, new object[] { boolList, this });
                break;
            }
        }

        //return true;
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
        var directionIdx = nextIdx - currentTile.tileIdx;
        var hintMgr = BattleMgr.Instance.hintMgr;
        var directionType = DirectionType.None;
        if (directionIdx.x != 0)
        {
            switch (directionIdx.x)
            {
                case 1:
                    directionType = DirectionType.Right;
                    break;
                case -1:
                    directionType = DirectionType.Left;
                    break;
            }
        }
        else if (directionIdx.z != 0)
        {
            switch (directionIdx.z)
            {
                case 1:
                    directionType = DirectionType.Top;
                    break;
                case -1:
                    directionType = DirectionType.Bot;
                    break;
            }
        }
        hintMgr.AddPrint(hintType, directionType, currentTile.tileIdx);
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

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!BattleMgr.Instance.playerMgr.GetPlayerSelected())
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        isSelect = !isSelect;
                        var battleInfo = BattleMgr.Instance.battleWindowMgr.Open(2, false).GetComponent<BattleInfoWindow>();

                        if (isSelect)
                        {
                            battleInfo.EnableMonsterInfo(true, this);
                            FloodFillVirus();
                        }
                        else
                        {
                            battleInfo.Close();
                            ReturnVirusTile();
                        }
                    }
                }
            }
        }
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
        go.transform.position = tile.tileIdx + new Vector3(0, 0.5f);
        var virusTile = go.GetComponent<MoveTile>();
        virusTile.parent = tile;
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
}