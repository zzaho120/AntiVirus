using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_IdleState : StateBase
{
    private float distance = Constants.disance;
    private float startTime;

    private Transform player;
    private Transform monster;

    public Chase_IdleState(FSM fsm)
    {
        this.fsm = fsm;

        player = GameObject.FindWithTag("Player").transform;
        monster = fsm.GetComponent<Transform>();
    }

    public override void Enter()
    {
        startTime = Time.time;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (Vector3.Distance(player.position, monster.transform.position) <= distance)
        {
            fsm.ChangeState(STATE.Chase);
        }
        else
        {
            var randTime = Random.Range(3f, 3f);
            if (Time.time > startTime + randTime)
            {
                fsm.ChangeState(STATE.Move);
            }
        }
    }
}