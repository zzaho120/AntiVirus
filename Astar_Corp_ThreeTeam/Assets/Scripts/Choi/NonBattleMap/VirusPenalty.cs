using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VirusType
{
    None = -1,
    E,
    B,
    P,
    I,
    T
}

public class VirusPenalty
{
    public Virus virus;
    public int penaltyGauge; // ���Ƽ��
    public int penaltyLevel;
    public int reductionGauge; // ���� ����ġ��
    public int reductionLevel;

    private readonly int expGauge = 100; 
    private readonly int maxLevel = 5; 

    public int CurrentReduction
    {
        get => virus.resistDec * reductionLevel;
    }
    
    public int GetPenaltyGauge(int paneltyLevel)
    {
        return virus.resistCharge * paneltyLevel;
    }

    public VirusPenalty(Virus virus)
    {
        this.virus = virus;
        penaltyGauge = 0;
        penaltyLevel = 0;
        reductionLevel = 1;
    }

    public void Calculation(int virusLevel)
    {
        penaltyGauge += (GetPenaltyGauge(virusLevel) - CurrentReduction);
        Debug.Log($"{virus.name} ���̷��� ���Ƽ {GetPenaltyGauge(virusLevel) - CurrentReduction}�� ������ {penaltyLevel}���� {penaltyGauge}�� �ƽ��ϴ�.");
        var maxGauge = expGauge * (penaltyLevel + 1);
        if (penaltyGauge >= maxGauge)
        {
            if (penaltyLevel != maxLevel)
            {
                penaltyGauge -= maxGauge;
                penaltyLevel++;
            }
            else if (penaltyLevel == maxLevel)
                penaltyGauge = maxGauge;
        }
    }

    public void ReductionCalculation(int gauge)
    {
        penaltyGauge -= gauge;
        var previousMax = expGauge * penaltyLevel;
        if (penaltyGauge < 0)
        {
            if (penaltyLevel != 1)
            {
                penaltyGauge += previousMax;
                penaltyLevel--;
            }
            else if (penaltyLevel == 1)
            {
                penaltyGauge = 0;
                penaltyLevel = 0;
            }
        }
    }

    public void GetReductionExp()
    {
        reductionGauge += virus.exp * penaltyLevel;
        var maxGauge = expGauge * (reductionLevel + 1);
        if (reductionGauge >= maxGauge)
        {
            if (reductionLevel != maxLevel)
            {
                reductionGauge -= maxGauge;
                reductionLevel++;
            }
            else if (reductionLevel == maxLevel)
                penaltyGauge = maxGauge;
        }
    }

    public int GetMaxGauge()
    { 
        return expGauge * (penaltyLevel + 1);
    }
}
