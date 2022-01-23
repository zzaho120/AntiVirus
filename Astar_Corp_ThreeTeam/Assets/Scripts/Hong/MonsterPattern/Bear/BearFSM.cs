using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearFSM : FSM
{
    // Start is called before the first frame update
    void Start()
    {
        AddState(new Bear_IdleState(this));         // State 1
        AddState(new Bear_PatrolState(this));       // State 2
        AddState(new Bear_ChaseState(this));        // State 3
        ChangeState(STATE.Idle);                    // 초기상태 설정
    }
}
