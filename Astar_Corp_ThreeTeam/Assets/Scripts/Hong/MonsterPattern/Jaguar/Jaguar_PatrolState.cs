using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jaguar_PatrolState : StateBase
{
    private float startTime;
    private bool isChase;
    private float randTimer;

    private float moveRange = Constants.jaguarRange;   // �̵� ���� ����
    private float distance = Constants.jaguarDistance;    // �÷��̾�, ���Ͱ� �Ÿ�
    private int atkChance = 7;

    private Vector3 targetPos;
    private Vector3 startPos;

    private Transform player;
    private NavMeshAgent agent;

    public Jaguar_PatrolState(FSM fsm)
    {
        this.fsm = fsm;

        if (this.fsm != null)
        {
            agent = fsm.GetComponent<NavMeshAgent>();
            startPos = fsm.transform.position;  // ���� ��ġ ����
            agent.transform.position = startPos;

            player = GameObject.FindWithTag("Player").transform;
        }
    }

    public override void Enter()
    {
        //agent.transform.position = startPos;
        // Debug.Log(startPos);
        agent.SetDestination(startPos);

        startTime = Time.time;
        randTimer = 0;

        // ���� ���� ����
        var randX = Random.Range(-moveRange, moveRange);
        var randY = Random.Range(-moveRange, moveRange);

        // Ÿ�� ��ġ ����
        targetPos = startPos + new Vector3(randX, fsm.transform.position.y, randY);

        // ��� ������ �ʰ��ϸ� Ÿ����ġ = ������ġ
        if (Vector3.Distance(targetPos, startPos) > moveRange * 3)
        {
            //Debug.Log("Range Over");
            targetPos = startPos;
        }

        // ����޽� �̵�
        //agent.SetDestination(agent.transform.position);
    }

    public override void Exit()
    {
        //agent.SetDestination(startPos);
    }

    public override void Update()
    {
        agent.SetDestination(targetPos);

        // �÷��̾ �� �νĹ��� ���� ������ ��
        if (Vector3.Distance(player.position, fsm.transform.position) <= distance)
        {
            randTimer += Time.deltaTime;

            // 2�ʿ� �ѹ��� �˻��ؼ� Ture �Ǹ� Chase ���·� ����
            if (randTimer > 2.0f)
            {
                int randNum = Random.Range(0, 10);
                if (randNum > atkChance - 1)
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
            if (Time.time > startTime + 5f || Vector3.Distance(targetPos, fsm.transform.position) < 0.3f)
            {
                fsm.ChangeState(STATE.Idle);
            }
        }
    }
}
