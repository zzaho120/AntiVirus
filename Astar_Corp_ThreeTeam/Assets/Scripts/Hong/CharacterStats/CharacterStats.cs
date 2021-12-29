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
    // �⺻ ����
    public Character character;
    public Antibody antibody;

    // ���� ����
    public Equippable mainWeapon;
    public Equippable subWeapon;
    //public AttackDefinition weapon;

    // ��ų
    //public List<CharacterSkillList> skills;
    public CharacterSkillList skills;

    // 1. ü��
    public int maxHp; //{ get => Hp; set => Hp = value; }
    [HideInInspector]
    public int currentHp;

    // 2. ���ݷ�
    // 3. ���ݹ���

    // 4. ������
    [HideInInspector]
    public int willpower;

    // 5. ���
    [HideInInspector]
    public float stamina;

    // 6. ����

    [HideInInspector]
    public int level;

    public string Name 
    { 
        get { return character.name; } 
    }

    #region ������ �����ϴ� ���� ����
    private int Hp
    {
        get
        {
            var baseHp = Random.Range(character.min_Hp, character.max_Hp + 1);
            var hp = baseHp;
            //Debug.Log("ȣ��");
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

    #region ��Ÿ �⺻ ���� ����
    // ����? ���� �� �� ������ ���߿� �ٽ� ����
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

    public void Init()
    {
        maxHp = Hp;
        currentHp = maxHp;
        stamina = Stamina;
        willpower = Willpower;
        level = 1;

    }

}
