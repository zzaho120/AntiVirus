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
    public int penaltyGauge; // 페널티바
    public int penaltyLevel;
    public int reductionGauge; // 내성 경험치바
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
        reductionLevel = 0;
    }

    public void Calculation(int virusLevel)
    {
        penaltyGauge += (GetPenaltyGauge(virusLevel) - CurrentReduction);
        Debug.Log($"{virus.name} 바이러스 페널티 {GetPenaltyGauge(virusLevel) - CurrentReduction}를 입혀서 {penaltyLevel}레벨 {penaltyGauge}가 됐습니다.");
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
}
