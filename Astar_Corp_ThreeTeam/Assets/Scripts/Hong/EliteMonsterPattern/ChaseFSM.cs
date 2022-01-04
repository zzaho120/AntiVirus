using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseFSM : FSM
{
    public int rand;

    void Start()
    {
        //AddState(new IdleState(this));          // State 1
        //AddState(new PatrolState(this));        // State 2
        //AddState(new ChaseState(this));         // State 3
        //ChangeState(STATE.Chase);              // 상태설정
    }
}
