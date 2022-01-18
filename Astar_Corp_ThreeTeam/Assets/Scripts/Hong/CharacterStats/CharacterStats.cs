using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ĳ���� ����, ����, ��ų

// ĳ���� ���ȵ�
// 1. ü��
// 2. ����
// 3. ������
// 4. ȸ����
// 5. ���߷�
// 6. ���ŷ�
// 7. ġ��Ÿ Ȯ��
// 8. ������ ���� -> ����ġ, ���� ������, ���̷��� ������ ���ҷ�
// 9. ��� ���� �����

public class CharacterStats : MonoBehaviour
{
    // �⺻ ����
    public Character character;

    // ���� ����
    public WeaponStats weapon;

    // ��ų
    public CharacterSkillList skills;


    // 1. �⺻ ü��
    public int MaxHp; // { get => Hp; set => Hp = value; }
    [HideInInspector]
    public int currentHp;
    // 1-1. ���� �� ���õ� Ȯ��
    public int HpChance;

    // 1-2. ���õ� �� �����ϴ� ü��
    public int HpRise;

    // 2. ����
    public int Weight;

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

    //���̷��� �г�Ƽ �� ����
    [HideInInspector]
    public Dictionary<string, VirusPenalty> virusPanalty = new Dictionary<string, VirusPenalty>();

    //����.
    public Dictionary<string, int> bag = new Dictionary<string, int>();
    public int bagLevel;
    public int currentWeight;

    //json ��ġ.
    public int saveId;

    public string Name 
    { 
        get { return character.name; } 
    }

    #region ĳ���� �⺻ ���� ����
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
            virusPanalty.Add("E", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/E"))));
            virusPanalty.Add("B", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/B"))));
            virusPanalty.Add("P", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/P"))));
            virusPanalty.Add("I", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/I"))));
            virusPanalty.Add("T", new VirusPenalty((Virus)Instantiate(Resources.Load("Choi/Datas/Viruses/T"))));
        }
    }

    private void Update()
    {
        // �׽�Ʈ�� ���� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(weapon.mainWeaponBullet);

            // ���� ���� �׽�Ʈ (������)
            Debug.Log(weapon.Damage);
        }
    }

    public void Init()
    {
        // ���� �ʱ�ȭ
        MaxHp           = Hp;    
        currentHp       = MaxHp;
        sensivity       = Sensivity;
        avoidRate       = AvoidRate;
        concentration   = Concentration;
        willpower       = Willpower;
        //critRate        = CritRate;
        level           = 1;

        // ���� ���� �ʱ�ȭ
        weapon.Init();
        VirusPanaltyInit();
    }

    public void StartTurn()
    {
        weapon.StartTurn();
    }
}
