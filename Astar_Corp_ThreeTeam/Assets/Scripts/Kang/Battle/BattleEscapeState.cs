using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEscapeState : StateBase
{
    private MonsterChar monster;
    private bool isActing;

    public BattleEscapeState(MonsterChar monster, FSM fsm)
    {
        this.monster = monster;
        this.fsm = fsm;
    }

    public override void Enter()
    {
        isActing = false;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (!isActing)
        {
            isActing = true;
            monster.MoveEscape();
        }
        else if (monster.isMoved)
            EventBusMgr.Publish(EventType.EndEnemy);
    }
}