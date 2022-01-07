using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleMonState
{
    Idle,
    Move,
    Attack
}


public class BattleMonsterFSM : FSM
{
    public MonsterChar monster;
    public BattleMonState state;

    public void Init(MonsterChar monster)
    {
        this.monster = monster;
        AddState(new BattleIdleState(monster, this));
        AddState(new BattleMoveState(monster, this));
        AddState(new BattleAttackState(monster, this));

        ChangeState((int)BattleMonState.Idle);
    }
}
