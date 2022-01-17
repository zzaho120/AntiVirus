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
        foreach (var monster in monsterArr)
        {
            monster.Init();
            monsters.Add(monster);
        }

        EventBusMgr.Subscribe(EventType.EndEnemy, SetEndTurn);
        EventBusMgr.Subscribe(EventType.StartEnemy, StartEnemy);

        playerableChars = BattleMgr.Instance.playerMgr.playerableChars;
    }

    public void StartEnemy(object empty)
    {
        monsterIdx = 0;

        foreach (var monster in monsters)
        {
            monster.fsm.ChangeState((int)BattleMonState.Idle);
        }
        curMonster = monsters[0];
        RecognizePlayer();
    }

    public void UpdateTurn()
    {
        if (curMonster != null)
            curMonster.MonsterUpdate();
    }

    public void SetEndTurn(object[] param)
    {
        monsterIdx++;
        curMonster = null;
        BattleMgr.Instance.hintMgr.CheckRader((Vector3)param[0]);

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

    private void RecognizePlayer()
    {
        foreach (var monster in monsters)
        {
            //monster.target = null;
            foreach (var player in playerableChars)
            {

                //if (monster.recognition > dist)
                //{
                //    monster.target = player;
                //    monster.ren.material.color = Color.cyan;
                //}
            }

            //if (monster.target == null)
            //    monster.ren.material.color = Color.red;
        }
    }

    public void RemoveMonster(MonsterChar monster)
    {
        var idx = monsters.IndexOf(monster);
        monsters.RemoveAt(idx);
        Destroy(monster.gameObject);
    }
}
