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
        startTime = Time.time;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        var randTime = Random.Range(3f, 5f);

        if (Time.time > startTime + randTime)
            fsm.ChangeState(STATE.State2);
            //fsm.ChangeState(STATE.Move);
    }
}
