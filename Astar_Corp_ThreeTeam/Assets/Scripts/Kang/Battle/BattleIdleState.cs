using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleIdleState : StateBase
{
    private MonsterChar monster;
    private bool isActing;

    public BattleIdleState(MonsterChar monster, FSM fsm)
    {
        this.fsm = fsm;
        this.monster = monster;
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
        if (monster.target == null)
        {
            if (!isActing)
            {
                isActing = true;
                monster.MoveRandomTile();
            }
        }
        else
        {
            if (!isActing)
            {
                isActing = true;

            }

            if ()
        }
        
    }
}
