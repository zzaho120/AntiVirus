using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassiveCase
{
    Ready,
}

public enum Stat
{
    None = -1,
    Sight,
    Reduction
}

public class PassiveSkill : SkillBase
{
    public string skillCase;
    public Stat stat;
    public float increase;
    public float decrease;
    public int lifeTurn;

    public void Invoke(BuffMgr buffMgr)
    {
        BuffBase buff = null;
        switch (stat)
        {
            case Stat.Sight:
                buff = new SightBuff(increase);
                break;
            case Stat.Reduction:
                break;
        }
        buffMgr.Addbuff(buff);
    }
}
