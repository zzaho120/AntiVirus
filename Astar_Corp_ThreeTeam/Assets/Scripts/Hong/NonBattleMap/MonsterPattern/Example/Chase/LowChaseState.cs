using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LowChaseState : StateBase
{
    private float distance = Constants.wolfDistance;
    private bool isChase;

    private NavMeshAgent agent;
    private Transform player;

    public LowChaseState(FSM fsm)
    {
        this.fsm = fsm;

        if (this.fsm != null)
        {
            agent = fsm.GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
        }
    }
    public override void Enter()
    {
        //startTime = Time.time;


        // Update��
        int randNum = Random.Range(0, 100);
        if (randNum > 2)
        {
            isChase = false;
            //Debug.Log("�� ����");
        }
        else
        {
            isChase = true;
        }
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        // �÷��̾� ����
        if (isChase)
        {
            if (Vector3.Distance(player.position, agent.transform.position) <= distance)
            {
                agent.SetDestination(player.position);
                Debug.Log("����");
            }
            else
            {
                fsm.ChangeState(STATE.Idle);
                Debug.Log("�� ����");
            }
        }
        // ���� X
        else
        {
            fsm.ChangeState(STATE.Patrol);
            //Debug.Log("�� ����");
        }
    }
}
