using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassiveCase
{
    Ready,
    Hit,
    Move,
    Game,
    Reload,
    Attacked,
    FullApMove,
    isMoved
}

public enum Stat
{
    None = -1,
    Sight,
    Reduction,
    Mp,
    Aggro,
    Virus,
    Weight,
    FullApMove,
    Ammo,
    Evade,
    MeleeDamage,
    Accuracy,
    AASR,
    MoveSRAccuracy,
    MaxMPLimit,
    Damage,
    Hp
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
        Debug.Log(stat);
        buffMgr.Addbuff(new BuffBase(stat, increase, lifeTurn));
    }
}
