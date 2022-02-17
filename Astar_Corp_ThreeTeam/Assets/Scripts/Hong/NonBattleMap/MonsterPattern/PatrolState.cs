using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateBase
{
    private float startTime;
    private bool isChase;
    private float randTimer;

    // 애니메이션 재생
    private bool isMoving;
    private Animator animator;

    // 몬스터별 수치
    private float moveRange;    // 이동 범위 설정
    private float distance;    // 플레이어, 몬스터간 거리
    private int atkChance; 

    private Vector3 targetPos;
    private Vector3 startPos;

    private Transform player;
    private NavMeshAgent agent;
    private GameObject monsterZone;

    private WorldMonsterChar monster;

    public PatrolState(WorldMonsterChar monster, FSM fsm)
    {
        this.fsm = fsm;
        this.monster = monster;
        
        if (this.fsm != null)
        {
            player = GameObject.FindWithTag("Player").transform;

            agent = monster.GetComponent<NavMeshAgent>();
            animator = monster.GetComponent<Animator>();
            startPos = monster.transform.position;  // 시작 위치 저장
            agent.transform.position = startPos;
            monsterZone = monster.gameObject.transform.parent.GetChild(0).gameObject; // 상위 몬스터 영역 가져오기
            //Debug.Log(monsterZone.name);

            // 몬스터별 스탯 설정
            agent.speed = monster.monsterStat.nonBattleMonster.speed;
            moveRange = monster.monsterStat.nonBattleMonster.areaRange;
            distance = monster.monsterStat.nonBattleMonster.sightRange;
            atkChance = monster.monsterStat.nonBattleMonster.suddenAtkRate / 10;

            //// 영역 크기 설정
            if (monsterZone.transform.lossyScale.x != moveRange* 2)
            {
                monsterZone.transform.localScale = new Vector3(moveRange * 2, monsterZone.transform.localScale.y, moveRange * 2);
                //monsterZone.transform.parent.GetComponent<SphereCollider>().radius = moveRange;
            }

        }
    }

    public override void Enter()
    {
        agent.SetDestination(startPos);

        startTime = Time.time;
        randTimer = 0;

        // 랜덤 범위 설정
        var randX = Random.Range(-moveRange, moveRange);
        var randY = Random.Range(-moveRange, moveRange);

        // 타겟 위치 설정
        targetPos = startPos + new Vector3(randX, monster.transform.position.y, randY);

        // 허용 범위를 초과하면 타겟위치 = 시작위치
        if (Vector3.Distance(targetPos, startPos) > moveRange * 2)
        {
            //Debug.Log("Range Over");
            targetPos = startPos;
        }
    }

    public override void Exit()
    {
        //agent.SetDestination(startPos);
    }

    public override void Update()
    {
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("IsWalking", true);
        }
        if (agent.velocity == Vector3.zero)
        {
            animator.SetBool("IsWalking", false);
        }

        agent.SetDestination(targetPos);

        // 플레이어가 몹 인식범위(감지거리) 내로 들어왔을 때
        if (Vector3.Distance(player.position, monster.transform.position) <= distance)
        {
            randTimer += Time.deltaTime;

            // 2초에 한번씩 검사해서 Ture 되면 Chase 상태로 변경
            if (randTimer > 2.0f)
            {
                int randNum = Random.Range(0, 10);  // 0~9
                if (randNum >= atkChance)
                {
                    isChase = false;
                    //Debug.Log("노 추적");
                }
                else
                {
                    isChase = true;
                }

                if (isChase)
                {
                    fsm.ChangeState(STATE.Chase);
                }
                else
                {
                    fsm.ChangeState(STATE.Idle);
                }
            }
        }
        else
        {
            if (Time.time > startTime + 5f || Vector3.Distance(targetPos, monster.transform.position) < 0.3f)
            {
                fsm.ChangeState(STATE.Idle);
            }
        }
    }
}
