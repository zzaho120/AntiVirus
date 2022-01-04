using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerFSM : FSM
public class FsmTest : FSM
{
    void Start()
    {
        AddState(new IdleState(this));      // State 1
        AddState(new StateTest(this));      // State 2
        ChangeState(STATE.Idle);            // 상태설정
    }
}