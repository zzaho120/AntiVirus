using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAttackState : StateBase
{
    public MonsterChar monster;

    public BattleAttackState(MonsterChar monster, FSM fsm)
    {
        this.monster = monster;
        this.fsm = fsm;
    }
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }
}
