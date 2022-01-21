using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightBuff : BuffBase
{
    public SightBuff(float amount, bool isIncrease = true)
        : base(amount, isIncrease, Stat.Sight) { }

    public override float GetAmount()
    {
        if (isIncrease)
            return amount;
        else
            return -amount;
    }
}