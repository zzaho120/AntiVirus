using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateBase
{
    private float distance;

    private NavMeshAgent agent;
    private Transform player;

    private WorldMonsterChar monster;

    public ChaseState(WorldMonsterChar monster, FSM fsm)
    {
        this.fsm = fsm;
        this.monster = monster;

        distance = monster.monsterStat.nonBattleMonster.areaRange;

        if (this.fsm != null)
        {
            agent = monster.GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
        }
    }
    public override void Enter()
    {
        //startTime = Time.time;
        Debug.Log("����");
        agent.SetDestination(player.position);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        // ���� �Ÿ� ����� ���� �ڸ���
        if (Vector3.Distance(player.position, agent.transform.position) > distance)
        {
            fsm.ChangeState(STATE.Patrol);
        }
        // ��ġ ������Ʈ
        else
        {
            agent.SetDestination(player.position);
            //Debug.Log("�÷��̾� ����");
        }
    }
}
