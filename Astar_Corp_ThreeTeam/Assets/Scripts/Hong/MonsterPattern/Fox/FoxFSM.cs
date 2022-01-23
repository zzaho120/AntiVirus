using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFSM : FSM
{
    // Start is called before the first frame update
    void Start()
    {
        AddState(new Fox_IdleState(this));         // State 1
        AddState(new Fox_PatrolState(this));       // State 2
        AddState(new Fox_ChaseState(this));        // State 3
        ChangeState(STATE.Idle);                    // 초기상태 설정
    }
}
