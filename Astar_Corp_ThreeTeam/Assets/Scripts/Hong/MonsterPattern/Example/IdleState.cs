using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateBase
{
    private float startTime;

    public IdleState(FSM fsm)
    {
        this.fsm = fsm;
    }

    public override void Enter()
    {
        //Debug.Log("Idle");
        startTime = Time.time;
    }

    public override void Exit()
    {
        startTime = 0;
    }

    public override void Update()
    {
        var randTime = Random.Range(3f, 5f);

        if (Time.time > startTime + 2f)
        {
            fsm.ChangeState(STATE.Patrol);
            //Debug.Log("Change state to Patrol");
        }
    }
}
