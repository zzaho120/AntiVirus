using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleMonState
{
    Idle,
    Move,
    Attack,
    EndTurn
}


public class BattleMonsterFSM : FSM
{
    public MonsterChar monster;
    public BattleMonState state;

    public BattleMonsterFSM(MonsterChar monster)
    {
        this.monster = monster;
        Init();
    }

    public void Init()
    {
        AddState(new BattleIdleState(monster, this));

        ChangeState((int)BattleMonState.Idle);
    }
}
