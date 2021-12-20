using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStats
{
    private int damage;
    public  int Damage { get => damage; }

    private bool critical;
    public  bool Critical { get => critical; }

    public AttackStats(int damage, bool critical)
    {
        this.damage = damage;
        this.critical = critical;
    }
}
