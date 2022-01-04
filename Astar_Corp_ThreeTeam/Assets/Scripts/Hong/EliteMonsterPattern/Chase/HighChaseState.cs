using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HighChaseState : StateBase
{
    //private float startTime;
    private float distance = Constants.disance;

    private NavMeshAgent agent;
    private Transform player;

    public HighChaseState(FSM fsm)
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

        agent.SetDestination(player.position);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        // 일정 거리 벗어나면 원래 자리로
        if (Vector3.Distance(player.position, agent.transform.position) > distance)
        {
            fsm.ChangeState(STATE.Move);
        }
        // 위치 업데이트
        else
        {
            agent.SetDestination(player.position);
            //Debug.Log("플레이어 추적");
        }
    }
}
