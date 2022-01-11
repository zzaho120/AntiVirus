using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 스탯, 항체 스탯, 무기, 스킬

// 캐릭터 기본 스텟
// 1. 체력
// 2. 예민함? Sensivity
// 3. 회피율
// 4. 집중력
// 5. 정신력
// 6. 치명타 확률

// 내성(항체) 관련 스탯 ---> 수정될듯함
// 6. 피격 피해 감소율
// 7. 특수능력 저항력
// 8. 특수능력 피해 감소율
// 9. 급습 피해 감소율

public class CharacterStats : MonoBehaviour
{
    // 기본 스탯
    public Character character;

    // 무기 스탯
    public WeaponStats weapon;

    // 스킬
    public CharacterSkillList skills;

    // 1. 체력
    public int maxHp; // { get => Hp; set => Hp = value; }
    [HideInInspector]
    public int currentHp;

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
            var baseHp = Random.Range(character.min_Hp, character.max_Hp + 1);
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
            var baseSensivity = Random.Range(character.min_Sensitivity, character.max_Sensitivity + 1);
            var stamina = baseSensivity;

            return stamina;
        }
    }

    private int AvoidRate
    {
        get
        {
            var baseAvoidRate = Random.Range(character.min_Avoid_Rate, character.max_Avoid_Rate + 1);
            var avoidRate = baseAvoidRate;

            return avoidRate;
        }
    }

    private int Concentration
    {
        get
        {
            var baseConcentration = Random.Range(character.min_Concentration, character.max_Concentration + 1);
            var concentration = baseConcentration;

            return concentration;
        }
    }

    private int Willpower
    {
        get
        {
            var baseWillpower = Random.Range(character.min_Willpower, character.max_Willpower + 1);
            var willpower = baseWillpower;

            return willpower;
        }
    }

    private int CritRate
    {
        get
        {
            var baseCritRate = Random.Range(character.min_Crit_Rate, character.max_Crit_Rate + 1);
            var critRate = baseCritRate;

            return critRate;
        }
    }
    #endregion


    public void VirusPanaltyInit()
    {
        if (virusPanalty.Count == 0)
        {
            virusPanalty.Add("E", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/E"))));
            virusPanalty.Add("B", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/B"))));
            virusPanalty.Add("P", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/P"))));
            virusPanalty.Add("I", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/I"))));
            virusPanalty.Add("T", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/T"))));
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
        maxHp           = Hp;    
        currentHp       = maxHp;
        sensivity       = Sensivity;
        avoidRate       = AvoidRate;
        concentration   = Concentration;
        willpower       = Willpower;
        critRate        = CritRate;
        level           = 1;

        // 무기 스탯 초기화
        weapon.Init();
        VirusPanaltyInit();
    }

    public void StartTurn()
    {
        weapon.StartTurn();
    }
}
