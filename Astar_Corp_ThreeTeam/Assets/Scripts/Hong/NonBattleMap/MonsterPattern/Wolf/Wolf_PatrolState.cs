using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf_PatrolState : StateBase
{
    private float startTime;
    private bool isChase;
    private float randTimer;

    private float moveRange = Constants.wolfRange;   // 이동 범위 설정
    private float distance  = Constants.wolfDistance;    // 플레이어, 몬스터간 거리
    private int atkChance = 6;

    private Vector3 targetPos;
    private Vector3 startPos;

    private Transform player;
    private NavMeshAgent agent;

    public Wolf_PatrolState(FSM fsm)
    {
        this.fsm = fsm;

        if (this.fsm != null)
        {
            agent = fsm.GetComponent<NavMeshAgent>();
            startPos = fsm.transform.position;  // 시작 위치 저장
            //agent.transform.position = startPos;

            player = GameObject.FindWithTag("Player").transform;
        }
    }

    public override void Enter()
    {
        //// Debug.Log(startPos);
        //agent.SetDestination(startPos);
        //
        //startTime = Time.time;
        //randTimer = 0;
        //
        //// 랜덤 범위 설정
        //var randX = Random.Range(-moveRange, moveRange);
        //var randY = Random.Range(-moveRange, moveRange);
        //
        //// 타겟 위치 설정
        //targetPos = startPos + new Vector3(randX, fsm.transform.position.y, randY);
        //
        //// 허용 범위를 초과하면 타겟위치 = 시작위치
        //if (Vector3.Distance(targetPos, startPos) > moveRange * 2)
        //{
        //    //Debug.Log("Range Over");
        //    targetPos = startPos;
        //}
        //
        //// 내비메쉬 이동
        /////agent.SetDestination(targetPos);
        ////agent.SetDestination(agent.transform.position);
    }

    public override void Exit()
    {
        //agent.SetDestination(startPos);
    }

    public override void Update()
    {
        //agent.SetDestination(targetPos);
        //
        //// 플레이어가 몹 인식범위 내로 들어왔을 때
        //if (Vector3.Distance(player.position, fsm.transform.position) <= distance)
        //{
        //    randTimer += Time.deltaTime;
        //
        //    // 2초에 한번씩 검사해서 Ture 되면 Chase 상태로 변경
        //    if (randTimer > 2.0f)
        //    {
        //        int randNum = Random.Range(0, 10);
        //        if (randNum > atkChance - 1)
        //        {
        //            isChase = false;
        //            //Debug.Log("노 추적");
        //        }
        //        else
        //        {
        //            isChase = true;
        //        }
        //
        //        if (isChase)
        //        {
        //            fsm.ChangeState(STATE.Chase);
        //        }
        //        else
        //        {
        //            fsm.ChangeState(STATE.Idle);
        //        }
        //    }
        //}
        //else
        //{
        //    if (Time.time > startTime + 5f || Vector3.Distance(targetPos, fsm.transform.position) < 0.3f)
        //    {
        //        fsm.ChangeState(STATE.Idle);
        //    }
        //}
        //
        ////if (Vector3.Distance(player.position, fsm.transform.position) <= distance)
        ////if (isChase)
        ////{
        ////    fsm.ChangeState(STATE.Chase);
        ////}
        ////else
        ////{
        ////    fsm.ChangeState(STATE.Idle);
        ////
        ////    //var randTime = Random.Range(3f, 3f);
        ////    //if (Time.time > startTime + randTime)
        ////    //{
        ////    //    fsm.ChangeState(STATE.Idle);
        ////    //}
        ////}
    }
}
