using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFSM : FSM
{
    // Start is called before the first frame update
    void Start()
    {
        AddState(new Spider_IdleState(this));         // State 1
        AddState(new Spider_PatrolState(this));       // State 2
        AddState(new Spider_ChaseState(this));        // State 3
        ChangeState(STATE.Patrol);                    // 초기상태 설정
    }
}
