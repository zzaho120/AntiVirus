using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    public string id;
    public string iconId;
    public string prefabId;

    public string type;
    public string kind;
    public new string name;

    public int accur_Rate_Base;     // 기본 명중률

    public int min_damage;          // 최소 데미지
    public int max_damage;          // 최대 데미지
    public int crit_Damage;         // 크리티컬 데미지 (데미지 * 계수)%

    public int bullet;              // 탄창 클립수
    public int accur_Rate_Dec;      // 명중률 감소
    /// <summary>
    /// Mp 소모량
    /// </summary>
    public int weight;              // 이동시 소모되는 MP 양

    public int firstShot_Ap;        // 새로운 경계, 조준 사격 시 소모 AP
    public int alertShot_Ap;        // 경계 시 첫 사격 이후 소모되는 AP
    public int aimShot_Ap;          // 조준사격 시 첫 사격 이후 소모되는 AP
    public int load_Ap;             // 장전 시 소모되는 AP

    public int range;               // 사거리
    public int overRange_Penalty;   // 사거리 +n타일에 따른 명중률 감소
    public int underRange_Penalty;  // 사거리 -n타일에 따른 명중률 감소

    //private Sprite iconSprite;

    //public Sprite IconSprite
    //{
    //    get { return iconSprite; }
    //}

    // Attack 수치 조정용
    //public AttackStats CreateAttack(Character player)
    //{
    //    // 데미지 조정
    //    // 수정 - 나중에 랜덤값 조절해보기
    //    var damage = player.damage;
    //    damage += Random.Range(damage - 10, damage + 10);
    //
    //    // 크리티컬 수치 조정
    //    var isCritical = Random.value < critRate;
    //    if (isCritical)
    //        damage *= critDamage;
    //
    //    return new AttackStats((int)damage, isCritical);
    //}
}
