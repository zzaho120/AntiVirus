using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowChaseFSM : FSM
{
    void Start()
    {
        AddState(new Chase_IdleState(this));        // State 1
        AddState(new Chase_PatrolState(this));      // State 2
        AddState(new LowChaseState(this));          // State 3
        ChangeState(STATE.Idle);                    // 초기상태 설정
    }
}
