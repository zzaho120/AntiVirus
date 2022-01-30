using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMonsterFSM : FSM
{
    public WorldMonsterChar monster;

    public void Init(WorldMonsterChar monster)
    {
        this.monster = monster;
        AddState(new IdleState(monster, this));
        AddState(new PatrolState(monster, this));
        AddState(new ChaseState(monster, this));

        ChangeState(STATE.Patrol);
    }
}
