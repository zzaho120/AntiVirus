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
    public int penaltyGauge; // む割じ夥
    public int penaltyLevel;
    public int reductionGauge; // 頂撩 唳я纂夥
    public int reductionLevel;

    private readonly int expGauge = 100; 
    private readonly int maxLevel = 5;

    public int CurrentReduction
    {
        get
        {
            if (reductionLevel == 0)
                return character.virusDec_Lev0;
            else
                return character.virusDec_Lev1 * reductionLevel;
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
        reductionLevel = 0;
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
        Debug.Log($"{virus.name} 夥檜楝蝶 む割じ {newGauge}蒂 殮⑵憮 {penaltyLevel}溯漣 {penaltyGauge}陛 腑蝗棲棻.");

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
        Debug.Log($"{virus.name} 夥檜楝蝶 む割じ {newGauge}蒂 殮⑵憮 {penaltyLevel}溯漣 {penaltyGauge}陛 腑蝗棲棻.");

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

    // 頂撩 啪檜雖縑 偭 嬴鼠剪釭 厥橫菸
    int resistGauge = 1;

    public void GetReductionExp(int reduceGauge)
    {
        if (reduceGauge < 0)
        {
            reductionGauge += (-reduceGauge);
            // 忙式式式式式式式式式式式式式式式式式式式式式式式式式式式式忖
            // 弛       �姨鶬鱌蝶囌熱薑       弛
            // 弛      頂撩啪檜雖 DB 儅梯     弛
            // 戌式式式式式式式   式式式式式式式式式式式式式式式式式式戎
            //          \/

            //           ㄞ^  \
            //          / ^w ^ \
            //﹛     _ / \ /    ′ _
            //      / '/ - �� -  /  \
            //     (  (葀' 嬣 /     |
            //     |  / 嬣 ′ ����\ /
            //      \�舝舝�>. �舝絻ㄞ
            //﹛﹛      ��(魦 /  ●
            //          / \' 式_/\
            //          /  \_ㄞ﹛ |
            //﹛        ��﹛﹛/ / /
            //var maxGauge = character.resistGauge * (reductionLevel + 1);
            var maxGauge = resistGauge * (reductionLevel + 1);
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

    public int GetMaxGauge()
    { 
        return expGauge * (penaltyLevel + 1);
    }

    public int GetMaxReductionGauge()
    {
        // 忙式式式式式式式式式式式式式式式式式式式式式式式式式式式式忖
        // 弛       �姨鶬鱌蝶囌熱薑       弛
        // 弛      頂撩啪檜雖 DB 儅梯     弛
        // 戌式式式式式式式   式式式式式式式式式式式式式式式式式式戎
        //          \/

        //           ㄞ^  \
        //          / ^w ^ \
        //﹛     _ / \ /    ′ _
        //      / '/ - �� -  /  \
        //     (  (葀' 嬣 /     |
        //     |  / 嬣 ′ ����\ /
        //      \�舝舝�>. �舝絻ㄞ
        //﹛﹛      ��(魦 /  ●
        //          / \' 式_/\
        //          /  \_ㄞ﹛ |
        //﹛        ��﹛﹛/ / /
        //return character.resistGauge * (reductionLevel + 1);
        return resistGauge * (reductionLevel + 1);
    }
}
