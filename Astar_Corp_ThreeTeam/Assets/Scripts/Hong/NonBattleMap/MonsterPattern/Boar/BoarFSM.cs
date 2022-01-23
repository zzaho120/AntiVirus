using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarFSM : FSM
{
    // Start is called before the first frame update
    void Start()
    {
        AddState(new Boar_IdleState(this));         // State 1
        AddState(new Boar_PatrolState(this));       // State 2
        AddState(new Boar_ChaseState(this));        // State 3
        ChangeState(STATE.Patrol);                    // 초기상태 설정
    }

}
