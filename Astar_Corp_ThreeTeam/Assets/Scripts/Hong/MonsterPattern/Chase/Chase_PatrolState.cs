using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase_PatrolState : StateBase
{
    private float startTime;
    private float moveRange = 5f;   // 이동 범위 설정
    private float distance = Constants.disance;    // 플레이어, 몬스터간 거리

    private Vector3 targetPos;
    private Vector3 startPos;

    private Transform player;
    private NavMeshAgent agent;

    public Chase_PatrolState(FSM fsm)
    {
        this.fsm = fsm;
        
        if (this.fsm != null)
        {
            agent = fsm.GetComponent<NavMeshAgent>();
            startPos = fsm.transform.position;  // 시작 위치 저장

            player = GameObject.FindWithTag("Player").transform;
        }
    }

    public override void Enter()
    {
        // Debug.Log(startPos);

        startTime = Time.time;

        // 랜덤 범위 설정
        var randX = Random.Range(-5f, 5f);
        var randY = Random.Range(-5f, 5f);

        // 타겟 위치 설정
        targetPos = startPos + new Vector3(randX, fsm.transform.position.y, randY);

        // 허용 범위를 초과하면 타겟위치 = 시작위치
        if (Vector3.Distance(targetPos, startPos) > moveRange)
        {
            //Debug.Log("Range Over");
            targetPos = startPos;
        }

        // 내비메쉬 이동
        agent.SetDestination(targetPos);
    }

    public override void Exit()
    {
        agent.SetDestination(startPos);
    }

    public override void Update()
    {
        if (Vector3.Distance(player.position, agent.transform.position) <= distance)
        {
            fsm.ChangeState(STATE.Chase);
        }
        else
        {
            var randTime = Random.Range(3f,3f);
            if (Time.time > startTime + randTime)
            {
                fsm.ChangeState(STATE.Idle);
            }
        }
    }
}
