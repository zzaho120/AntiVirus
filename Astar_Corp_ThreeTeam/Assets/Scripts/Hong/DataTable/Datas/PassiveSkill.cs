using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassiveCase
{
    Ready,
    Hit
}

public enum Stat
{
    None = -1,
    Sight,
    Reduction
}

public class PassiveSkill : SkillBase
{
    public PassiveCase skillCase;
    public Stat stat;
    public float increase;
    public float decrease;
    public int lifeTurn;

    public void Invoke(BuffMgr buffMgr)
    {
        buffMgr.Addbuff(new BuffBase(stat, increase, lifeTurn));
    }
}
