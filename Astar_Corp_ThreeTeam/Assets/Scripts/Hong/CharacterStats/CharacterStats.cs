using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ĳ���� ����, ����, ��ų

// ĳ���� ���ȵ�
// 1. ü��
// 2. ����
// 3. ������
// 4. ȸ����
// 5. ���߷�
// 6. ���ŷ�
// 7. ġ��Ÿ Ȯ��
// 8. ������ ���� -> ����ġ, ���� ������, ���̷��� ������ ���ҷ�
// 9. ��� ���� �����

public class CharacterStats
{
    // �⺻ ����
    public Character character;

    // ���� ����
    public WeaponStats weapon;

    // ��ų
    public SkillMgr skillMgr;

    // ����
    public BuffMgr buffMgr;

    // 1. �⺻ ü��
    public int MaxHp; // { get => Hp; set => Hp = value; }
    [HideInInspector]
    public int currentHp;

    // 2. ����
    public int Weight;

    // 3. ������
    [HideInInspector]
    public int sensivity;

    // 4. ȸ����
    [HideInInspector]
    public int avoidRate;

    // 5. ���߷�
    [HideInInspector]
    public int concentration;

    // 6. ���߷¿� ���� �߰� ���߷�
    [HideInInspector]
    public int accuracy;

    // 7. ���ŷ�
    [HideInInspector]
    public int willpower;

    // 8. ġ��Ÿ Ȯ��
    [HideInInspector]
    public int critRate;

    // 9. ��� ���߷� ������
    public int alertAccuracy;

    // 10. ũ��Ƽ�� ������
    public int critResistRate;

    // 11. �þ� ����
    public int sightDistance;
    // ����
    [HideInInspector]
    public int level;
    private readonly int MaxLevel = 12;

    // ���� ����ġ ��� ����ġ �䱸��
    [HideInInspector]
    public int currentExp;
    [HideInInspector]
    public int totalExp;

    //���̷��� �г�Ƽ �� ����
    [HideInInspector]
    public Dictionary<string, VirusPenalty> virusPenalty = new Dictionary<string, VirusPenalty>();

    //����.
    public Dictionary<string, int> bag = new Dictionary<string, int>();
    public int bagLevel;
    public int currentWeight;

    //json ��ġ.
    public int saveId;

    public string Name 
    { 
        get { return character.name; } 
    }

    #region ĳ���� �⺻ ���� ����
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
        // ���� �ʱ�ȭ
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
        // ���� ���� �ʱ�ȭ
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
        level++; // ���߿� ������ �ý����� ������ ��.
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
                    // 10, 20p ���� �þ� 1 ����
                    // 15, 25 ���� mp 1 ����
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
