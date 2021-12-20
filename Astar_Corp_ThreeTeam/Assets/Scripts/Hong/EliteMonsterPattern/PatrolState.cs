using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EliteMonsterState
{
    private float startTime;
    private float moveRange = 10f;  // �̵� ���� ����

    private Vector3 targetPos;
    private Vector3 startPos;
    
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
            targetPos = startPos;
            Debug.Log("Range Over");
        }

        // ����޽� �̵�
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
