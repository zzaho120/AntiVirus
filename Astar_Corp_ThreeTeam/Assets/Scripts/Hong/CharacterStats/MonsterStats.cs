using UnityEngine;

// ���� ����

// ���� �⺻ ����
// 1. HP
// 2. ���� ���� �Ÿ� (Ÿ��)
// 3. AP
// 4. ���Կ� ���� �߰� AP
// 5. �������� �� �Ҹ� AP
// 6. ������
// 7. ũȮ
// 8. ũ��
// 9. ũ��Ƽ�� ���׷�
// 10. EXP
// 11. �ν� ���� (ĭ)
// 12. ���̷��� ������ ������
// 13. ����AI�� �ʿ� ü�� ���ҷ� ?

// ���������� ���� ����
// 1. �浹 �� ���� �ִ� ���� ��

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
    public WorldMonster nonBattleMonster;
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

    // ���� �߻� �� ����(?)�ϴ� ���� ��
    // �ּ� ���� �� ~ �ִ� ���� �� ���̿��� ���� ����
    public int BattleMonsterNum
    {
        get
        {
            var monsterNum = Random.Range(nonBattleMonster.battleMinNum, nonBattleMonster.battleMaxNum + 1);
            return monsterNum;
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
