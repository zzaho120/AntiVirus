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
            var isHit = Random.Range(0, 100) > monster.target.characterStats.avoidRate;

            var dir = (monster.currentTile.tileIdx - player.currentTile.tileIdx).normalized;

            var rotY = 0;
            if (dir.x > 0)
                rotY = 270;
            else if (dir.x < 0)
                rotY = 90;
            else if (dir.z > 0)
                rotY = 180;
            else if (dir.z < 0)
                rotY = 0;
            monster.transform.rotation = Quaternion.Euler(0f, rotY, 0f);

            if (isHit)
            {
                var isCrit = Random.Range(0, 100) < monster.monsterStats.Crit_Rate - monster.target.characterStats.critResistRate;
                monster.animator.SetTrigger("Attack");
                if (player.GetDamage(monster.monsterStats, isCrit))
                    monster.SetTarget(null);
            }
        }
        else
            EventBusMgr.Publish(EventType.EndEnemy);
    }
}
