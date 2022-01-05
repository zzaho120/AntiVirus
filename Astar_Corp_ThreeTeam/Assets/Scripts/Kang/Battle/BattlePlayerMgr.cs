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
        foreach (var character in playerableChars)
        {
            character.StartTurn();
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