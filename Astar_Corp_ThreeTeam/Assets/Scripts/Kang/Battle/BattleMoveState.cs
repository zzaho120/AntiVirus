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
        if (monster.monsterStats.CheckRunMonster())
            Debug.Log("run");

        if (Input.GetKeyDown(KeyCode.Tab))
            monster.monsterStats.currentHp = 1;

        if (player == null)
        {
            if (!isActing)
            {
                isActing = true;
                monster.Move(monster.target);
            }
            else if (monster.isMoved && monster.CheckAttackRange() == null)
                EventBusMgr.Publish(EventType.EndEnemy);
            else if (monster.CheckAttackRange() != null && monster.monsterStats.CheckAttackAp())
                fsm.ChangeState((int)BattleMonState.Attack);
        }
        else
        {
            if (monster.monsterStats.CheckAttackAp())
                fsm.ChangeState((int)BattleMonState.Attack);
            else
                EventBusMgr.Publish(EventType.EndEnemy);
        }
    }
}
