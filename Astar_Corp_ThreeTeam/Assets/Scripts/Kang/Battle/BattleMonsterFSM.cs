using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleMonState
{
    Idle,
    Attack,
    Escape
}


public class BattleMonsterFSM : FSM
{
    public MonsterChar monster;
    public BattleMonState state;

    public void Init(MonsterChar monster)
    {
        this.monster = monster;
        AddState(new BattleMoveState(monster, this));
        AddState(new BattleAttackState(monster, this));
        AddState(new BattleEscapeState(monster, this));

        ChangeState((int)BattleMonState.Idle);
    }
}
