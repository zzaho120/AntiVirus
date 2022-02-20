using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoveState : StateBase
{
    private MonsterChar monster;
    private bool isActing;
    private PlayerableChar player;
    public BattleMoveState(MonsterChar monster, FSM fsm)
    {
        this.fsm = fsm;
        this.monster = monster;
    }

    public override void Enter()
    {
        isActing = false;
        player = monster.CheckAttackRange();
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        var range = monster.CheckAttackRange();
        var ap = monster.monsterStats.CheckAttackAp();
        if (player == null)
        {
            if (range != null && ap)
                fsm.ChangeState((int)BattleMonState.Attack);
            else if (!isActing && range == null)
            {
                isActing = true;
                monster.Move(monster.target);
            }
            else if (monster.isMoved && range == null)
                EventBusMgr.Publish(EventType.EndEnemy);
            else if (range == null)
                EventBusMgr.Publish(EventType.EndEnemy);
        }
        else
        {
            if (ap)
                fsm.ChangeState((int)BattleMonState.Attack);
            else
                EventBusMgr.Publish(EventType.EndEnemy);
        }
    }
}
