using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateBase
{
    private float distance;
    private float startTime;

    private Transform player;
    private Transform monPos;

    private WorldMonsterChar monster;

    public IdleState(WorldMonsterChar monster, FSM fsm)
    {
        this.fsm = fsm;
        this.monster = monster;

        player = GameObject.FindWithTag("Player").transform;
        monPos = monster.GetComponent<Transform>();

        distance = monster.monsterStat.nonBattleMonster.sightRange;
    }

    public override void Enter()
    {
        startTime = Time.time;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (Vector3.Distance(player.position, monPos.transform.position) <= distance)
        {
            fsm.ChangeState(STATE.Chase);
        }
        else
        {
            //var randTime = Random.Range(1f, 3f);
            if (Time.time > startTime + 2f)
            {
                fsm.ChangeState(STATE.Patrol);
            }
        }
    }
}
