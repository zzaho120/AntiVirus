using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase_PatrolState : StateBase
{
    private float startTime;
    private float moveRange = 5f;   // �̵� ���� ����
    private float distance = Constants.disance;    // �÷��̾�, ���Ͱ� �Ÿ�

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
