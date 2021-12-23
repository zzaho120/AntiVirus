using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    public List<PlayerableChar> playerableChars;

    public void Init()
    {
        foreach (var character in playerableChars)
        {
            character.Init();
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
