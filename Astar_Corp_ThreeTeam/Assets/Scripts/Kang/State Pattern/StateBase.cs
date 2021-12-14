using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    protected FSM fsm;
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}