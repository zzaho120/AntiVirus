using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MonsterStats))]
public class MonsterChar : BattleTile
{
    [Header("Character")]
    public MonsterStats monsterStats;
    public BattleMonsterFSM fsm;
    public bool turnState;

    public PlayerableChar target;
    public bool isMoved;

    public override void Init()
    {
        base.Init();
        monsterStats = GetComponent<MonsterStats>();
        monsterStats.monster = (Monster)Instantiate(Resources.Load("Choi/Datas/Monsters/Fox"));
        monsterStats.Init();
        fsm = new BattleMonsterFSM();
        fsm.Init(this);
    }

    public void StartTurn()
    {
        monsterStats.StartTurn();
        isMoved = false;
    }

    public void GetDamage(int dmg)
    {
        var hp = monsterStats.currentHp;
        hp -= dmg;
        monsterStats.currentHp = Mathf.Clamp(hp, 0, hp);

        var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
        window.SetMsgText($"{monsterStats.gameObject.name} is damaged {dmg}Point - HP : {monsterStats.currentHp}");

        if (monsterStats.currentHp == 0)
            EventBusMgr.Publish(EventType.DestroyChar, new object[] { this, 1 });
    }

    public void MonsterUpdate()
    {
        fsm.Update();
    }

    public void Move(PlayerableChar target)
    {
        var Ap1ByMp = 3 + monsterStats.Mp;
        var mp = monsterStats.currentAp * Ap1ByMp;
        var pathMgr = BattleMgr.Instance.pathMgr;
        var destTile = Vector3.zero;

        if (target == null)
            MoveRandomTile(mp, Ap1ByMp);
        else
            MoveTarget(mp, Ap1ByMp);
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
            Debug.Log(destTile);
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
        var pathMgr = BattleMgr.Instance.pathMgr;
        var printIdx = 0;

        if (Ap1ByMp < 2)
            printIdx = 1;
        else if (Ap1ByMp < 4)
            printIdx = 2;
        else if (Ap1ByMp > 4)
            printIdx = 3;

        var idx = 1;
        var moveIdx = 0;
        CreateHint(HintType.Footprint, tileIdx);

        var compareIdx = Mathf.Abs(destTile.x) + Mathf.Abs(destTile.z);
        while (Mathf.Abs(tileIdx.x) + Mathf.Abs(tileIdx.z) + monsterStats.AtkRange != compareIdx)
        {
            idx++;
            if (idx >= printIdx)
            {
                idx = 0;
                CreateHint(HintType.Footprint, tileIdx);
            }

            var nextTile = pathMgr.pathList.Pop();
            if (!MoveTile(nextTile.tileBase.tileIdx))
                break;

            if (moveIdx == 0)
                monsterStats.currentAp--;
            moveIdx += 2;
            if (moveIdx >= Ap1ByMp)
                moveIdx = 0;
            if (monsterStats.currentAp == 0)
                break;
            yield return new WaitForSeconds(0.1f);
        }

        if (moveIdx > 0)
            monsterStats.currentAp--;

        isMoved = true;
    }

    public bool MoveTile(Vector3 nextIdx)
    {
        foreach (var tile in currentTile.adjNodes)
        {
            if (tile.tileIdx.x == nextIdx.x && tile.tileIdx.z == nextIdx.z)
            {
                if (tile.charObj != null && tile.charObj.CompareTag("BattlePlayer"))
                    return false;

                currentTile.charObj = null;
                tileIdx = nextIdx;
                currentTile = tile;
                currentTile.charObj = gameObject;
                transform.position = new Vector3(tile.tileIdx.x, tile.tileIdx.y + 0.5f, tile.tileIdx.z);
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

        return true;
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
}