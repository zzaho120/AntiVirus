using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerFSM : FSM
{
    // Start is called before the first frame update
    void Start()
    {
        AddState(new Tiger_IdleState(this));         // State 1
        AddState(new Tiger_PatrolState(this));       // State 2
        AddState(new Tiger_ChaseState(this));        // State 3
        ChangeState(STATE.Idle);                    // 초기상태 설정
    }
}
