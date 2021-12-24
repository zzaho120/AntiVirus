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
    public int maxHp { get => Hp; }
    [HideInInspector]
    public int currentHp;

    // 2. ���ݷ�
    // 3. ���ݹ���

    // 4. ������
    public int willpower;

    // 5. ���
    public float stamina;

    // 6. ����

    public int level;

    public string Name { get { return characterStat.name; } }

    #region ������ �����ϴ� ���� ����
    private int Hp
    {
        get
        {
            var baseHp = Random.Range(characterStat.min_Hp, characterStat.max_Hp + 1);
            var hp = baseHp;
            
            return hp;
        }
    }

    private int Stamina
    {
        get
        {
            var baseStamina = Random.Range(characterStat.min_Stamina, characterStat.max_Stamina + 1);
            var stamina = baseStamina;

            return stamina;
        }
    }

    private int Willpower
    {
        get
        {
            var baseWillpower = Random.Range(characterStat.min_Willpower, characterStat.max_Willpower + 1);
            var willpower = baseWillpower;

            return willpower;
        }
    }
    #endregion

    #region ��Ÿ �⺻ ���� ����
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
    #endregion

    #region ��ü ����
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

    private void Init()
    {
        currentHp = maxHp;
        stamina = Stamina;
        willpower = Willpower;
        level = 1;
    }

}
