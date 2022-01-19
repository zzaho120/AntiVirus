using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEscapeState : StateBase
{
    private MonsterChar monster;

    public BattleEscapeState(MonsterChar monster, FSM fsm)
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

    private void SetPath()
    {
    }
}