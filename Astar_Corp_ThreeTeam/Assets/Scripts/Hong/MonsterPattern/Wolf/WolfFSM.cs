using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfFSM : FSM
{
    // Start is called before the first frame update
    void Start()
    {
        AddState(new Wolf_IdleState(this));         // State 1
        AddState(new Wolf_PatrolState(this));       // State 2
        AddState(new Wolf_ChaseState(this));        // State 3
        ChangeState(STATE.Idle);                    // 초기상태 설정
    }
}
