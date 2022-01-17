using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    Idle,
    Move,
    Chase

    //State1,
    //State2,
    //State3
}

public class FSM : MonoBehaviour
{
    private List<StateBase> states = new List<StateBase>();
    private StateBase curState;

    public void Update()
    {
        if (curState != null)
            curState.Update();
        //Debug.Log(states.Count);
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

        //Debug.Log("state : " + curState);
    }

    public void ChangeState(int state)
    {
        if (curState != null)
            curState.Exit();

        var stateNum = state;
        if (states.Count > stateNum)
        {
            curState = states[stateNum];
            curState.Enter();
        }

        //Debug.Log("state : " + curState);
    }
}
