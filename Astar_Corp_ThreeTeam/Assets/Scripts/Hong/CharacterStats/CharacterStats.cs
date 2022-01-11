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
    // �⺻ ����
    public Character character;

    // ���� ����
    public WeaponStats weapon;

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

    //���̷��� �г�Ƽ �� ����
    [HideInInspector]
    public Dictionary<string, VirusPenalty> virusPanalty = new Dictionary<string, VirusPenalty>();

    //����.
    public Dictionary<string, int> bag = new Dictionary<string, int>();

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
        VirusPanaltyInit();
    }

    public void StartTurn()
    {
        weapon.StartTurn();
    }
}
