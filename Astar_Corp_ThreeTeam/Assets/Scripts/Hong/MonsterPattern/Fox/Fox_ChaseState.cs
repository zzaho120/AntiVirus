using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fox_ChaseState : StateBase
{
    private float distance = Constants.foxDistance;

    private NavMeshAgent agent;
    private Transform player;

    public Fox_ChaseState(FSM fsm)
    {
        this.fsm = fsm;

        if (this.fsm != null)
        {
            agent = fsm.GetComponent<NavMeshAgent>();
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
