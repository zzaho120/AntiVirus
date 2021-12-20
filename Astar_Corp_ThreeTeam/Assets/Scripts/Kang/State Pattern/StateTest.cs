using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class MoveState : StateBase
public class StateTest : StateBase
{
    private float speed = 3f;
    private Rigidbody rigid;
    private float startTime;

    //public MoveState(FSM fsm)
    public StateTest(FSM fsm)
    {
        this.fsm = fsm;

        if (this.fsm != null)
            rigid = fsm.GetComponent<Rigidbody>();

    }
    public override void Enter()
    {
        startTime = Time.time;
    }

    public override void Exit()
    {
        if (rigid != null)
            rigid.velocity = Vector3.zero;
    }

    public override void Update()
    {
        if (rigid != null)
            rigid.velocity = new Vector3(speed * 3f * Time.fixedDeltaTime, 0f, 0f);

        if (Time.time > startTime + 3f)
            fsm.ChangeState(STATE.State1);
            //fsm.ChangeState(STATE.Idle);
    }
}