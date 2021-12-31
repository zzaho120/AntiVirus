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
    // 스크립터블 오브젝트 불러오기
    //private ScriptableMgr scriptableMgr;

    // 기본 스탯
    public Character character;
    public List<Antibody> antibody;

    // 무기 스탯
    public WeaponStats weapon;
    //public Weapon mainWeapon;
    //public Weapon subWeapon;

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

    public string Name 
    { 
        get { return character.name; } 
    }

    #region 레벨당 증가하는 스탯 설정
    private int Hp
    {
        get
        {
            Debug.Log(character);
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

    #region 무기 관련 스탯 설정
    //public int AccurRate_Base
    //{
    //    get
    //    {
    //
    //    }
    //}

    //public int Damage
    //{
    //    get
    //    {
    //        
    //    }
    //}
    
    //public int Range
    //{
    //    get
    //    {
    //        var baseRange = mainWeapon.range;
    //        var range = baseRange;
    //
    //        return range;
    //    }
    //}
    //
    //private int OverRange_Penalty
    //{
    //    get
    //    {
    //        return mainWeapon.overRange_Penalty;
    //    }
    //}
    //
    //private int UnderRange_Penalty
    //{
    //    get
    //    {
    //        return mainWeapon.underRange_Penalty;
    //    }
    //}


    #endregion

    #region 항체 스탯 - 수정필요
    public float HitDmgDecRate
    {
        get 
        {
            float sum = 0;
            foreach (var element in antibody)
            {
                sum += element.hitDmgDecRate;
            }
            return sum;
        }
    }

    public float VirusSkillResist
    {
        get 
        {
            float sum = 0;
            foreach (var element in antibody)
            {
                sum += element.virusSkillResist;
            }
            return sum; 
        }
    }

    public float VirusDmgDecRate
    {
        get 
        {
            float sum = 0;
            foreach (var element in antibody)
            {
                sum += element.virusDmgDecRate;
            }
            return sum;
        }
    }

    public float SuddenDmgDecRate
    {
        get 
        {
            float sum = 0;
            foreach (var element in antibody)
            {
                sum += element.suddenDmgDecRate;
            }
            return sum;
        }
    }
    #endregion

    private void Start()
    {
        //Init();
    }

    private void Update()
    {
        // 테스트용 스탯 설정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(weapon.mainWeaponBullet);

            // 무기 스탯 테스트 (데미지)
            Debug.Log(weapon.damage);
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
    }
}
