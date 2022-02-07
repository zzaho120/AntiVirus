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
    public int penaltyGauge; // Æä³ÎÆ¼¹Ù
    public int penaltyLevel;
    public int reductionGauge; // ³»¼º °æÇèÄ¡¹Ù
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
        Debug.Log($"{virus.name} ¹ÙÀÌ·¯½º Æä³ÎÆ¼ {newGauge}¸¦ ÀÔÇô¼­ {penaltyLevel}·¹º§ {penaltyGauge}°¡ µÆ½À´Ï´Ù.");

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
        Debug.Log($"{virus.name} ¹ÙÀÌ·¯½º Æä³ÎÆ¼ {newGauge}¸¦ ÀÔÇô¼­ {penaltyLevel}·¹º§ {penaltyGauge}°¡ µÆ½À´Ï´Ù.");

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

    // ³»¼º °ÔÀÌÁö¿¡ °Á ¾Æ¹«°Å³ª ³Ö¾îµÒ
    int resistGauge = 1;

    public void GetReductionExp(int reduceGauge)
    {
        if (reduceGauge < 0)
        {
            reductionGauge += (-reduceGauge);
            // ¦£¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¤
            // ¦¢       È«¼öÁø_½ºÅÈ¼öÁ¤       ¦¢
            // ¦¢      ³»¼º°ÔÀÌÁö DB »ý±è     ¦¢
            // ¦¦¦¡¦¡¦¡¦¡¦¡¦¡¦¡   ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¥
            //          \/

            //           £¯^  \
            //          / ^w ^ \
            //¡¡     _ / \ /    ¡¬ _
            //      / '/ - £Ù -  /  \
            //     (  (ß²' ìÑ /     |
            //     |  / ìÑ ¡¬ £þ£þ\ /
            //      \£ß£ß£ß>. £ß£ß_£¯
            //¡¡¡¡      £ü(èÝ /  ¡´
            //          / \' ¦¡_/\
            //          /  \_£¯¡¡ |
            //¡¡        £ü¡¡¡¡/ / /
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
        // ¦£¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¤
        // ¦¢       È«¼öÁø_½ºÅÈ¼öÁ¤       ¦¢
        // ¦¢      ³»¼º°ÔÀÌÁö DB »ý±è     ¦¢
        // ¦¦¦¡¦¡¦¡¦¡¦¡¦¡¦¡   ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¥
        //          \/

        //           £¯^  \
        //          / ^w ^ \
        //¡¡     _ / \ /    ¡¬ _
        //      / '/ - £Ù -  /  \
        //     (  (ß²' ìÑ /     |
        //     |  / ìÑ ¡¬ £þ£þ\ /
        //      \£ß£ß£ß>. £ß£ß_£¯
        //¡¡¡¡      £ü(èÝ /  ¡´
        //          / \' ¦¡_/\
        //          /  \_£¯¡¡ |
        //¡¡        £ü¡¡¡¡/ / /
        //return character.resistGauge * (reductionLevel + 1);
        return resistGauge * (reductionLevel + 1);
    }
}
