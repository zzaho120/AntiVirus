using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateBase
{
    private float startTime;
    private float moveRange = 5f;  // �̵� ���� ����

    private Vector3 targetPos;
    private Vector3 startPos;

    private Transform player;
    private NavMeshAgent agent;

    //public PatrolState(FSM fsm)
    public PatrolState(PatrolFSM fsm)
    {
        this.fsm = fsm;

        if (this.fsm != null)
        {
            agent = fsm.GetComponent<NavMeshAgent>();
            startPos = fsm.transform.position;  // ���� ��ġ ����

            player = GameObject.FindWithTag("Player").transform;
        }
    }

    public override void Enter()
    {
        // Debug.Log(startPos);

        startTime = Time.time;

        // ���� ���� ����
        var randX = Random.Range(-5f, 5f);
        var randY = Random.Range(-5f, 5f);

        // Ÿ�� ��ġ ����
        targetPos = startPos + new Vector3(randX, fsm.transform.position.y, randY);

        // ��� ������ �ʰ��ϸ� Ÿ����ġ = ������ġ
        if (Vector3.Distance(targetPos, startPos) > moveRange)
        {
            //Debug.Log("Range Over");
            targetPos = startPos;
        }

        // ����޽� �̵�
        agent.SetDestination(targetPos);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        var randTime = Random.Range(0f, 5f);

        if (Time.time > startTime + randTime)
        {
            fsm.ChangeState(STATE.Idle);
        }
    }
}
