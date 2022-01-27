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

    // 1-1. ���� �� ���õ� Ȯ��
    public int HpChance;

    // 1-2. ���õ� �� �����ϴ� ü��
    public int HpRise;

    // 2. ����
    public int Weight;

    // 2. ������
    [HideInInspector]
    public int sensivity;

    // 3. ȸ����
    [HideInInspector]
    public int avoidRate;

    // 4. ���߷�
    [HideInInspector]
    public int concentration;

    // 5. ���ŷ�
    [HideInInspector]
    public int willpower;

    // 6. ġ��Ÿ Ȯ��
    [HideInInspector]
    public int critRate;

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
        avoidRate       = AvoidRate;
        concentration   = Concentration;
        willpower       = Willpower;
        level           = 1;
        currentExp      = 0;
        totalExp        = ScriptableMgr.Instance.GetCharacterExp($"EXP_{level}").totalExp;

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
    }

    private List<int> GetRandomStats()
    {
        var statList = new List<int>();

        while (statList.Count == 2)
        {
            var randomRate = Random.Range(0, 100);
            if (!statList.Exists(x => x == 1))
            {
                if (randomRate < character.hpChance)
                    statList.Add(1);
            }
            randomRate = Random.Range(0, 100);
            if (!statList.Exists(x => x == 2))
            {
                if (randomRate < character.concentrationChance)
                {
                    statList.Add(2);
                    if (statList.Count >= 2)
                        break;
                }
            }
            randomRate = Random.Range(0, 100);
            if (!statList.Exists(x => x == 3))
            {
                if (randomRate < character.senChance)
                    statList.Add(3);
            }
            randomRate = Random.Range(0, 100);
            if (!statList.Exists(x => x == 4))
            {
                if (randomRate < character.willChance)
                    statList.Add(4);
            }
        }
        return statList;
    }
}
