using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateBase
{
    private float startTime;
    private bool isChase;
    private float randTimer;

    // �ִϸ��̼� ���
    private bool isMoving;
    private Animator animator;

    // ���ͺ� ��ġ
    private float moveRange;    // �̵� ���� ����
    private float distance;    // �÷��̾�, ���Ͱ� �Ÿ�
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
            startPos = monster.transform.position;  // ���� ��ġ ����
            agent.transform.position = startPos;
            monsterZone = monster.gameObject.transform.parent.GetChild(0).gameObject; // ���� ���� ���� ��������
            //Debug.Log(monsterZone.name);

            // ���ͺ� ���� ����
            agent.speed = monster.monsterStat.nonBattleMonster.speed;
            moveRange = monster.monsterStat.nonBattleMonster.areaRange;
            distance = monster.monsterStat.nonBattleMonster.sightRange;
            atkChance = monster.monsterStat.nonBattleMonster.suddenAtkRate / 10;

            //// ���� ũ�� ����
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

        // ���� ���� ����
        var randX = Random.Range(-moveRange, moveRange);
        var randY = Random.Range(-moveRange, moveRange);

        // Ÿ�� ��ġ ����
        targetPos = startPos + new Vector3(randX, monster.transform.position.y, randY);

        // ��� ������ �ʰ��ϸ� Ÿ����ġ = ������ġ
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

        // �÷��̾ �� �νĹ���(�����Ÿ�) ���� ������ ��
        if (Vector3.Distance(player.position, monster.transform.position) <= distance)
        {
            randTimer += Time.deltaTime;

            // 2�ʿ� �ѹ��� �˻��ؼ� Ture �Ǹ� Chase ���·� ����
            if (randTimer > 2.0f)
            {
                int randNum = Random.Range(0, 10);  // 0~9
                if (randNum >= atkChance)
                {
                    isChase = false;
                    //Debug.Log("�� ����");
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
