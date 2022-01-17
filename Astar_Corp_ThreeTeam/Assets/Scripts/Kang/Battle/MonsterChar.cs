using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChar : BattleTile
{
    [Header("Character")]
    public MonsterStats monsterStats;
    public BattleMonsterFSM fsm;
    public bool turnState;

    public override void Init()
    {
        base.Init();
        monsterStats.monster = (Monster)Instantiate(Resources.Load("Choi/Datas/Monsters/Bear"));
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

    public void MoveRandomTile()
    {
        var Ap = monsterStats.currentAp;
        var Mp = Ap * (3 + monsterStats.Mp);
        var moveTilePoint = Mp / 2;
        Debug.Log(moveTilePoint);
    }

    public void MoveTile(Vector3 nextIdx)
    {
        foreach (var tile in currentTile.adjNodes)
        {
            if (tile.tileIdx.x == nextIdx.x && tile.tileIdx.z == nextIdx.z)
            {
                CreateHint(HintType.Footprint, nextIdx);
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
            if (playerChars[playerIdx].status != PlayerState.Alert)
                continue;

            if (frontSights[playerIdx].Exists(sightTile => sightTile.tileBase == currentTile))
                boolList[playerIdx] = true;
        }

        return boolList;
    }
}