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

        EventBusMgr.Subscribe(EventType.TurnEnd, StartTurn);
    }

    public void StartTurn(object empty)
    {
        foreach (var character in playerableChars)
        {
            character.StartTurn();
        }
    }
}