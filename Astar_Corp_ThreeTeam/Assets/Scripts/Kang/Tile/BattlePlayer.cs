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
    }

    public void StartTurn()
    {
        foreach (var character in playerableChars)
        {
            character.StartTurn();
        }
    }

}
