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

    // 1-1. 렙업 시 선택될 확률
    public int HpChance;

    // 1-2. 선택될 시 증가하는 체력
    public int HpRise;

    // 2. 무게
    public int Weight;

    // 2. 예민함
    [HideInInspector]
    public int sensivity;

    // 3. 회피율
    [HideInInspector]
    public int avoidRate;

    // 4. 집중력
    [HideInInspector]
    public int concentration;

    // 5. 정신력
    [HideInInspector]
    public int willpower;

    // 6. 치명타 확률
    [HideInInspector]
    public int critRate;

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


    public void VirusPanaltyInit()
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
        avoidRate       = AvoidRate;
        concentration   = Concentration;
        willpower       = Willpower;
        //critRate        = CritRate;
        level           = 1;
        currentExp = 0;
        totalExp = ScriptableMgr.Instance.GetCharacterExp("EXP_0001").totalExp;

        buffMgr = new BuffMgr();
        skillMgr = new SkillMgr();

        VirusPanaltyInit();
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
        currentExp -= totalExp;
        totalExp = ScriptableMgr.Instance.GetCharacterExp($"EXP_{level}").totalExp;
    }
}
