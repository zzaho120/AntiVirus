using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerFSM : FSM
{
    // Start is called before the first frame update
    void Start()
    {
        //AddState(new IdleState(this));
        AddState(new MoveState(this));
        ChangeState(STATE.Idle);
    }
}