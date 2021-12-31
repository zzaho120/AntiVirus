using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ĳ���� ����, ��ü ����, ����, ��ų

// ĳ���� �⺻ ����
// 1. ü��
// 2. ������? Sensivity
// 3. ȸ����
// 4. ���߷�
// 5. ���ŷ�
// 6. ġ��Ÿ Ȯ��

// ����(��ü) ���� ���� ---> �����ɵ���
// 6. �ǰ� ���� ������
// 7. Ư���ɷ� ���׷�
// 8. Ư���ɷ� ���� ������
// 9. �޽� ���� ������

public class CharacterStats : MonoBehaviour
{
    // ��ũ���ͺ� ������Ʈ �ҷ�����
    //private ScriptableMgr scriptableMgr;

    // �⺻ ����
    public Character character;
    public List<Antibody> antibody;

    // ���� ����
    public WeaponStats weapon;
    //public Weapon mainWeapon;
    //public Weapon subWeapon;

    // ��ų
    public CharacterSkillList skills;

    // 1. ü��
    public int maxHp; // { get => Hp; set => Hp = value; }
    [HideInInspector]
    public int currentHp;

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

    public string Name 
    { 
        get { return character.name; } 
    }

    #region ������ �����ϴ� ���� ����
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

    #region ���� ���� ���� ����
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

    #region ��ü ���� - �����ʿ�
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
        // �׽�Ʈ�� ���� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(weapon.mainWeaponBullet);

            // ���� ���� �׽�Ʈ (������)
            Debug.Log(weapon.damage);
        }
    }

    public void Init()
    {
        // ���� �ʱ�ȭ
        maxHp           = Hp;    
        currentHp       = maxHp;
        sensivity       = Sensivity;
        avoidRate       = AvoidRate;
        concentration   = Concentration;
        willpower       = Willpower;
        critRate        = CritRate;
        level           = 1;

        // ���� ���� �ʱ�ȭ
        weapon.Init();
    }
}
