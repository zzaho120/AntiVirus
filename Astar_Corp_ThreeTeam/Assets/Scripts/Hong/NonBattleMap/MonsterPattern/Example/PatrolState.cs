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

        Debug.Log(fsm.gameObject.name);
    }

    public override void Enter()
    {
        startTime = Time.time;

        // ���� ���� ����
        var randX = Random.Range(-5f, 5f);
        var randY = Random.Range(-5f, 5f);

        // Ÿ�� ��ġ ����
        //targetPos = startPos + new Vector3(randX, fsm.transform.position.y, randY);
        targetPos = startPos + new Vector3(randX, 0f, randY);

        // ��� ������ �ʰ��ϸ� Ÿ����ġ = ������ġ
        if (Vector3.Distance(targetPos, startPos) > moveRange)
        {
            //Debug.Log("Range Over");
            targetPos = startPos;
        }
        //Debug.Log(targetPos);

        //NavMeshHit hitInfo;
        //if (NavMesh.SamplePosition(fsm.transform.position, out hitInfo, 3f, NavMesh.AllAreas))
        //{
        //    Debug.Log(hitInfo.mask);
        //    agent.SetDestination(targetPos);
        //}

        //// ����޽� �̵�
        //Debug.Log(agent.gameObject.name);
        agent.SetDestination(targetPos);
    }

    public override void Exit()
    {
       agent.SetDestination(startPos);
    }

    public override void Update()
    {
        var randTime = Random.Range(1.5f, 3f);

        if (Time.time > startTime + 2f)
        {
            fsm.ChangeState(STATE.Idle);
            //Debug.Log("Change State to Idle");
        }
    }

}
