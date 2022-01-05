using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerMgr : MonoBehaviour
{
    public List<PlayerableChar> playerableChars;

    public void Init()
    {
        playerableChars.Clear();
        var playerArr = transform.GetComponentsInChildren<PlayerableChar>();
        foreach (var player in playerArr)
        {
            player.Init();
            playerableChars.Add(player);
        }

        EventBusMgr.Subscribe(EventType.StartPlayer, StartTurn);
        EventBusMgr.Subscribe(EventType.EndPlayer, CheckEndTurn);
    }

    public void StartTurn(object empty)
    {
        var monsters = BattleMgr.Instance.monsterMgr.monsters;
        foreach (var character in playerableChars)
        {
            character.StartTurn();

            var levelList = new List<int>();
            foreach (var monster in monsters)
            {
                levelList.Add(character.GetVirusLevel(monster));
            }

            for (var i = 0; i < levelList.Count; ++i)
            {
                var virusType = monsters[i].monsterStats.virus.id;

                if (levelList[i] < 1)
                    continue;


                var maxLevel = int.MinValue;
                // 각 바이러스의 최고레벨을 적용하자!
                for (var j = 0; j < levelList.Count; ++j)
                {
                    if (i == j || levelList[j] < 1)
                        continue;

                    if (virusType == monsters[j].monsterStats.virus.id)
                    {
                    }
                }
            }
        }
    }

    public void CheckEndTurn(object empty)
    {
        var turnEndCount = 0; 
        foreach (var player in playerableChars)
        {
            if (player.status == PlayerStatus.TurnEnd || player.status == PlayerStatus.Alert)
                turnEndCount++;
        }

        if (turnEndCount == playerableChars.Count)
            EventBusMgr.Publish(EventType.ChangeTurn);
    }
}