using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EliteMonsterState
{
    private float startTime;
    private float moveRange = 10f;  // 이동 범위 설정

    private Vector3 targetPos;
    private Vector3 startPos;
    
    private NavMeshAgent agent;


    public PatrolState(FSM fsm)
    {
        this.fsm = fsm;

        if (this.fsm != null)
        {
            agent = fsm.GetComponent<NavMeshAgent>();
            startPos = fsm.transform.position;  // 시작 위치 저장
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
            targetPos = startPos;
            Debug.Log("Range Over");
        }

        // 내비메쉬 이동
        agent.SetDestination(targetPos);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (Time.time > startTime + 3)
            fsm.ChangeState(STATE.State1);
    }
}
