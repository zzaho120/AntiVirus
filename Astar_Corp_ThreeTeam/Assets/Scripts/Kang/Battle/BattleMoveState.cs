using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoveState : StateBase
{
    private MonsterChar monster;

    public BattleMoveState(MonsterChar monster, FSM fsm)
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