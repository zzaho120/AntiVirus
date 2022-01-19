using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 기본 스탯
// 1. HP
// 2. 공격 가능 거리 (타일)
// 3. AP
// 4. 무게에 따른 추가 AP
// 5. 근접공격 시 소모 AP
// 6. 데미지
// 7. 크확
// 8. 크뎀
// 9. 크리티컬 저항률
// 10. EXP
// 11. 인식 범위 (칸)
// 12. 바이러스 게이지 증가량
// 13. 도망AI시 필요 체력 감소량 ?

// 바이러스 스탯
// 1. HP
// 2. AP
// 3. 데미지
// 4. 크확
// 5. 크뎀
// 6. EXP
// 7. 내성 게이지

public class MonsterStats : MonoBehaviour
{
    public Monster monster;
    public Virus virus;

    public int virusLevel;

    private int maxHp { get => Hp; }

    [HideInInspector]
    public int currentHp;
    [HideInInspector]
    public int currentAp;


    private int Hp
    {
        get
        {
            var baseHp = Random.Range(monster.minHp, monster.maxHp + 1);
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

    public int AtkRange
    {
        get
        {
            return monster.atkRange;
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

    public int Mp
    {
        get
        {
            return monster.mp;
        }
    }

    public int CloseAttackAp
    {
        get
        {
            return monster.closeUpAtkAp;
        }
    }

    public int Damage
    {
        get
        {
            var baseDamage = Random.Range(monster.minDmg, monster.maxDmg + 1);
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
            var baseRate = Random.Range(monster.minCritRate, monster.maxCritRate + 1);
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

    public int critResist
    {
        get
        {
            return monster.critResist;
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

    private int VirusGauge
    {
        get 
        {
            var baseVirusGauge = monster.virusGauge;

            if (virus != null)
            {
                var virusGauge = baseVirusGauge + virus.virusGauge;
                return virusGauge;
            }
            else
            {
                return baseVirusGauge;
            }
        }
    }

    public int EscapeHpDec
    {
        get
        {
            return monster.escapeHpDec;
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

    public void StartTurn()
    {
        currentAp = Ap;
    }

    public bool CheckAttackAp()
    {
        return currentAp >= CloseAttackAp;
    }

    public void CalculateAttackAp()
    {
        currentAp -= CloseAttackAp;
    }

    public bool CheckRunMonster()
    {
        return currentHp < maxHp * (monster.escapeHpDec / 100f);
    }
}
