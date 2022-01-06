using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChar : BattleChar
{
    [Header("Character")]
    public MonsterStats monsterStats;
    public int moveDistance;
    public int recognition;
    public PlayerableChar target;
    public BattleMonsterFSM fsm;
    public MeshRenderer ren;

    public override void Init()
    {
        base.Init();
        monsterStats.monster = (Monster)Instantiate(Resources.Load("Choi/Datas/Monsters/1"));
        monsterStats.Init();
        fsm = new BattleMonsterFSM();
        fsm.Init(this);
        ren = GetComponent<MeshRenderer>();
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
                transform.position = new Vector3(tile.tileIdx.x, tile.tileIdx.y + 1, tile.tileIdx.z);
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