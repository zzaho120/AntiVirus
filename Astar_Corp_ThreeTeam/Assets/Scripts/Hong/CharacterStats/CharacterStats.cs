using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 기본 스텟
// 1. 체력
// 2. 공격력
// 3. 분과별 공격 사정/유효 거리
// 4. 의지력
// 5. 기력

// 내성(항체) 관련 스탯
// 6. 피격 피해 감소율
// 7. 특수능력 저항력
// 8. 특수능력 피해 감소율
// 9. 급습 피해 감소율

public class CharacterStats : MonoBehaviour
{
    public Character characterStat;
    public Antibody antibody;

    // 1. 체력
    public int maxHp;
    public int currentHp;

    // 2. 공격력, 3. 공격범위 --> AttackStats, AttackDefition 클래스에서 추가 관리
    public float Damage;
    
    // 4. 의지력
    public int willpower;

    // 5. 기력
    public int stamina;

    // 6. 내성
    public float resistance;

    // 경험치?
    // public float Exp;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        currentHp = maxHp;
    }
}
