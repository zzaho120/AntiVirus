using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equippable : ScriptableObject
{
    public string id;
    public string iconId;
    public string prefabId;
    public string name;
    public string description;
    public string type;
    public int damage;
    public int critRate;
    public int critDamage;
    public int cost;
    public int weight;
    public int str;
    public int con;
    public int intellet;
    public int luck;
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
