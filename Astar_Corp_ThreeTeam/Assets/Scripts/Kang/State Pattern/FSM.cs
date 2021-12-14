using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    Idle,
    Move
}

public class FSM : MonoBehaviour
{
    private List<StateBase> states = new List<StateBase>();
    private StateBase curState;

    public void Update()
    {
        if (curState != null)
            curState.Update();
    }

    public void AddState(StateBase state)
    {
        if (!states.Contains(state))
            states.Add(state);
    }

    public void ChangeState(STATE state)
    {
        if (curState != null)
            curState.Exit();

        var stateNum = (int)state;
        if (states.Count > stateNum)
        {
            curState = states[stateNum];
            curState.Enter();
        }

        Debug.Log(state);
    }
}
