using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonsterMgr : MonoBehaviour
{
    public List<MonsterChar> monsters;
    public int monsterIdx;
    private List<PlayerableChar> playerableChars;
    public MonsterChar curMonster;
    public void Init()
    {
        monsters.Clear();
        var monsterArr = transform.GetComponentsInChildren<MonsterChar>();
        Debug.Log(monsterArr.Length);
        for (var idx = 0; idx < monsterArr.Length; ++idx)
        {
            monsterArr[idx].Init();
            monsterArr[idx].monsterIdx = idx;
            monsters.Add(monsterArr[idx]);
        }

        EventBusMgr.Subscribe(EventType.EndEnemy, SetEndTurn);
        EventBusMgr.Subscribe(EventType.StartEnemy, StartEnemy);

        playerableChars = BattleMgr.Instance.playerMgr.playerableChars;
    }

    public void StartEnemy(object empty)
    {
        monsterIdx = 0;

        var sightMgr = BattleMgr.Instance.sightMgr;
        for (var idx = 0; idx < monsters.Count; ++idx)
        {
            var monster = monsters[idx];

            if (monster.IsfatalDmg)
                monster.fsm.ChangeState((int)BattleMonState.Escape);
            else
                monster.fsm.ChangeState((int)BattleMonState.Idle);
            
            monster.StartTurn();
            sightMgr.InitMonsterSight(idx);
            if (monster.target == null)
                monster.SetTarget(sightMgr.GetPlayerInMonsterSight(idx));
            
        }

        if (monsters.Count > 0)
            curMonster = monsters[0];
    }

    public void UpdateTurn()
    {
        if (curMonster != null)
            curMonster.MonsterUpdate();
    }

    public void SetEndTurn(object[] param)
    {
        monsterIdx++;
        if (!BattleMgr.Instance.sightMgr.GetMonsterInPlayerSight(curMonster.gameObject))
            BattleMgr.Instance.hintMgr.CheckRader(curMonster.tileIdx);
        
        curMonster = null;

        Invoke("CheckEndTurn", RaderWindow.maxTime);
    }

    private void CheckEndTurn()
    {
        if (monsterIdx >= monsters.Count)
        {
            EventBusMgr.Publish(EventType.ChangeTurn);
        }
        else
        {
            curMonster = monsters[monsterIdx];
        }
    }

    public void RemoveMonster(MonsterChar monster)
    {
        var idx = monsters.IndexOf(monster);
        monsters.RemoveAt(idx);
        Destroy(monster.gameObject);

        if (BattleMgr.Instance.turn == BattleTurn.Enemy)
            EventBusMgr.Publish(EventType.EndEnemy);
    }
}
