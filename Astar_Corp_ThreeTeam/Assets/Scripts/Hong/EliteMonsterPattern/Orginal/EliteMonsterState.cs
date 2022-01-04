using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterState : StateBase
{
    public override void Enter()
    {
        Debug.Log("Enter");
    }

    public override void Exit()
    {
        Debug.Log("Exit");
    }

    public override void Update()
    {
        Debug.Log("Update");
    }
}
