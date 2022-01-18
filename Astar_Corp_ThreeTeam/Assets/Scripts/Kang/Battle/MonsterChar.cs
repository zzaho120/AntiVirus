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
        var mp = monsterStats.currentAp * (3 + monsterStats.Mp);
        var pathMgr = BattleMgr.Instance.pathMgr;
        var destTile = Vector3.zero;

        if (target == null)
            MoveRandomTile(mp);
    }

    public void MoveRandomTile(int mp)
    {
        var Ap1ByMp = 3 + monsterStats.Mp;
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

    public void MoveTarget()
    {

    }
    public IEnumerator CoMove(int mp, Vector3 destTile)
    {
        var pathMgr = BattleMgr.Instance.pathMgr;
        var printIdx = 0;

        if (mp < 2)
            printIdx = 1;
        else if (mp < 4)
            printIdx = 2;
        else if (mp > 4)
            printIdx = 3;

        var idx = 1; 
        CreateHint(HintType.Footprint, tileIdx);
        
        while (tileIdx != destTile)
        {
            idx++;
            if (idx >= printIdx)
            {
                idx = 0;
                CreateHint(HintType.Footprint, tileIdx);
            }

            var nextTile = pathMgr.pathList.Pop();
            MoveTile(nextTile.tileBase.tileIdx);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void MoveTile(Vector3 nextIdx)
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
}