using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ĳ���� �⺻ ����
// 1. ü��
// 2. ���ݷ�
// 3. �а��� ���� ����/��ȿ �Ÿ�
// 4. ������
// 5. ���

// ����(��ü) ���� ����
// 6. �ǰ� ���� ������
// 7. Ư���ɷ� ���׷�
// 8. Ư���ɷ� ���� ������
// 9. �޽� ���� ������

public class CharacterStats : MonoBehaviour
{
    public Character characterStat;
    public Antibody antibody;
    public AttackDefinition weapon;

    // 1. ü��
    private int maxHp { get => Hp; }
    [HideInInspector]
    public int currentHp;

    // 2. ���ݷ�, 3. ���ݹ��� --> AttackStats, AttackDefition Ŭ�������� �߰� ����
    //public float Damage;
    
    //// 4. ������
    //public int willpower;
    //
    //// 5. ���
    //public int stamina;
    //
    //// 6. ����
    //public float resistance;

    // ����ġ?
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

    // ����? ���� �� �� ������ ���߿� �ٽ� ����
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
