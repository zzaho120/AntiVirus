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
        var player = monster.CheckAttackRange();
        if (player != null && monster.monsterStats.CheckAttackAp())
        {
            if (player.GetDamage(monster.monsterStats))
                monster.SetTarget(null);
        }
        else
            EventBusMgr.Publish(EventType.EndEnemy);
    }
}
