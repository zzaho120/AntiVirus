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

    public int accur_Rate_Base;
    public int accur_Rate_Alert;

    public int unstability;
    public int min_damage;
    public int max_damage;
    public int crit_Damage;

    public int bullet;
    public int recoil;
    public int accur_Rate_Dec;
    public int weight;

    public int firstAp;
    public int nextAp;
    public int loadAp;

    public int range;
    public int overRange_Penalty;
    public int underRange_Penalty;

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
