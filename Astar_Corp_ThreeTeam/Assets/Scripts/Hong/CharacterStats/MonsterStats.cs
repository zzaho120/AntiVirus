using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 기본 스탯
//public int damage;
//public int hp; 
//public int stamina;
//public int range; v

// 바이러스 추가 스탯
//public int damage;
//public int hp; 
//public int stamina;
//public int exp; v
//public int range; v

public class MonsterStats : MonoBehaviour
{
    public Monster monsterStat;
    public Virus virus;

    private int maxHp { get => Hp; }
    public int currentHp;

    public int Damage
    {
        get
        {
            var baseDamage = monsterStat.damage;
            if (virus != null)
            {
                var damage = baseDamage + virus.damage;
                return damage;
            }
            else
            {
                var damage = baseDamage;
                return damage;
            }
        }
    }

    private int Hp
    {
        get
        {
            var baseHp = monsterStat.hp;
            if (virus != null)
            {
                var Hp = baseHp + virus.hp;
                return Hp;
            }
            else
            {
                var Hp = baseHp;
                return Hp;
            }
        }
    }

    public int Stamina
    {
        get
        {
            var baseStamina = monsterStat.stamina;
            if (virus != null)
            {
                var stamina = baseStamina + virus.stamina;
                return stamina;
            }
            else
            {
                var stamina = baseStamina;
                return stamina;
            }
        }
    }

    public int Range
    {
        get
        {
            var baseRange = monsterStat.range;
            if (virus != null)
            {
                var range = baseRange + virus.range;
                return range;
            }
            else
            {
                var range = baseRange;
                return range;
            }
        }
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
