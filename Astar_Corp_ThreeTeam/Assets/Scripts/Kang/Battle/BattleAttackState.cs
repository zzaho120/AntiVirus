using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAttackState : StateBase
{
    public MonsterChar monster;
    public PlayerableChar target;

    public BattleAttackState(MonsterChar monster, FSM fsm)
    {
        this.monster = monster;
        this.fsm = fsm;
    }
    public override void Enter()
    {
        target = monster.target;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        Debug.Log(target);
        if (target != null)
        {
            var stats = monster.monsterStats;
            target.GetDamage(stats.Damage);
            EventBusMgr.Publish(EventType.EndEnemy);
        }
        else
            fsm.ChangeState((int)BattleMonState.Idle);
    }
}
