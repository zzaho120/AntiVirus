using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonsterMgr : MonoBehaviour
{
    public List<MonsterChar> monsters;
    public int monsterIdx;
    private List<PlayerableChar> playerableChars;
    public void Init()
    {
        monsters.Clear();
        var monsterArr = transform.GetComponentsInChildren<MonsterChar>();
        foreach (var monster in monsterArr)
        {
            monster.Init();
            monsters.Add(monster);
        }

        EventBusMgr.Subscribe(EventType.EndEnemy, CheckEndTurn);
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
        RecognizePlayer();
    }

    public void UpdateTurn()
    {
        if (monsterIdx < monsters.Count)
            monsters[monsterIdx].MonsterUpdate();
    }

    public void CheckEndTurn(object empty)
    {
        monsterIdx++;
        if (monsterIdx >= monsters.Count)
        {
            CameraController.Instance.SetFollowObject(null);
            EventBusMgr.Publish(EventType.ChangeTurn);
        }
        else
            CameraController.Instance.SetFollowObject(monsters[monsterIdx].transform);
    }

    private void RecognizePlayer()
    {
        foreach (var monster in monsters)
        {
            monster.target = null;
            foreach (var player in playerableChars)
            {
                var dist = Vector3.Distance(monster.currentTile.tileIdx, player.currentTile.tileIdx);

                if (monster.recognition > dist)
                {
                    monster.target = player;
                    monster.ren.material.color = Color.cyan;
                }
            }

            if (monster.target == null)
                monster.ren.material.color = Color.red;

            Debug.Log(monster.target);
        }
    }

    public void RemoveMonster(MonsterChar monster)
    {
        var idx = monsters.IndexOf(monster);
        monsters.RemoveAt(idx);
        Destroy(monster.gameObject);
    }
}
