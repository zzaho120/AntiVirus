using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffBase
{
    public Stat stat;
    public float amount;
    public bool isIncrease;

    public BuffBase(float amount, bool isIncrease = true, Stat stat = Stat.None)
    {
        this.stat = stat;
        this.amount = amount;
        this.isIncrease = isIncrease;
    }

    public abstract float GetAmount();
}
