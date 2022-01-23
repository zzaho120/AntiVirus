using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase
{
    public Stat stat;
    private float amount;
    private bool isIncrease;
    private int maxLifeTurn;
    private int currentLifeTurn;

    public bool IsOverLifeTurn
    {
        get => maxLifeTurn <= currentLifeTurn;
    }

    public BuffBase(Stat stat, float amount, int lifeTurn, bool isIncrease = true)
    {
        this.stat = stat;
        this.amount = amount;
        this.isIncrease = isIncrease;
        maxLifeTurn = lifeTurn;
        currentLifeTurn = 0;
    }

    public float GetAmount()
    {
        if (isIncrease)
            return amount;
        else
            return -amount;
    }

    public void StartTurn()
    {
        currentLifeTurn++;
        Debug.Log(currentLifeTurn);
    }
}