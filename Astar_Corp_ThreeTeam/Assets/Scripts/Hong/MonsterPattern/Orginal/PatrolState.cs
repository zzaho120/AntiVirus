using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateBase
{
    private float startTime;
    private float moveRange = 5f;  // �̵� ���� ����
    private float speed = 0.5f;

    private Vector3 startPos, targetPos;
    private NavMeshAgent agent;

    public PatrolState(FSM fsm)
    {
        this.fsm = fsm;

        if (this.fsm != null)
        {
            agent = fsm.GetComponent<NavMeshAgent>();
            startPos = fsm.transform.position;  // ���� ��ġ ����
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
       agent.SetDestination(startPos);
    }

    public override void Update()
    {
        var randTime = Random.Range(0f, 2f);

        if (Time.time > startTime + 2f)
        {
            fsm.ChangeState(STATE.Idle);
            //Debug.Log("Change State to Idle");
        }
    }

}
