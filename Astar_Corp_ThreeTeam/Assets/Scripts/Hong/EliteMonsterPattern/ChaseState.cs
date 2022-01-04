using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EliteMonsterState
{
    private float startTime;

    private NavMeshAgent agent;
    private Transform target;

    public ChaseState(FSM fsm)
    {
        this.fsm = fsm;

        if (this.fsm != null)
        {
            agent = fsm.GetComponent<NavMeshAgent>();
            target = GameObject.FindWithTag("Player").transform;
        }
    }
    public override void Enter()
    {
        startTime = Time.time;

        agent.SetDestination(target.position);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        var randTime = Random.Range(3f, 5f);

        if (Time.time > startTime + randTime)
            fsm.ChangeState(STATE.Move);
    }
}
