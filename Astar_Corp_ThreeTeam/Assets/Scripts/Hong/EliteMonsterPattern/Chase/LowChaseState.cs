using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 상수 정의
static class Constants
{
    public const int disance = 15;
}

public class LowChaseState : StateBase
{
    //private float startTime;

    private float distance = Constants.disance;
    private bool isChase;

    private NavMeshAgent agent;
    private Transform player;

    public LowChaseState(FSM fsm)
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

        int randNum = Random.Range(0, 1000);
        if (randNum > 2)
        {
            isChase = false;
            //Debug.Log("노 추적");
        }
        else
        {
            isChase = true;
        }
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        // 플레이어 추적
        if (isChase)
        {
            if (Vector3.Distance(player.position, agent.transform.position) <= distance)
            {
                agent.SetDestination(player.position);
                //Debug.Log("추적");
            }
            else
            {
                fsm.ChangeState(STATE.Idle);
                //Debug.Log("노 추적");
            }
        }
        // 추적 X
        else
        {
            fsm.ChangeState(STATE.Move);
            //Debug.Log("노 추적");
        }
    }

    private IEnumerator ChangeState(STATE state)
    {
        yield return new WaitForSeconds(3f);
        fsm.ChangeState(state);
    }
}
