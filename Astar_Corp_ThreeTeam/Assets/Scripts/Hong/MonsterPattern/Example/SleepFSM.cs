using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepFSM : FSM
{
    void Start()
    {
        // Idle ���·� �����ؼ� ���� sleep ���·�
         AddState(new IdleState(this));     
         AddState(new SleepState(this));    
         ChangeState(STATE.Idle);           
    }

}
