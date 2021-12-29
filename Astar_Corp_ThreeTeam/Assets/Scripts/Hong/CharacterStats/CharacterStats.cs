using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 기본 스텟
// 1. 체력
// 2. 공격력
// 3. 분과별 공격 사정/유효 거리
// 4. 의지력
// 5. 기력

// 내성(항체) 관련 스탯
// 6. 피격 피해 감소율
// 7. 특수능력 저항력
// 8. 특수능력 피해 감소율
// 9. 급습 피해 감소율

public class CharacterStats : MonoBehaviour
{
    // 기본 스탯
    public Character character;
    public Antibody antibody;

    // 무기 스탯
    public Equippable mainWeapon;
    public Equippable subWeapon;
    //public AttackDefinition weapon;

    // 스킬
    //public List<CharacterSkillList> skills;
    public CharacterSkillList skills;

    // 1. 체력
    public int maxHp; //{ get => Hp; set => Hp = value; }
    [HideInInspector]
    public int currentHp;

    // 2. 공격력
    // 3. 공격범위

    // 4. 의지력
    [HideInInspector]
    public int willpower;

    // 5. 기력
    [HideInInspector]
    public float stamina;

    // 6. 내성

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
            var baseHp = Random.Range(character.min_Hp, character.max_Hp + 1);
            var hp = baseHp;
            //Debug.Log("호출");
            return hp;
        }
    }

    private int Stamina
    {
        get
        {
            var baseStamina = Random.Range(character.min_Stamina, character.max_Stamina + 1);
            var stamina = baseStamina;

            return stamina;
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
    #endregion

    #region 기타 기본 스탯 설정
    // 수정? 대충 된 것 같은데 나중에 다시 보기
    public int Damage
    {
        get
        {
            var baseDamage = character.damage;
            var damage = baseDamage;

            //var damage = weapon.CreateAttack(character).Damage;

            if (mainWeapon != null)
            
               damage += mainWeapon.damage;
            
            if (subWeapon != null)
            
               damage += subWeapon.damage;
            
            return damage;
        }
    }

    public int Range
    {
        get
        {
            var baseRange = character.range;
            var range = baseRange;

            return range;
        }
    }

    public float Crit_rate
    {
        get
        {
            var baseCritRate = character.crit_rate;
            var critRate = baseCritRate;

            return critRate;
        }
    }
    #endregion

    #region 항체 스탯
    public float HitDmgDecRate
    {
        get { return antibody.hitDmgDecRate; }
    }

    public float VirusSkillResist
    {
        get { return antibody.virusSkillResist; }
    }

    public float VirusDmgDecRate
    {
        get { return antibody.virusDmgDecRate; }
    }

    public float SuddenDmgDecRate
    {
        get { return antibody.suddenDmgDecRate; }
    }
    #endregion

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        maxHp = Hp;
        currentHp = maxHp;
        stamina = Stamina;
        willpower = Willpower;
        level = 1;

    }

}
