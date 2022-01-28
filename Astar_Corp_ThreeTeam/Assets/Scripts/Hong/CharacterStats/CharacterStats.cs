using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 스탯, 무기, 스킬

// 캐릭터 스탯들
// 1. 체력
// 2. 무게
// 3. 예민함
// 4. 회피율
// 5. 집중력
// 6. 정신력
// 7. 치명타 확률
// 8. 게이지 관련 -> 경험치, 내성 게이지, 바이러스 게이지 감소량
// 9. 사용 가능 무기들

public class CharacterStats
{
    // 기본 스탯
    public Character character;

    // 무기 스탯
    public WeaponStats weapon;

    // 스킬
    public SkillMgr skillMgr;

    // 버프
    public BuffMgr buffMgr;

    // 1. 기본 체력
    public int MaxHp; // { get => Hp; set => Hp = value; }
    [HideInInspector]
    public int currentHp;

    // 2. 무게
    public int Weight;

    // 3. 예민함
    [HideInInspector]
    public int sensivity;

    // 4. 회피율
    [HideInInspector]
    public int avoidRate;

    // 5. 집중력
    [HideInInspector]
    public int concentration;

    // 6. 집중력에 의한 추가 명중률
    [HideInInspector]
    public int accuracy;

    // 7. 정신력
    [HideInInspector]
    public int willpower;

    // 8. 치명타 확률
    [HideInInspector]
    public int critRate;

    // 9. 경계 명중률 증가량
    public int alertAccuracy;

    // 10. 크리티컬 저항율
    public int critResistRate;

    // 11. 시야 범위
    public int sightDistance;
    // 레벨
    [HideInInspector]
    public int level;
    private readonly int MaxLevel = 12;

    // 현재 경험치 양과 경험치 요구량
    [HideInInspector]
    public int currentExp;
    [HideInInspector]
    public int totalExp;

    //바이러스 패널티 및 내성
    [HideInInspector]
    public Dictionary<string, VirusPenalty> virusPenalty = new Dictionary<string, VirusPenalty>();

    //가방.
    public Dictionary<string, int> bag = new Dictionary<string, int>();
    public int bagLevel;
    public int currentWeight;

    //json 위치.
    public int saveId;

    public string Name 
    { 
        get { return character.name; } 
    }

    #region 캐릭터 기본 스탯 설정
    private int Hp
    {
        get
        {
            //Debug.Log(character);
            var baseHp = Random.Range(character.minHp, character.maxHp + 1);
            var hp = baseHp;
            return hp;
        }
        set
        {
            Hp = value;
        }
    }

    private int Sensivity
    {
        get
        {
            var baseSensivity = Random.Range(character.minSensitivity, character.maxSensitivity + 1);
            var stamina = baseSensivity;

            return stamina;
        }
    }

    private int AvoidRate
    {
        get
        {
            var baseAvoidRate = Random.Range(character.minAvoidRate, character.maxAvoidRate + 1);
            var avoidRate = baseAvoidRate;

            return avoidRate;
        }
    }

    private int Concentration
    {
        get
        {
            var baseConcentration = Random.Range(character.minConcentration, character.maxConcentration + 1);
            var concentration = baseConcentration;

            return concentration;
        }
    }

    private int Willpower
    {
        get
        {
            var baseWillpower = Random.Range(character.minWillpower, character.maxWillpower + 1);
            var willpower = baseWillpower;

            return willpower;
        }
    }
    #endregion


    public void VirusPenaltyInit()
    {
        if (virusPenalty.Count == 0)
        {
            virusPenalty.Add("E", new VirusPenalty(ScriptableMgr.Instance.GetVirus("E"), character));
            virusPenalty.Add("B", new VirusPenalty(ScriptableMgr.Instance.GetVirus("B"), character));
            virusPenalty.Add("P", new VirusPenalty(ScriptableMgr.Instance.GetVirus("P"), character));
            virusPenalty.Add("I", new VirusPenalty(ScriptableMgr.Instance.GetVirus("I"), character));
            virusPenalty.Add("T", new VirusPenalty(ScriptableMgr.Instance.GetVirus("T"), character));
        }
    }

    public void Init()
    {
        // 스탯 초기화
        MaxHp           = Hp;    
        currentHp       = MaxHp;
        sensivity       = Sensivity;
        avoidRate       = AvoidRate + (sensivity * character.avoidRateRisePerSen);
        concentration   = Concentration;
        willpower       = Willpower;
        level           = 1;
        currentExp      = 0;
        totalExp        = ScriptableMgr.Instance.GetCharacterExp($"EXP_{level}").totalExp;
        sightDistance = 3;

        Weight = character.weight;
        accuracy = concentration * character.accurRatePerCon;
        critResistRate = willpower * character.critResistRateRise;
        alertAccuracy = (willpower / 3) * character.alertAccurRateRise;
        critRate = (willpower / 3) * character.critRateRise;

        buffMgr = new BuffMgr();
        skillMgr = new SkillMgr();

        VirusPenaltyInit();
    }

    public void StartGame()
    {
        // 무기 스탯 초기화
        weapon.Init();
        var skillList = skillMgr.GetPassiveSkills(PassiveCase.Ready);

        foreach (var skill in skillList)
        {
            skill.Invoke(buffMgr);
        }
    }

    public void StartTurn()
    {
        weapon.StartTurn();
        buffMgr.StartTurn();
    }

    public void GetExp(int exp)
    {
        if (level < MaxLevel)
        {
            currentExp += exp;

            if (currentExp >= totalExp)
            {
                LevelUp();
            }
        }
    }

    private void LevelUp()
    {
        level++; // 나중에 레벨업 시스템을 구축할 것.
        skillMgr.LevelUp();
        currentExp -= totalExp;
        totalExp = ScriptableMgr.Instance.GetCharacterExp($"EXP_{level}").totalExp;
        GrowStats(GetRandomStats());
    }

    private List<int> GetRandomStats()
    {
        var statsList = new List<int>();

        while (statsList.Count == 2)
        {
            var randomRate = Random.Range(0, 100);
            if (!statsList.Exists(x => x == 1))
            {
                if (randomRate < character.hpChance)
                {
                    statsList.Add(1);
                    if (statsList.Count >= 2)
                        break;
                }
            }

            randomRate = Random.Range(0, 100);
            if (!statsList.Exists(x => x == 2))
            {
                if (randomRate < character.concentrationChance)
                {
                    statsList.Add(2);
                    if (statsList.Count >= 2)
                        break;
                }
            }
            randomRate = Random.Range(0, 100);
            if (!statsList.Exists(x => x == 3))
            {
                if (randomRate < character.senChance)
                {
                    statsList.Add(3);
                    if (statsList.Count >= 2)
                        break;
                }
            }
            randomRate = Random.Range(0, 100);
            if (!statsList.Exists(x => x == 4))
            {
                if (randomRate < character.willChance)
                {
                    statsList.Add(4);
                    if (statsList.Count >= 2)
                        break;
                }
            }
        }
        return statsList;
    }

    private void GrowStats(List<int> statsList)
    {
        foreach (var stats in statsList)
        {
            switch (stats)
            {
                case 1:
                    MaxHp += character.hpRise;
                    Weight += character.weight_Rise;
                    break;
                case 2:
                    sensivity += character.senRise;
                    avoidRate = AvoidRate + (sensivity * character.avoidRateRisePerSen);
                    // 10, 20p 마다 시야 1 증가
                    // 15, 25 마다 mp 1 증가
                    break;
                case 3:
                    concentration += character.concentrationRise;
                    accuracy = concentration * character.accurRatePerCon;
                    break;
                case 4:
                    willpower += character.willRise;
                    critResistRate = willpower * character.critResistRateRise;
                    alertAccuracy = (willpower / 3) * character.alertAccurRateRise;
                    critRate = (willpower / 3) * character.critRateRise;
                    break;
            }
        }
    }
}
