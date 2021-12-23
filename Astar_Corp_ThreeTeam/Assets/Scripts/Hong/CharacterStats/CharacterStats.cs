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
    public Character characterStat;
    public Antibody antibody;
    public AttackDefinition weapon;

    // 1. 체력
    private int maxHp { get => Hp; }
    [HideInInspector]
    public int currentHp;

    // 2. 공격력, 3. 공격범위 --> AttackStats, AttackDefition 클래스에서 추가 관리
    //public float Damage;
    
    //// 4. 의지력
    //public int willpower;
    //
    //// 5. 기력
    //public int stamina;
    //
    //// 6. 내성
    //public float resistance;

    // 경험치?
    // public float Exp;

    public string Name { get { return characterStat.name; } }

    private int Hp
    {
        get
        {
            var baseHp = Random.Range(characterStat.min_Hp, characterStat.max_Hp);
            var hp = baseHp;

            return hp;
        }
    }

    // 수정? 대충 된 것 같은데 나중에 다시 보기
    public int Damage
    {
        get
        {
            //var baseDamage = characterStat.damage;
            //var damage = baseDamage;
            var damage = weapon.CreateAttack(characterStat).Damage;

            return damage;
        }
    }

    public int Range
    {
        get
        {
            var baseRange = characterStat.range;
            var range = baseRange;

            return range;
        }
    }

    public float Crit_rate
    {
        get
        {
            var baseCritRate = characterStat.crit_rate;
            var critRate = baseCritRate;

            return critRate;
        }
    }

    public int Willpower
    {
        get
        {
            var baseWillpower = Random.Range(characterStat.min_Willpower, characterStat.max_Willpower);
            var willpower = baseWillpower;

            return willpower;
        }
    }
    
    public int Stamina
    {
        get
        {
            var baseStamina = Random.Range(characterStat.min_Stamina, characterStat.max_Stamina);
            var stamina = baseStamina;

            return stamina;
        }
    }

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

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        currentHp = maxHp;
    }
}
