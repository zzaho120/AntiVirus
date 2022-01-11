using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �⺻ ����
// 1. HP
// 2. AP
// 3. �������� �� �Ҹ� AP
// 4. ������
// 5. ũȮ
// 6. ũ��
// 7. EXP

// ���̷��� ����
// 1. HP
// 2. AP
// 3. ������
// 4. ũȮ
// 5. ũ��
// 6. EXP
// 7. ���� ������

public class MonsterStats : MonoBehaviour
{
    public Monster monster;
    public Virus virus;

    public int virusLevel;

    private int maxHp { get => Hp; }
    [HideInInspector]
    public int currentHp;


    private int Hp
    {
        get
        {
            var baseHp = Random.Range(monster.min_Hp, monster.max_Hp + 1);
            var hp = baseHp;

            if (virus != null)
            {
                hp = baseHp + (virus.hp * virusLevel);
                return hp;
            }
            else
            {
                var Hp = baseHp;
                return Hp;
            }
        }
    }

    private int Ap
    {
        get 
        {
            int baseAp = monster.ap;
            int ap = baseAp;

            if (virus != null)
            {
                ap = ap + (virus.ap * virusLevel);
                return ap;
            }
            else
            {
                return ap;
            }
        }
    }

    public int CloseAttackAp
    {
        get
        {
            return monster.closeUpAtk_Ap;
        }
    }

    public int Damage
    {
        get
        {
            var baseDamage = Random.Range(monster.min_Dmg, monster.max_Dmg + 1);
            var damage = baseDamage;

            if (virus != null)
            {
                damage = damage + (virus.damage * virusLevel);
                return damage;
            }
            else
            {
                return damage;
            }
        }
    }

    public int Crit_Rate
    {
        get
        {
            var baseRate = Random.Range(monster.min_CritRate, monster.max_CritRate + 1);
            var critRate = baseRate;

            if (virus != null)
            {
                critRate = critRate + (virus.crit_Rate * virusLevel);
                return critRate;
            }
            else
            {
                return critRate;
            }
        }
    }
    
    public int CritDmg
    {
        get
        {
            var baseCritDmg = monster.critDmg;
            var critDmg = baseCritDmg;

            if (virus != null)
            {
                critDmg = critDmg + (virus.crit_Dmg * virusLevel);
                return critDmg;
            }
            else
            {
                return critDmg;
            }
        }
    }

    private int Exp
    {
        get
        {
            var baseExp = monster.exp;

            if (virus != null)
            {
                var exp = baseExp + (virus.exp * virusLevel);
                return exp;
            }
            else
            {
                return baseExp;
            }
        }
    }

    private int virusCharge
    {
        get { return virus.virusCharge; }
    }

    //private void Awake()
    //{
    //    Init();
    //}

    public void Init()
    {
        currentHp = maxHp;
    }
}
