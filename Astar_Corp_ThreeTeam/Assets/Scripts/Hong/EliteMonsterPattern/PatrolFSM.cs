using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFSM : FSM
{
    // Start is called before the first frame update
    void Start()
    {
        AddState(new IdleState(this));          // State 1
        AddState(new PatrolState(this));        // State 2
        ChangeState(STATE.State1);              // 상태설정
    }

}
