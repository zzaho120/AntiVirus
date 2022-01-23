using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepFSM : FSM
{
    void Start()
    {
        // Idle 상태로 진입해서 무한 sleep 상태로
         AddState(new IdleState(this));     
         AddState(new SleepState(this));    
         ChangeState(STATE.Idle);           
    }

}
