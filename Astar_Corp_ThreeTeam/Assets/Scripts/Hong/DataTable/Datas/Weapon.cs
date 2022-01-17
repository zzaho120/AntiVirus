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

    public int accurRateBase;       // 기본 명중률

    public int minDamage;           // 최소 데미지
    public int maxDamage;           // 최대 데미지
    public int critRate;            // 크리티컬 확률
    public int critDamage;          // 크리티컬 데미지 (데미지 * 계수)%

    public int bullet;              // 탄창 클립수
    public int mpPerAp;             // 1 AP당 제공 MP

    public int firstShotAp;         // 사격 첫발 시 소모 AP
    public int otherShotAp;         // 첫발 이후 소모되는 AP
    public int loadAp;              // 장전 시 소모되는 AP

    public int minRange;            // 최소 사거리
    public int maxRange;            // 최대 사거리
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
