using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats 
{
    public event EventHandler OnStatsChanged;

    public static int STAT_MIN = 0;
    public static int STAT_MAX = 5;

    public enum StatType
    {
        E,
        B,
        P,
        I,
        T
    }

    private SingleStat eStat;
    private SingleStat bStat;
    private SingleStat pStat;
    private SingleStat iStat;
    private SingleStat tStat;

    public Stats(int eStatAmount, int bStatAmount, int pStatAmount,
        int iStatAmount, int tStatAmount)
    {
        eStat = new SingleStat(eStatAmount);
        bStat = new SingleStat(bStatAmount);
        pStat = new SingleStat(pStatAmount);
        iStat = new SingleStat(iStatAmount);
        tStat = new SingleStat(tStatAmount);
    }

    private SingleStat GetSingleStat(StatType statType)
    {
        switch (statType)
        {
            default:
            case StatType.E: return eStat;
            case StatType.B: return bStat;
            case StatType.P: return pStat;
            case StatType.I: return iStat;
            case StatType.T: return tStat;
        }
    }

    public void SetStatAmount(StatType statType, int statAmount)
    {
        GetSingleStat(statType).SetStatAmount(statAmount);
        if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
    }

    public int GetStatAmount(StatType statType)
    {
        return GetSingleStat(statType).GetStatAmount();
    }

    public float GetStatAmountNormalized(StatType statType)
    {
        return GetSingleStat(statType).GetStatAmountNormalized();
    }

    private class SingleStat
    {
        private int stat;

        public SingleStat(int statAmount)
        {
            SetStatAmount(statAmount);
        }

        public void SetStatAmount(int statAmount)
        {
            stat = Mathf.Clamp(statAmount, STAT_MIN, STAT_MAX);
        }

        public int GetStatAmount()
        {
            return stat;
        }

        public float GetStatAmountNormalized()
        {
            return (float)stat / STAT_MAX;
        }
    }
}
