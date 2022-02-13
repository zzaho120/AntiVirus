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

public enum VirusPenalyType
{
    None,
    DmgDec,
    HpDec,
    MpDec,
    AccuracyDec,
    All
}

public class VirusPenalty
{
    public Virus virus;
    public Character character;
    public int penaltyGauge; // 페널티바
    public int penaltyLevel;
    public int resistGauge; // 내성 현재 경험치 바
    public int resistLevel;

    private readonly int expGauge = 100; 
    private readonly int maxLevel = 5;

    private int MaxResistGauge
    {
        get
        {
            var resistExp = ScriptableMgr.Instance.GetResistExp($"RES_{resistLevel + 1}");
            return resistExp.exp;
        }
    }
    public int CurrentReduction
    {
        get
        {
            if (resistLevel == 0)
                return character.virusDec_Lev0;
            else
                return character.virusDec_Lev1 * resistLevel;
        }
    }

    public int GetPenaltyGauge(int paneltyLevel)
    {
        return virus.virusGauge * paneltyLevel;
    }

    public VirusPenalty(Virus virus, Character character)
    {
        this.virus = virus;
        this.character = character;
        penaltyGauge = 0;
        penaltyLevel = 0;
        resistLevel = 0;
    }

    public void Calculation(int virusLevel, float virusBuff = 1f, float reductionBuff = 1f)
    {
        var newGauge = (int)((GetPenaltyGauge(virusLevel) * virusBuff) - (CurrentReduction * reductionBuff));
        penaltyGauge += newGauge;

        if (newGauge < 0)
        {
            if (penaltyGauge < 0)
                GetReductionExp(newGauge - penaltyGauge);
            else
                GetReductionExp(newGauge);
        }

        penaltyGauge = Mathf.Clamp(penaltyGauge, 0, GetMaxGauge());
        Debug.Log($"{virus.name} 바이러스 페널티 {newGauge}를 입혀서 {penaltyLevel}레벨 {penaltyGauge}가 됐습니다.");

        CheckPenaltyGauge();
    }

    public void Calculation(int virusLevel, int virusAmount)
    {
        var newGauge = virusAmount * virusLevel - CurrentReduction;
        penaltyGauge += newGauge;

        if (newGauge < 0)
        {
            if (penaltyGauge < 0)
                GetReductionExp(newGauge - penaltyGauge);
            else
                GetReductionExp(newGauge);
        }

        penaltyGauge = Mathf.Clamp(penaltyGauge, 0, GetMaxGauge());
        Debug.Log($"{virus.name} 바이러스 페널티 {newGauge}를 입혀서 {penaltyLevel}레벨 {penaltyGauge}가 됐습니다.");

        CheckPenaltyGauge();
    }

    private void CheckPenaltyGauge()
    {
        var maxGauge = GetMaxGauge();
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
        else if (penaltyGauge < 0)
        {
            if (penaltyLevel > 0)
            {
                penaltyGauge += expGauge * penaltyLevel;
                penaltyLevel--;
            }
            else
                penaltyGauge = 0;
        }
    }

    public void ReductionCalculation(int gauge)
    {
        penaltyGauge -= gauge;
        if (penaltyGauge < 0)
        {
            if (penaltyLevel != 1)
            {
                penaltyLevel--;
                penaltyGauge += MaxResistGauge;
            }
            else if (penaltyLevel == 1)
            {
                penaltyGauge = 0;
                penaltyLevel = 0;
            }
        }
    }

   


    public void GetReductionExp(int reduceGauge)
    {
        if (reduceGauge < 0)
        {
            resistGauge += (-reduceGauge);
            // ┌────────────────────────────┐
            // │       홍수진_스탯수정       │
            // │      내성게이지 DB 생김     │
            // └───────   ──────────────────┘
            //          \/

            //           ／^  \
            //          / ^w ^ \
            //　     _ / \ /    ＼ _
            //      / '/ - Ｙ -  /  \
            //     (  (三' 人 /     |
            //     |  / 人 ＼ ￣￣\ /
            //      \＿＿＿>. ＿＿_／
            //　　      ｜(王 /  〈
            //          / \' ─_/\
            //          /  \_／　 |
            //　        ｜　　/ / /
            //var maxGauge = character.resistGauge * (reductionLevel + 1);
            var maxGauge = MaxResistGauge;
            if (resistGauge >= maxGauge)
            {
                if (resistLevel != maxLevel)
                {
                    resistGauge -= maxGauge;
                    resistLevel++;
                }
                else if (resistLevel == maxLevel)
                    penaltyGauge = maxGauge;
            }
        }
    }

    public int GetMaxGauge()
    { 
        return expGauge * (penaltyLevel + 1);
    }

    public int GetMaxReductionGauge()
    {
        return MaxResistGauge;
    }
}
