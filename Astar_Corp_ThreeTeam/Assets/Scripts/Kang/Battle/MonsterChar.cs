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

    public override void Init()
    {
        base.Init();
        monsterStats.monster = (Monster)Instantiate(Resources.Load("Choi/Datas/Monsters/1"));
        monsterStats.Init();
        fsm = new BattleMonsterFSM();
        fsm.Init(this);
    }

    public void GetDamage(int dmg)
    {
        var hp = monsterStats.currentHp;
        hp -= dmg;
        monsterStats.currentHp = Mathf.Clamp(hp, 0, hp);

        Debug.Log($"{monsterStats.gameObject.name}은 {dmg} 데미지를 입어 {monsterStats.currentHp}가 되었다.");

        if (monsterStats.currentHp == 0)
        {
            BattleMgr.Instance.monsterMgr.monsters.Remove(this);
            Destroy(gameObject);
        }
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
    }
}