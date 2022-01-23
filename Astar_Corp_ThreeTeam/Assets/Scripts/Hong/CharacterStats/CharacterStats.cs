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

public class CharacterStats : MonoBehaviour
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

    //바이러스 패널티 및 내성
    [HideInInspector]
    public Dictionary<string, VirusPenalty> virusPanalty = new Dictionary<string, VirusPenalty>();

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
        if (virusPanalty.Count == 0)
        {
            virusPanalty.Add("E", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/E")), character));
            virusPanalty.Add("B", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/B")), character));
            virusPanalty.Add("P", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/P")), character));
            virusPanalty.Add("I", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/I")), character));
            virusPanalty.Add("T", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/T")), character));
        }
    }

    private void Update()
    {
        // 테스트용 스탯 설정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(weapon.mainWeaponBullet);

            // 무기 스탯 테스트 (데미지)
            Debug.Log(weapon.Damage);
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

        buffMgr = new BuffMgr();

        // 무기 스탯 초기화
        weapon.Init();
        VirusPanaltyInit();

        // test
        //var skilltest = ScriptableMgr.Instance.passiveSkillList["PSK_0001"];
        var skilltest = ScriptableMgr.Instance.passiveSkillList["PSK_0009"];

        skillMgr.AddSkill(SkillType.Passive, skilltest);

        var skillList = skillMgr.GetPassiveSkills(skilltest.skillCase);

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
}
